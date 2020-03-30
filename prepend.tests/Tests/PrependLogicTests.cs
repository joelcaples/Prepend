using NUnit.Framework;
using Prepend.Lib;
using Prepend.Tests.TestData;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;

namespace Prepend.Tests {

    public class PrependLogicTests {

        [SetUp]
        public void Setup() {
        }

        [Test]
        public void AddPrependTextTest() {

            // ARRANGE
            var fakeFileSystem = new Fakes().BuildFakeFileSystemWithoutPrepend();
            var logic = new PrependLogic(fakeFileSystem);

            // ACT
            logic.AddPrependText(@"T:\TestFiles\*.txt", "aaa### - ", 100, ConfirmActionTrue);

            // ASSERT
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\aaa100 - file-01.txt"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-02.doc"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\aaa101 - file-03.txt"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-04.junk"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-05"));
        }

        [Test]
        public void RemovePrependTextTest() {

            // ARRANGE
            var fakeFileSystem = new Fakes().BuildFakeFileSystemWithPrepend();
            var logic = new PrependLogic(fakeFileSystem);

            // ACT
            logic.RemovePrependedText(@"T:\TestFiles\*.txt", "aaa### - ", ConfirmActionTrue);

            // ASSERT
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-01.txt"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-02.doc"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-03.txt"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-04.junk"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-05"));
        }

        public bool ConfirmActionTrue(string file, string newFileName) {
            return true;
        }
    }
}