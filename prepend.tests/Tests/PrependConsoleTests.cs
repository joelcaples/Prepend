using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Prepend.Interfaces;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace Prepend.Tests {
    class PrependConsoleTests {

        [SetUp]
        public void Setup() {
        }

        [Test]
        public void UsageTest() {

            // ARRANGE
            var args = new string[] { @"--help" };
            var fakeConsole = new FakeConsole();
            var serviceCollection = new ServiceCollection()
                    .AddSingleton<IArgumentsLogic>(new ArgumentsLogic(args))
                    .AddSingleton<IConsole>(fakeConsole)
                    .AddSingleton<IFileSystem>(A.Fake<IFileSystem>());

            var app = new PrependConsole(serviceCollection);

            // ACT
            app.Run();

            // ASSERT
            Assert.That(fakeConsole.Output.Count > 0 && fakeConsole.Output[0].StartsWith("Usage"));
        }

        [Test]
        public void PrependTest() {

            var _messages = new List<string>();
            void CaptureConsoleOutput(string msg) {
                _messages.Add(msg);
            }

            // ARRANGE
            var args = new string[] { 
                @"--folder-path=T:\TestFiles\*.txt", 
                @"--prepend-text=aaa### - ",
                @"--file-number-seed=100"
            };

            //var fakeConsole = new FakeConsole();

            var fakeConsole = A.Fake<IConsole>();
            A.CallTo(() => fakeConsole.WriteLine(A<string>._)).Invokes((string msg) => CaptureConsoleOutput(msg));
            A.CallTo(() => fakeConsole.ReadKey()).Returns(new System.ConsoleKeyInfo('y', System.ConsoleKey.Y, false, false, false));

            var fakeFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                { @"T:\TestFiles\file-01.txt", new MockFileData("file-01-Contents") },
                { @"T:\TestFiles\file-02.doc", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
                { @"T:\TestFiles\file-03.txt", new MockFileData("file-03-Contents") },
                { @"T:\TestFiles\file-04.junk", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
                { @"T:\TestFiles\file-05", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            });
            var serviceCollection = new ServiceCollection()
                    .AddSingleton<IArgumentsLogic>(new ArgumentsLogic(args))
                    .AddSingleton<IConsole>(fakeConsole)
                    .AddSingleton<IFileSystem>(fakeFileSystem);

            var app = new PrependConsole(serviceCollection);

            // ACT
            app.Run();

            // ASSERT
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\aaa100 - file-01.txt"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-02.doc"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\aaa101 - file-03.txt"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-04.junk"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-05"));
        }

        [Test]
        public void RemoveTest() {

            var _messages = new List<string>();
            void CaptureConsoleOutput(string msg) {
                _messages.Add(msg);
            }

            // ARRANGE
            var args = new string[] {
                @"--folder-path=T:\TestFiles\*.txt",
                @"--prepend-text=aaa### - ",
                @"--file-number-seed=100",
                @"--remove"
            };

            //var fakeConsole = new FakeConsole();

            var fakeConsole = A.Fake<IConsole>();
            A.CallTo(() => fakeConsole.WriteLine(A<string>._)).Invokes((string msg) => CaptureConsoleOutput(msg));
            A.CallTo(() => fakeConsole.ReadKey()).Returns(new System.ConsoleKeyInfo('y', System.ConsoleKey.Y, false, false, false));

            var fakeFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                { @"T:\TestFiles\aaa100 - file-01.txt", new MockFileData(@"file-01-Contents") },
                { @"T:\TestFiles\file-02.doc", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
                { @"T:\TestFiles\aaa101 - file-03.txt", new MockFileData(@"file-03-Contents") },
                { @"T:\TestFiles\file-04.junk", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
                { @"T:\TestFiles\file-05", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            });

            var serviceCollection = new ServiceCollection()
                    .AddSingleton<IArgumentsLogic>(new ArgumentsLogic(args))
                    .AddSingleton<IConsole>(fakeConsole)
                    .AddSingleton<IFileSystem>(fakeFileSystem);

            var app = new PrependConsole(serviceCollection);

            // ACT
            app.Run();

            // ASSERT
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-01.txt"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-02.doc"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-03.txt"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-04.junk"));
            Assert.IsTrue(fakeFileSystem.FileExists(@"T:\TestFiles\file-05"));
        }

    }
}
