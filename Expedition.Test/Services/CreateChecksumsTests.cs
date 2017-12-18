using System;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Expedition.Core;
using Expedition.Core.Services;

namespace Expedition.Test.Services
{
	[TestClass]
	public class CreateChecksumsTests
	{
		public CreateChecksums _service;
		public string _dataUri;

		[TestInitialize]
		public void TestInitialize()
		{
			_dataUri = TestsHelper.GetTestDataDirectory();
			_service = new CreateChecksums();
		}

		[TestCleanup]
		public void TestCleanup()
		{
			var files = Directory.GetFiles(_dataUri);
			foreach(var file in files)
			{
				if (file.EndsWith(".md5") || file.EndsWith(".sha1")) File.Delete(file);
			}
		}

		private CreateChecksumsRequest GetDefaultRequest()
		{
			return new CreateChecksumsRequest()
			{
				DirectoryUri = _dataUri,
			};
		}

		[TestMethod]
		public void TestDefaultRequest_ShouldExcludeChecksumFile()
		{
			// prepare
			var request = GetDefaultRequest();
			request.Recursive = false;

			// execute
			var response = _service.Execute(request);

			// assert
			Assert.AreEqual(1, response.Files.Length);
			Assert.AreEqual("File 0.txt", response.Files[0].Name);
		}

		[TestMethod]
		public void TestDefaultRequest_ShouldIncludeComments()
		{
			// prepare
			var request = GetDefaultRequest();
			request.Recursive = false;

			// execute
			var response = _service.Execute(request);

			// assert
			var lines = File.ReadAllLines(response.OutputFileUri);
			Assert.IsTrue(lines[0].StartsWith("# Generated Md5 with Expedition at"));
			Assert.IsTrue(lines[1].StartsWith("# https://github.com/DesignedSimplicity/Expedition/"));
		}

		[TestMethod]
		public void TestRecursiveRequest_ShouldHaveFourFiles()
		{
			// prepare
			var request = GetDefaultRequest();
			request.Recursive = true;

			// execute
			var response = _service.Execute(request);

			// assert
			Assert.AreEqual(4, response.Files.Length);
			Assert.IsTrue(response.Files.Any(x => x.Name == "File 0.txt"));
			Assert.IsTrue(response.Files.Any(x => x.Name == "File 1.txt"));
			Assert.IsTrue(response.Files.Any(x => x.Name == "File A.nfo"));
			Assert.IsTrue(response.Files.Any(x => x.Name == "File B.csv"));
		}

		[TestMethod]
		public void TestMd5HashAlgorithm_ShouldMatchMd5Hash()
		{
			// prepare
			var request = GetDefaultRequest();
			request.Recursive = false;

			// execute
			var response = _service.Execute(request);

			// assert
			var lines = File.ReadAllLines(response.OutputFileUri);
			Assert.IsTrue(lines.Any(x => x.StartsWith("34A753225D1F963F973B2D06260CD47D")));
		}

		[TestMethod]
		public void TestSha1HashAlgorithm_ShouldMatchSha1Hash()
		{
			// prepare
			var request = GetDefaultRequest();
			request.Recursive = false;
			request.HashType = HashType.Sha1;

			// execute
			var response = _service.Execute(request);

			// assert
			var lines = File.ReadAllLines(response.OutputFileUri);
			Assert.IsTrue(lines.Any(x => x.StartsWith("D7B60397491D21BB21191E5A4E9915DAA8F0AC80")));
		}

		[TestMethod]
		public void TestRelativeToUri_ShouldHaveAbsolutePath()
		{
			// prepare
			var request = GetDefaultRequest();
			request.Recursive = true;

			// execute
			var response = _service.Execute(request);

			// assert
			var lines = File.ReadAllLines(response.OutputFileUri);
			var first = lines.Where(x => x.EndsWith(@"File B.csv", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
			var path = first.Substring(33).Trim();
			var expected = Path.Combine(TestsHelper.GetTestDataDirectory(), @"Root 2\Folder B\Subfolder B\File B.csv");
			Assert.AreEqual(expected, path);
		}

		[TestMethod]
		public void TestRelativeToUri_ShouldHaveRelativePath()
		{
			// prepare
			var request = GetDefaultRequest();
			request.Recursive = true;
			request.RelativeToUri = request.DirectoryUri;

			// execute
			var response = _service.Execute(request);

			// assert
			var lines = File.ReadAllLines(response.OutputFileUri);
			var first = lines.Where(x => x.EndsWith(@"File B.csv", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
			var path = first.Substring(33).Trim();
			var expected = @"Root 2\Folder B\Subfolder B\File B.csv";
			Assert.AreEqual(expected, path);
		}
	}
}
