using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithogen.Core.Tests.Unit
{
    public class PathUtilsTests
    {
        public const string ParentDirectory = @"C:\temp\Lithogen_testdir";
        public const string NonParentDirectory = @"C:\somewhere\else";
        public const string SubDirectory = @"C:\temp\Lithogen_testdir\dir1";
        

        [Test]
        public void GetSubPath_WhenArgumentsAreNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => PathUtils.GetSubPath(null, "child"));
            Assert.Throws<ArgumentNullException>(() => PathUtils.GetSubPath("parent", null));
        }

        [Test]
        public void GetSubPath_WhenArgumentsAreWhitespace_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => PathUtils.GetSubPath(" ", "child"));
            Assert.Throws<ArgumentException>(() => PathUtils.GetSubPath("parent", " "));
        }

        [Test]
        public void GetSubPath_WhenChildIsNotSubPath_ReturnsChild()
        {
            Assert.AreEqual(SubDirectory, PathUtils.GetSubPath(NonParentDirectory, SubDirectory));
        }

        [Test]
        public void GetSubPath_WhenChildIsSubPath_ReturnsChildComponent()
        {
            string result = PathUtils.GetSubPath(ParentDirectory, SubDirectory);
            Assert.AreEqual("dir1", result);
        }

        [Test]
        public void GetSubPath_WhenChildIsSubPathInDifferentCase_ReturnsChildComponent()
        {
            string result = PathUtils.GetSubPath(ParentDirectory.ToUpperInvariant(), SubDirectory);
            Assert.AreEqual("dir1", result);
        }
    }
}
