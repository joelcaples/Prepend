using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;

namespace Prepend.Tests.TestData {
    public class Fakes {

        public MockFileSystem BuildFakeFileSystemWithoutPrepend() {

            var fakeFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                { @"T:\TestFiles\file-01.txt", new MockFileData("file-01-Contents") },
                { @"T:\TestFiles\file-02.doc", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
                { @"T:\TestFiles\file-03.txt", new MockFileData("file-03-Contents") },
                { @"T:\TestFiles\file-04.junk", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
                { @"T:\TestFiles\file-05", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            });

            return fakeFileSystem;
        }

        public MockFileSystem BuildFakeFileSystemWithPrepend() {
            var fakeFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                { @"T:\TestFiles\aaa100 - file-01.txt", new MockFileData(@"file-01-Contents") },
                { @"T:\TestFiles\file-02.doc", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
                { @"T:\TestFiles\aaa101 - file-03.txt", new MockFileData(@"file-03-Contents") },
                { @"T:\TestFiles\file-04.junk", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
                { @"T:\TestFiles\file-05", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            });

            return fakeFileSystem;
        }

    }
}
