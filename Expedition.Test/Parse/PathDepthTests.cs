using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Expedition.Core;

namespace Expedition.Test.Parse
{
	[TestClass]
	public class PathDepthTests
	{
		[TestMethod]
		public void TestGetDepthInvalid()
		{
			var uri = @"";
			var depth = ParsePath.GetDepth(uri);
			Assert.AreEqual(-1, depth);
		}

		[TestMethod]
		public void TestGetDepthAtRoot()
		{
			var uri = @"C:";
			var depth = ParsePath.GetDepth(uri);
			Assert.AreEqual(0, depth);
		}

		[TestMethod]
		public void TestGetDepthAtRootWithSlash()
		{
			var uri = @"C:\";
			var depth = ParsePath.GetDepth(uri);
			Assert.AreEqual(0, depth);
		}

		[TestMethod]
		public void TestGetDepthAtRootWithExtraSlashs()
		{
			var uri = @"C:\\";
			var depth = ParsePath.GetDepth(uri);
			Assert.AreEqual(0, depth);
		}

		[TestMethod]
		public void TestGetDepthForDirectory()
		{
			var uri = @"C:\RootDir";
			var depth = ParsePath.GetDepth(uri);
			Assert.AreEqual(1, depth);
		}

		[TestMethod]
		public void TestGetDepthForDirectoryWithSlash()
		{
			var uri = @"C:\RootDir\";
			var depth = ParsePath.GetDepth(uri);
			Assert.AreEqual(1, depth);
		}

		[TestMethod]
		public void TestGetDepthForDirectoryWithDoubleSlashes()
		{
			var uri = @"C:\\RootDir\\";
			var depth = ParsePath.GetDepth(uri);
			Assert.AreEqual(1, depth);
		}

		[TestMethod]
		public void TestGetDepthForSubDirectory()
		{
			var uri = @"C:\\RootDir\\SubDir";
			var depth = ParsePath.GetDepth(uri);
			Assert.AreEqual(2, depth);
		}

		[TestMethod]
		public void TestGetDepthForFileInSubDirectory()
		{
			var uri = @"C:\RootDir\SubDir\\FileName.ext";
			var depth = ParsePath.GetDepth(uri);
			Assert.AreEqual(3, depth);
		}
	}
}
