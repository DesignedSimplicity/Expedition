using System;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Expedition.Core;
using Expedition.Core.Services;

namespace Expedition.Test.Services
{
	[TestClass]
	public class VerifyChecksumsTests
	{
		public VerifyChecksums _service;
		public string _dataUri;

		[TestInitialize]
		public void TestInitialize()
		{
			_dataUri = TestsHelper.GetTestDataDirectory();
			_service = new VerifyChecksums();
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

		private VerifyChecksumsRequest GetDefaultRequest(string fileUri)
		{
			return new VerifyChecksumsRequest()
			{
				FileUri = fileUri
			};
		}

		private string CreateChecksumFile(HashType hashType, bool relative, bool recursive)
		{
			var create = new CreateChecksums();
			var request = new CreateChecksumsRequest()
			{
				RelativeToUri = (relative ? _dataUri : String.Empty),
				DirectoryUri = _dataUri,
				Recursive = recursive,
				HashType = hashType,
			};
			var response = create.Execute(request);
			return response.OutputFileUri;
		}

		[TestMethod]
		public void TestAbsoluteMd5_ShouldVerifyHash()
		{
			// prepare
			var checksum = CreateChecksumFile(HashType.Md5, true, false);
			var request = GetDefaultRequest(checksum);

			// execute
			var response = _service.Execute(request);

			// assert
			Assert.IsFalse(response.HasErrors);
			Assert.AreEqual(1, response.Files.Length);
			Assert.AreEqual("File 0.txt", response.Files[0].Name);
		}

		[TestMethod]
		public void TestRelativeMd5_ShouldVerifyHash()
		{
			// prepare
			var checksum = CreateChecksumFile(HashType.Md5, false, false);
			var request = GetDefaultRequest(checksum);

			// execute
			var response = _service.Execute(request);

			// assert
			Assert.IsFalse(response.HasErrors);
			Assert.AreEqual(1, response.Files.Length);
			Assert.AreEqual("File 0.txt", response.Files[0].Name);
		}

		[TestMethod]
		public void TestAbsoluteSha1_ShouldVerifyHash()
		{
			// prepare
			var checksum = CreateChecksumFile(HashType.Sha1, true, false);
			var request = GetDefaultRequest(checksum);

			// execute
			var response = _service.Execute(request);

			// assert
			Assert.IsFalse(response.HasErrors);
			Assert.AreEqual(1, response.Files.Length);
			Assert.AreEqual("File 0.txt", response.Files[0].Name);
		}

		[TestMethod]
		public void TestRelativeSha1_ShouldVerifyHash()
		{
			// prepare
			var checksum = CreateChecksumFile(HashType.Sha1, false, false);
			var request = GetDefaultRequest(checksum);

			// execute
			var response = _service.Execute(request);

			// assert
			Assert.IsFalse(response.HasErrors);
			Assert.AreEqual(1, response.Files.Length);
			Assert.AreEqual("File 0.txt", response.Files[0].Name);
		}

		[TestMethod]
		public void TestMissingFile_ShouldHaveError()
		{
			// prepare
			var checksum = Path.Combine(_dataUri, "BAD.md5");
			using (var stream = File.CreateText(checksum))
			{
				stream.WriteLine("00A753225D1F963F973B2D06260CD47D File X.txt");
			}
			var request = GetDefaultRequest(checksum);

			// execute
			var response = _service.Execute(request);

			// assert
			var key = Path.Combine(_dataUri, "File X.txt");
			Assert.IsTrue(response.HasErrors);
			Assert.IsTrue(response.Errors.ContainsKey(key));
			Assert.AreEqual($"Could not find file '{key}'.", response.Errors[key].Message);
		}

		[TestMethod]
		public void TestInvalidMd5_ShouldHaveError()
		{
			// prepare
			var checksum = Path.Combine(_dataUri, "BAD.md5");
			using (var stream = File.CreateText(checksum))
			{
				stream.WriteLine("00A753225D1F963F973B2D06260CD47D File 0.txt");
			}
			var request = GetDefaultRequest(checksum);

			// execute
			var response = _service.Execute(request);

			// assert
			var key = Path.Combine(_dataUri, "File 0.txt");
			Assert.IsTrue(response.HasErrors);
			Assert.IsTrue(response.Errors.ContainsKey(key));
			Assert.AreEqual("Hash Invalid", response.Errors[key].Message);
		}

		[TestMethod]
		public void TestInvalidSha1_ShouldHaveError()
		{
			// prepare
			var checksum = Path.Combine(_dataUri, "BAD.sha1");
			using (var stream = File.CreateText(checksum))
			{
				stream.WriteLine("00B60397491D21BB21191E5A4E9915DAA8F0AC80 File 0.txt");
			}
			var request = GetDefaultRequest(checksum);

			// execute
			var response = _service.Execute(request);

			// assert
			var key = Path.Combine(_dataUri, "File 0.txt");
			Assert.IsTrue(response.HasErrors);
			Assert.IsTrue(response.Errors.ContainsKey(key));
			Assert.AreEqual("Hash Invalid", response.Errors[key].Message);
		}
	}
}
