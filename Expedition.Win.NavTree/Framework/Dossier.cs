using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Win.NavTree.Framework
{
    public enum DossierTypes { Drive = 0, Root = 1, Folder = 10, File = 100, Text = 101, Audio = 102, Image = 3, Video = 104 }
    public enum DossierStatus { Unknown = 0, Normal = 1, Created = 2, Updated = 3, Deleted = 4, Corrupted = 5, Denied = 6 }

    public enum SizeDisplayUnits { Bytes = 0, KB = 1, MB = 2, GB = 3, TB = 4, PB = 5, EB = 6, ZB = 7, YB = 8 }


    interface IFileDataInfo
    {
        string DisplayName { get; }
        string DisplayType { get; }
        string DisplaySize { get; }
        string DisplayDate { get; }
        string DisplayInfo { get; }
    }

    public class Dossier
    {
        public const string FileExtensionsTextDefault = "txt,log,nfo,md5,csv,xml,xls,html,info,idx,srt,sub";
        public const string FileExtensionsAudioDefault = "mp3,wmv,wav,ogg,aif,flac";
        public const string FileExtensionsImageDefault = "png,jpg,jpeg,gif,tif,tiff,psd,hdr";
        public const string FileExtensionsVideoDefault = "avi,mov,wmv,mp4,m4v,mkv,flv,mpg,mpeg,h264,divx,qt";

        public static string[] FileExtensionsText;
        public static string[] FileExtensionsAudio;
        public static string[] FileExtensionsImage;
        public static string[] FileExtensionsVideo;

        static Dossier()
        {
            FileExtensionsText = FileExtensionsTextDefault.Split(',');
            FileExtensionsAudio = FileExtensionsAudioDefault.Split(',');
            FileExtensionsImage = FileExtensionsImageDefault.Split(',');
            FileExtensionsVideo = FileExtensionsVideoDefault.Split(',');
        }

        public static DossierTypes GetFileType(string extension)
        {
            if (FileExtensionsText.Contains(extension.ToLowerInvariant()))
                return DossierTypes.Text;
            else if (FileExtensionsAudio.Contains(extension.ToLowerInvariant()))
                return DossierTypes.Audio;
            else if (FileExtensionsImage.Contains(extension.ToLowerInvariant()))
                return DossierTypes.Image;
            else if (FileExtensionsVideo.Contains(extension.ToLowerInvariant()))
                return DossierTypes.Video;
            else
                return DossierTypes.File;
        }

        public static Guid CalculateMd5(string uri)
        {
            FileStream file = File.Open(uri, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(file);
            file.Close();
            return new Guid(hash);
        }

        public static long GetUnitBytes(SizeDisplayUnits units)
        {
            return Convert.ToInt64(Math.Pow(2, (10 * (int)units)));
        }

        public static string BytesDisplayText(long bytes, bool padded = false)
        {
            int factor = 0;
            decimal size = bytes;
            while (size > 1024)
            {
                size = size / 1024;
                factor++;
            }

            string units = Enum.GetName(typeof(SizeDisplayUnits), (SizeDisplayUnits)factor);
            if (padded)
                return String.Format("{0:000.0} {1}", size, units);
            else
                return String.Format("{0:0.#} {1}", size, units);
        }

        public static string BytesDisplayText(long bytes, SizeDisplayUnits factor)
        {
            double div = Math.Pow(2, (10 * (int)factor));

            string units = Enum.GetName(typeof(SizeDisplayUnits), (SizeDisplayUnits)factor);
            return String.Format("{0:0.0} {1}", bytes / div, units);
        }

        public static DriveInfo GetDriveByLetter(char letter)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && Char.ToUpperInvariant(drive.Name[0]) == Char.ToUpperInvariant(letter))
                {
                    return drive;
                }
            }
            return null;
        }

		public static string GetFileExtension(string filename)
		{
			int index = filename.LastIndexOf('.');
			return (index > 0 ? filename.Substring(index + 1) : "");
		}

		public static string GetFileNameWithoutExtension(string filename)
		{
			int index = filename.LastIndexOf('.');
			return (index > 0 ? filename.Substring(0, index) : "");
		}
	}
}
