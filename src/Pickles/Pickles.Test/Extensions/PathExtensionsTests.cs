using System;
using NUnit.Framework;
using PicklesDoc.Pickles.Extensions;

namespace PicklesDoc.Pickles.Test.Extensions
{
    [TestFixture]
    internal class PathExtensionsTests : BaseFixture
    {
        [Test]
        public void Get_A_Relative_Path_When_Location_Is_Deeper_Than_Root()
        {
            // Arrange
            string root = @"c:\test";
            string location = @"c:\test\deep\blah.feature";
            string expected = @"deep\blah.feature";

            // Act
            string actual = PathExtensions.MakeRelativePath(root, location, FileSystem);

            // Assert
            Assert.AreEqual(actual, expected, string.Format("Expected {0} got {1}", expected, actual));
        }

        [Test]
        public void Get_A_Relative_Path_When_Location_Is_Deeper_Than_Root_Even_When_Root_Contains_End_Slash()
        {
            string root = @"c:\test\";
            string location = @"c:\test\deep\blah.feature";
            string expected = @"deep\blah.feature";

            string actual = PathExtensions.MakeRelativePath(root, location, FileSystem);

            Assert.AreEqual(actual, expected, string.Format("Expected {0} got {1}", expected, actual));
        }
    }
}