using FakeItEasy;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace Prepend.Tests {
    public class PrependLogicTests {

        [SetUp]
        public void Setup() {
        }

        [Test]
        public void AddPrependTextTest() {

            // ARRANGE
            var fakeFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                { @"T:\TestFiles\file-01.txt", new MockFileData("file-01-Contents") },
                { @"T:\TestFiles\file-02.doc", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
                { @"T:\TestFiles\file-03.txt", new MockFileData("file-03-Contents") },
                { @"T:\TestFiles\file-04.junk", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
                { @"T:\TestFiles\file-05", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            });

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
            var fakeFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                { @"T:\TestFiles\aaa100 - file-01.txt", new MockFileData(@"file-01-Contents") },
                { @"T:\TestFiles\file-02.doc", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
                { @"T:\TestFiles\aaa101 - file-03.txt", new MockFileData(@"file-03-Contents") },
                { @"T:\TestFiles\file-04.junk", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
                { @"T:\TestFiles\file-05", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            });

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