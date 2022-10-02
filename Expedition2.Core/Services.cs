using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition2.Core
{
	public class NewPatrolService
	{
		public SourcePatrolInfo Source;
		public List<FolderPatrolInfo> Folders;
		public List<FilePatrolInfo> Files;

		/// <summary>
		/// Creates new Patrol record at source, ensures read and write access, checks for existing patrol files
		/// </summary>
		public SourcePatrolInfo Start(string directoryUri)
		{
			return null;
		}

		/// <summary>
		/// Recursively reads all subdirectories for given source folder
		/// </summary>
		public IList<FolderPatrolInfo> Preview()
		{
			return null;
		}

		/// <summary>
		/// Recursively reads all files and metadata for given source folder
		/// </summary>
		public IList<FilePatrolInfo> Prepare()
		{
			return null;
		}
	}
}
