﻿using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Prepend.Interfaces;
using Prepend.Tests.TestData;
using System.Collections.Generic;
using System.IO.Abstractions;

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

            var fakeFileSystem = new Fakes().BuildFakeFileSystemWithoutPrepend();
            var serviceCollection = new ServiceCollection()
                    .AddSingleton<IArgumentsLogic>(new ArgumentsLogic(args))
                    .AddSingleton<IConsole>(fakeConsole)
                    .AddSingleton<IFileSystem>(fakeFileSystem);

            var app = new PrependConsole(serviceCollection);

            // ACT
            app.Run();

            // ASSERT
            new Asserts().AssertFileSystemWithPrepend(fakeFileSystem);
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

            var fakeConsole = A.Fake<IConsole>();
            A.CallTo(() => fakeConsole.WriteLine(A<string>._)).Invokes((string msg) => CaptureConsoleOutput(msg));
            A.CallTo(() => fakeConsole.ReadKey()).Returns(new System.ConsoleKeyInfo('y', System.ConsoleKey.Y, false, false, false));

            var fakeFileSystem = new Fakes().BuildFakeFileSystemWithPrepend();

            var serviceCollection = new ServiceCollection()
                    .AddSingleton<IArgumentsLogic>(new ArgumentsLogic(args))
                    .AddSingleton<IConsole>(fakeConsole)
                    .AddSingleton<IFileSystem>(fakeFileSystem);

            var app = new PrependConsole(serviceCollection);

            // ACT
            app.Run();

            // ASSERT
            new Asserts().AssertFileSystemWithoutPrepend(fakeFileSystem);
        }

    }
}
