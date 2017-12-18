using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Expedition.Core;
using Expedition.Core.Services;

namespace Expedition.Test.Services
{
	[TestClass]
	public class QueryFileSystemTests
	{
		public QueryFileSystem _service;
		public string _dataUri;

		[TestInitialize]
		public void TestInitialize()
		{
			_dataUri = TestsHelper.GetTestDataDirectory();
			_service = new QueryFileSystem();
		}

		private QueryFileSystemRequest GetDefaultRequest()
		{
			return new QueryFileSystemRequest()
			{
				DirectoryUri = _dataUri
			};
		}

		[TestMethod]
		public void TestEmptyDirectory_ShouldThrowException()
		{
			// prepare
			var request = GetDefaultRequest();
			request.DirectoryUri = null;

			// execute
			Exception exception = null;
			try
			{
				var response = _service.Execute(request);
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			// assert
			Assert.IsTrue(exception.Message.EndsWith("not provided"));
		}

		[TestMethod]
		public void TestInvalidDirectory_ShouldThrowException()
		{
			// prepare
			var request = GetDefaultRequest();
			request.DirectoryUri = @"Z:\{INVALID}";

			// execute
			Exception exception = null;
			try
			{
				var response = _service.Execute(request);
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			// assert
			Assert.IsTrue(exception.Message.EndsWith("does not exist or is not accessible"));
		}

		[TestMethod]
		public void TestNonRecursiveQuery_ShouldHaveOneFile()
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
		public void TestRecursiveQuery_ShouldHaveFourFiles()
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
		public void TestFilteredQuery_ShouldHaveTwoFiles()
		{
			// prepare
			var request = GetDefaultRequest();
			request.FilePattern = "*.txt";
			request.Recursive = true;

			// execute
			var response = _service.Execute(request);

			// assert
			Assert.AreEqual(2, response.Files.Length);
			Assert.IsTrue(response.Files.Any(x => x.Name == "File 0.txt"));
			Assert.IsTrue(response.Files.Any(x => x.Name == "File 1.txt"));
		}

		[TestMethod]
		public void TestNonRecursiveQueryVerbose_ShouldHaveOneFile()
		{
			// prepare
			var request = GetDefaultRequest();
			request.Recursive = false;
			request.Verbose = true;

			// execute
			var response = _service.Execute(request);

			// assert
			Assert.AreEqual(1, response.Files.Length);
			Assert.AreEqual("File 0.txt", response.Files[0].Name);
		}

		[TestMethod]
		public void TestRecursiveQueryVerbose_ShouldHaveFourFiles()
		{
			// prepare
			var request = GetDefaultRequest();
			request.Recursive = true;
			request.Verbose = true;

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
		public void TestFilteredQueryVerbose_ShouldHaveTwoFiles()
		{
			// prepare
			var request = GetDefaultRequest();
			request.FilePattern = "*.txt";
			request.Recursive = true;
			request.Verbose = true;

			// execute
			var response = _service.Execute(request);

			// assert
			Assert.AreEqual(2, response.Files.Length);
			Assert.IsTrue(response.Files.Any(x => x.Name == "File 0.txt"));
			Assert.IsTrue(response.Files.Any(x => x.Name == "File 1.txt"));
		}
	}
}
