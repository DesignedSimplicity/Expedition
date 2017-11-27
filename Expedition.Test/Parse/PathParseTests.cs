using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Expedition.Core;

namespace Expedition.Test.Parse
{
	[TestClass]
	public class PathParseTests
	{
		[TestMethod]
		public void TestGetPathsEmpty()
		{
			var uri = @"";
			var paths = ParsePath.GetPaths(uri);
			Assert.AreEqual(0, paths.Length);
		}

		[TestMethod]
		public void TestGetPathsAtRoot()
		{
			var uri = @"C:";
			var paths = ParsePath.GetPaths(uri);
			Assert.AreEqual("C:", paths[0]);
		}
		
		[TestMethod]
		public void TestGetPathsAtRootWithSlash()
		{
			var uri = @"C:\";
			var paths = ParsePath.GetPaths(uri);
			Assert.AreEqual("C:", paths[0]);
		}

		[TestMethod]
		public void TestGetPathsAtRootWithExtraSlashs()
		{
			var uri = @"C:\\";
			var paths = ParsePath.GetPaths(uri);
			Assert.AreEqual("C:", paths[0]);
		}

		[TestMethod]
		public void TestGetPathsForDirectory()
		{
			var uri = @"C:\RootDir";
			var paths = ParsePath.GetPaths(uri);
			Assert.AreEqual("C:", paths[0]);
			Assert.AreEqual("RootDir", paths[1]);
		}

		[TestMethod]
		public void TestGetPathsForDirectoryWithSlash()
		{
			var uri = @"C:\RootDir\";
			var paths = ParsePath.GetPaths(uri);
			Assert.AreEqual("C:", paths[0]);
			Assert.AreEqual("RootDir", paths[1]);
		}

		[TestMethod]
		public void TestGetPathsForDirectoryWithDoubleSlashes()
		{
			var uri = @"C:\\RootDir\\";
			var paths = ParsePath.GetPaths(uri);
			Assert.AreEqual("C:", paths[0]);
			Assert.AreEqual("RootDir", paths[1]);
		}

		[TestMethod]
		public void TestGetPathsForSubDirectory()
		{
			var uri = @"C:\\RootDir\\SubDir";
			var paths = ParsePath.GetPaths(uri);
			Assert.AreEqual("C:", paths[0]);
			Assert.AreEqual("RootDir", paths[1]);
			Assert.AreEqual("SubDir", paths[2]);
		}

		[TestMethod]
		public void TestGetPathsForFileInSubDirectory()
		{
			var uri = @"C:\RootDir\SubDir\\FileName.ext";
			var paths = ParsePath.GetPaths(uri);
			Assert.AreEqual("C:", paths[0]);
			Assert.AreEqual("RootDir", paths[1]);
			Assert.AreEqual("SubDir", paths[2]);
			Assert.AreEqual("FileName.ext", paths[3]);
		}
	}
}
