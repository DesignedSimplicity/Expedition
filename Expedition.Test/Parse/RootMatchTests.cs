using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Expedition.Core;

namespace Expedition.Test.Parse
{
	[TestClass]
	public class RootMatchTests
	{
		[TestMethod]
		public void TestHasSameRootAtRoot()
		{
			var uri1 = @"C:";
			var uri2 = @"C:";
			var same = ParsePath.HasSameRoot(uri1, uri2);
			Assert.IsTrue(same);
		}

		[TestMethod]
		public void TestHasSameRootAtRootWithSlash1()
		{
			var uri1 = @"C:\";
			var uri2 = @"C:";
			var same = ParsePath.HasSameRoot(uri1, uri2);
			Assert.IsTrue(same);
		}

		[TestMethod]
		public void TestHasSameRootAtRootWithSlash2()
		{
			var uri1 = @"C:";
			var uri2 = @"C:\";
			var same = ParsePath.HasSameRoot(uri1, uri2);
			Assert.IsTrue(same);
		}

		[TestMethod]
		public void TestHasSameRootAtRootWithSlash1And2()
		{
			var uri1 = @"C:\";
			var uri2 = @"C:\";
			var same = ParsePath.HasSameRoot(uri1, uri2);
			Assert.IsTrue(same);
		}

		[TestMethod]
		public void TestHasSameRootAtRootWithDirectory1()
		{
			var uri1 = @"C:\Directory1";
			var uri2 = @"C:\";
			var same = ParsePath.HasSameRoot(uri1, uri2);
			Assert.IsTrue(same);
		}

		[TestMethod]
		public void TestHasSameRootAtRootWithDirectory2()
		{
			var uri1 = @"C:\";
			var uri2 = @"C:\Directory2";
			var same = ParsePath.HasSameRoot(uri1, uri2);
			Assert.IsTrue(same);
		}

		[TestMethod]
		public void TestHasSameRootAtRootWithDirectory1And2()
		{
			var uri1 = @"C:\Directory1";
			var uri2 = @"C:\Directory2";
			var same = ParsePath.HasSameRoot(uri1, uri2);
			Assert.IsTrue(same);
		}

		[TestMethod]
		public void TestHasSameRootAtRootWithSubDirectories()
		{
			var uri1 = @"C:\Directory1\Subdirectory";
			var uri2 = @"C:\Directory2\Subdirectory";
			var same = ParsePath.HasSameRoot(uri1, uri2);
			Assert.IsTrue(same);
		}

		[TestMethod]
		public void TestHasSameRootNotSameRootAtRoot()
		{
			var uri1 = @"C:\";
			var uri2 = @"D:\Directory2\Subdirectory";
			var same = ParsePath.HasSameRoot(uri1, uri2);
			Assert.IsFalse(same);
		}

		[TestMethod]
		public void TestHasSameRootNotSameRoot()
		{
			var uri1 = @"C:\Directory1\Subdirectory";
			var uri2 = @"D:\Directory2\Subdirectory";
			var same = ParsePath.HasSameRoot(uri1, uri2);
			Assert.IsFalse(same);
		}
	}
}
