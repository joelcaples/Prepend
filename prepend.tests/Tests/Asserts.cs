using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Text;

namespace Prepend.Tests {
    class Asserts {
        public void AssertFileSystemWithoutPrepend(MockFileSystem fakeFileSystem) {
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-01.txt"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-02.doc"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-03.txt"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-04.junk"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-05"));
        }

        public void AssertFileSystemWithPrepend(MockFileSystem fakeFileSystem) {

            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\aaa100 - file-01.txt"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-02.doc"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\aaa101 - file-03.txt"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-04.junk"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-05"));
        }
    }
}