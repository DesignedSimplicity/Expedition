using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Win.NavTree.Framework
{
    public class DossierReportInfo
    {
        public DossierTypes Type;
        public string Extension;
        public long TotalCount;
        public long TotalSize;
        public long LargestSize;
        public long SmallestSize;
        public long AverageSize { get { return TotalSize / TotalCount; } }

        public DateTime FirstCreated;
        public DateTime LastUpdated;
    }

    public class DossierReport : List<DossierReportInfo>
    {
        public DossierReportInfo GrandTotal = new DossierReportInfo();

        public DossierReport()
        {
        }

        public DossierReport(IList<DossierFileInfo> files)
        {
            foreach (DossierFileInfo file in files)
            {
                Add(file.Size, file.Extension, file.Type);
            }
        }

        public DossierReportInfo Get(string extension, DossierTypes? type = null)
        {
            if (String.IsNullOrWhiteSpace(extension)) extension = "";
            extension = extension.ToLowerInvariant();
            DossierReportInfo info = this.SingleOrDefault(t => t.Extension == extension);
            if (info == null)
            {
                info = new DossierReportInfo();
                info.Type = type.HasValue ? type.Value : Dossier.GetFileType(extension);
                info.Extension = extension;
                base.Add(info);
            }
            return info;
        }

        public DossierReportInfo Add(long size, string extension, DossierTypes? type = null)
        {
            GrandTotal.TotalCount++;
            GrandTotal.TotalSize += size;

            DossierReportInfo info = Get(extension, type);

            info.TotalCount++;
            info.TotalSize += size;
            if (size > info.LargestSize) info.LargestSize = size;
            if (size < info.SmallestSize) info.SmallestSize = size;

            return info;
        }

        public new DossierReportInfo Add(DossierReportInfo add)
        {
            GrandTotal.TotalCount += add.TotalCount;
            GrandTotal.TotalSize += add.TotalSize;

            DossierReportInfo info = Get(add.Extension);

            info.TotalCount += add.TotalCount;
            info.TotalSize += add.TotalSize;
            if (add.LargestSize > info.LargestSize) info.LargestSize = add.LargestSize;
            if (add.SmallestSize < info.SmallestSize) info.SmallestSize = add.SmallestSize;

            return info;
        }

        public void Add(DossierReport add)
        {
            foreach (DossierReportInfo info in add)
            {
                this.Add(info);               
            }
        }

        public DossierReport GetTypeSummaryReport()
        {
            DossierReport summary = new DossierReport();
            DossierReportInfo audio = summary.Get(DossierTypes.Audio.ToString(), DossierTypes.Audio);
            DossierReportInfo image = summary.Get(DossierTypes.Image.ToString(), DossierTypes.Image);
            DossierReportInfo video = summary.Get(DossierTypes.Video.ToString(), DossierTypes.Video);
            DossierReportInfo text = summary.Get(DossierTypes.Text.ToString(), DossierTypes.Text);
            DossierReportInfo data = summary.Get(DossierTypes.File.ToString(), DossierTypes.File);
            foreach (DossierReportInfo info in this)
            {
                summary.Add(info.TotalSize, info.Type.ToString(), info.Type).TotalCount += info.TotalCount - 1;
            }
            return summary;
        }
    }
}
