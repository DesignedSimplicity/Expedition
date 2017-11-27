using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Expedition.Core;

namespace Expedition.Test.Parse
{
	[TestClass]
	public class RootParseTests
	{
		[TestMethod]
		public void TestParseCommonPathRootInvalid()
		{
			var uri1 = @"C:";
			var uri2 = @"D:";
			var path = ParsePath.GetCommonPath(uri1, uri2);
			Assert.AreEqual(@"", path);
		}

		[TestMethod]
		public void TestParseCommonPathRootInvalidWithDirectory()
		{
			var uri1 = @"C:\Same";
			var uri2 = @"D:\Same";
			var path = ParsePath.GetCommonPath(uri1, uri2);
			Assert.AreEqual(@"", path);
		}

		[TestMethod]
		public void TestParseCommonPathRoot()
		{
			var uri1 = @"C:";
			var uri2 = @"C:";
			var path = ParsePath.GetCommonPath(uri1, uri2);
			Assert.AreEqual(@"C:", path);
		}

		[TestMethod]
		public void TestParseCommonPathRootWithDirectory1()
		{
			var uri1 = @"C:\Directory1";
			var uri2 = @"C:";
			var path = ParsePath.GetCommonPath(uri1, uri2);
			Assert.AreEqual(@"C:", path);
		}

		[TestMethod]
		public void TestParseCommonPathRootWithDirectory2()
		{
			var uri1 = @"C:\";
			var uri2 = @"C:\Directory2";
			var path = ParsePath.GetCommonPath(uri1, uri2);
			Assert.AreEqual(@"C:", path);
		}

		[TestMethod]
		public void TestParseCommonPathRootWithDirectory1And2()
		{
			var uri1 = @"C:\Directory1";
			var uri2 = @"C:\Directory2\\";
			var path = ParsePath.GetCommonPath(uri1, uri2);
			Assert.AreEqual(@"C:", path);
		}

		[TestMethod]
		public void TestParseCommonPathRootWithCommonDirectory()
		{
			var uri1 = @"C:\Directory\Sub1";
			var uri2 = @"C:\Directory\Sub2\";
			var path = ParsePath.GetCommonPath(uri1, uri2);
			Assert.AreEqual(@"C:\Directory", path);
		}

		[TestMethod]
		public void TestParseCommonPathRootWithCommonSubDirectory()
		{
			var uri1 = @"C:\Directory\Sub\Child1";
			var uri2 = @"C:\Directory\Sub\\Child2";
			var path = ParsePath.GetCommonPath(uri1, uri2);
			Assert.AreEqual(@"C:\Directory\Sub", path);
		}

		[TestMethod]
		public void TestParseCommonPathRootWithCommonRootDirectory()
		{
			var uri1 = @"C:\Directory\Sub1\Child1";
			var uri2 = @"C:\Directory\Sub2\Child2\";
			var path = ParsePath.GetCommonPath(uri1, uri2);
			Assert.AreEqual(@"C:\Directory", path);
		}

		[TestMethod]
		public void TestParseCommonPathRootWithCommonRootDirectoryDifferentLevels()
		{
			var uri1 = @"C:\Directory\Sub1\Child1\";
			var uri2 = @"C:\Directory\Sub2\Child2\Grandchild2";
			var path = ParsePath.GetCommonPath(uri1, uri2);
			Assert.AreEqual(@"C:\Directory", path);
		}
	}
}
