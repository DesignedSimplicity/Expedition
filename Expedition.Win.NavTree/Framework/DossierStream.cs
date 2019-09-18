using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Expedition.Win.NavTree.Framework
{
    public enum DossierEventTypes { Error = -1, Message = 0, Created = 1, Updated = 2, Verified = 3 }

    public class DossierEventArgs : EventArgs
    {
        public DossierEventTypes EventType;
        public DossierInfo Item;
        public Exception Error;        
        public string Message;

        public DossierEventArgs(string message)
        {
            EventType = DossierEventTypes.Message;
            Message = message;
        }

        public DossierEventArgs(Exception error)
        {
            EventType = DossierEventTypes.Error;
            Error = error;
            Message = error.Message; ;
        }

        public DossierEventArgs(DossierInfo item)
        {
            EventType = DossierEventTypes.Created;
            Item = item;
            Message = String.Format("{0} {1}", EventType, item.FullPath);
        }

        public DossierEventArgs(DossierInfo item, DossierEventTypes type)
        {
            EventType = type;
            Item = item;
            Message = String.Format("{0} {1}", EventType, item.FullPath);
        }
    }

    /// <summary>
    /// Threadsafe event driven static methods to Export a stream for FileSystem objects
    /// </summary>
    public class DossierStream
    {
        /// <summary>
        /// Populates the children of a valid FileData folder object
        /// </summary>
        //public static DossierInfo[] LoadDirectory(string directory, bool recursive = false, bool excludeFiles = false) { return LoadDirectory(new 
        public static DossierInfo[] LoadFolder(DossierFolderInfo folder, bool recursive = false, bool excludeFiles = false, EventHandler<DossierEventArgs> events = null)        
        {
            List<DossierInfo> list = new List<DossierInfo>();
            DirectoryInfo dir = new DirectoryInfo(folder.FullPath);
            if (!excludeFiles)
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    DossierFileInfo f = new DossierFileInfo(file);
                    f.Parent = folder;
                    folder.Files.Add(f);
                    list.Add(f);
                    if (events != null) events(null, new DossierEventArgs(f));
                }
            }
            foreach (DirectoryInfo sub in dir.GetDirectories())
            {
                DossierFolderInfo subfolder = new DossierFolderInfo(sub);
                subfolder.Parent = folder;
                folder.Folders.Add(subfolder);
                list.Add(subfolder);
                if (events != null) events(null, new DossierEventArgs(subfolder));

                if (recursive) list.AddRange(LoadFolder(subfolder, recursive, excludeFiles, events));
            }

            return list.ToArray();
        }


        /// <summary>
        /// Recreates all command line options for dir /?
        /// </summary>
        /// <returns>An array of all FileSystem objects created starting with a single root</returns>
        public static DossierInfo[] Dir(string query, EventHandler<DossierEventArgs> events = null)
        {
            // *.* OR *.ext OR *.ext /s OR *.* /s OR *. /s OR *sub*.ext /s OR start*.ext /s
            bool recusive = (query.Contains(@"/s", true) || query.Contains(@"/r", true));
            query = query.Replace(@"/s", string.Empty).Replace(@"/r", string.Empty).Trim();

            string directory = "";
            if (query.Contains('*'))
            {
                int lastPath = query.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
                if (lastPath > 0)
                {
                    directory = query.Left(lastPath);
                    query = query.Substring(lastPath).Trim();
                }
            }
            else
            {
                directory = query;
                query = "";
            }

            bool dirOnly = false;
            string fileType = "";
            string fileName = "";
            string filePrefix = "";
            string fileSuffix = "";
            if (!query.IsNullOrWhiteSpace())
            {
                fileName = query;
                dirOnly = (query == "*.");
                if ((query != "*") && (query != "*.*"))
                {
                    int lastDot = fileName.LastIndexOf('.');
                    if (lastDot > 0)
                    {
                        fileType = query.Substring(lastDot);
                        if (fileType == ".") fileType = "";
                        fileName = fileName.Left(lastDot).Trim();
                    }
                    if (fileName.StartsWith("*") && query.EndsWith("*"))
                    {
                        filePrefix = "*";
                        fileSuffix = "*";
                        fileName = query.TrimStart('*').TrimEnd('*');
                    }
                    else if (query.StartsWith("*"))
                    {
                        filePrefix = "*";
                        fileSuffix = query.Substring(1);
                        fileName = "";
                    }
                    else if (query.EndsWith("*"))
                    {
                        filePrefix = query.Left(query.Length - 1);
                        fileSuffix = "*";
                        fileName = "";
                    }
                }
            }

            /*
            Console.WriteLine(String.Format("{0} = {1}", "Type", fileType));
            Console.WriteLine(String.Format("{0} = {1}", "Name", fileName));
            Console.WriteLine(String.Format("{0} = {1}", "Prefix", filePrefix));
            Console.WriteLine(String.Format("{0} = {1}", "Suffix", fileSuffix));
            */

            List<DossierInfo> list = new List<DossierInfo>();
            DirectoryInfo dir = new DirectoryInfo(directory);
            if (dir.Exists)
            {
                DossierRootInfo root = new DossierRootInfo(dir);
                list.Add(root);
                if (events != null) events(null, new DossierEventArgs(root));

                foreach (DirectoryInfo sub in dir.GetDirectories())
                {
                    DossierFolderInfo folder = new DossierFolderInfo(sub);
                    folder.Parent = root;
                    root.Folders.Add(folder);
                    list.Add(folder);
                    if (events != null) events(null, new DossierEventArgs(folder));
                }

                foreach (DossierFolderInfo sub in root.Folders)
                {
                    LoadFolder(sub, true, false, events);
                }
            }
            return list.ToArray();
        }
        /*
        public static DossierInfo[] Dir(string path, string expression = "", bool recursive = false, DateTime? since = null) { return null; }
        public static DossierInfo[] Dir(string path, DossierTypes[] types, bool recursive = false, DateTime? since = null) { return null; }
        public static DossierInfo[] Dir(string path, DossierTypes type, bool recursive = false, DateTime? since = null) { return null; }
        */

        /// <summary>
        /// Verifies the existance, size and dates of all folders and files from starting folder
        /// </summary>
        public static void Verify(DossierFolderInfo folder) { }
        public static void Verify(DossierFileInfo[] files) { }

        /// <summary>
        /// Refreshes the size and dates of all folders and files from starting folder
        /// </summary>
        public static void Refresh(DossierFolderInfo folder) { }
        public static void Refresh(DossierFileInfo[] files) { }

        /// <summary>
        /// Creates or verifies all files and updates FileSystem properties
        /// </summary>
        public static void CreateMD5(DossierFolderInfo folder) { CreateMD5(folder.GetAllFiles()); }
        public static void CreateMD5(List<DossierFileInfo> files)
        {
            foreach (DossierFileInfo file in files)
            {
                file.Hash = Dossier.CalculateMd5(file.FullPath);
                file.IsVerified = true;
                file.Verified = DateTime.Now;
                file.Status = DossierStatus.Normal;
            }
        }

        public static List<DossierFileInfo> VerifyMD5(DossierFolderInfo folder) { return VerifyMD5(folder.GetAllFiles()); }
        public static List<DossierFileInfo> VerifyMD5(List<DossierFileInfo> files)
        {
            List<DossierFileInfo> changes = new List<DossierFileInfo>();

            foreach (DossierFileInfo file in files)
            {
                Guid hash = Dossier.CalculateMd5(file.FullPath);
                file.IsVerified = true;
                file.Verified = DateTime.Now;
                file.Status = DossierStatus.Normal;
                if (hash != file.Hash)
                {
                    file.IsVerified = false;
                    file.Verified = null;
                    file.Status = DossierStatus.Corrupted;
                    changes.Add(file);
                }
            }

            return changes;
        }

        /// <summary>
        /// Recursively exports data from folder into specified format
        /// </summary>
        // full path of each directory and file on a new line with counts and sizes
        public static void ExportTXT(string uri, DossierFolderInfo folder, bool simple = true)
        {
            TextWriter txt = File.CreateText(uri);
            txt.WriteLine(uri);
            ExportFolderTXT(txt, folder, simple);
            txt.Close();
        }
        private static void ExportFolderTXT(TextWriter txt, DossierFolderInfo folder, bool simple)
        {
            if (simple)
                txt.WriteLine(folder.FullPath);
            else
            {
                txt.WriteLine();
                txt.WriteLine("{0}\t{1}({2})", Dossier.BytesDisplayText(folder.ReportRecursive.GrandTotal.TotalSize).PadLeft(8), folder.Name, folder.ReportRecursive.GrandTotal.TotalCount);
            }
            foreach (DossierFileInfo file in folder.Files.OrderBy(f => f.Name))
            {
                if (simple)
                    txt.WriteLine(file.FullPath);
                else
                    txt.WriteLine("{0}\t{1}\t{2}", Dossier.BytesDisplayText(file.Size).PadLeft(8), file.Extension, file.Name);
            }
            foreach (DossierFolderInfo sub in folder.Folders.OrderBy(f => f.Name))
            {
                ExportFolderTXT(txt, sub, simple);
            }
        }

        // folder path, base file name, extension, file type, file size, hash, created, updated, verified
        public static void ExportCSV(string uri, DossierFolderInfo folder, bool includeSize = false, bool includeDates = false, bool includeAttribures = false)
        {
            TextWriter txt = File.CreateText(uri);
            txt.WriteLine("FolderPath, FileName, FileType, FileSize, DateCreated, DateModified");
            ExportFolderCSV(txt, folder, includeSize, includeDates, includeAttribures);
            txt.Close();
        }
        private static void ExportFolderCSV(TextWriter txt, DossierFolderInfo folder, bool includeSize = false, bool includeDates = false, bool includeAttribures = false)
        {            
            foreach (DossierFileInfo file in folder.Files.OrderBy(f => f.Name))
            {
                txt.Write("\"{0}\",\"{1}\",{2},", file.Parent.FullPath, file.Name, file.Type.ToString());
                txt.WriteLine("{0},{1},{2}", file.Size, file.Created, file.Modified);
            }
            foreach (DossierFolderInfo sub in folder.Folders.OrderBy(f => f.Name))
            {
                ExportFolderCSV(txt, sub);
            }
        }

        public static void ExportMD5(DossierFolderInfo folder, string uri, bool useFullPath = false) { } // creates standard md5 file for verification using any program
        public static void ExportXML(DossierFolderInfo folder, string uri) { } // creates custom xml DataSet file
        public static void ExportSQL(DossierFolderInfo folder, string uri) { } // creates custom sql database file



        
    }
}
