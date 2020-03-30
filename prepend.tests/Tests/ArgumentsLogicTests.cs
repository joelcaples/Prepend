using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prepend.Tests {
    public class ArgumentsLogicTests {

        [SetUp]
        public void Setup() {
        }

        [Test]
        public void GetFolderPath() {

            // ARRANGE
            var args = new string[] { @"--folder-path=T:\TestData\*.txt" };
            var logic = new ArgumentsLogic(args);

            // ACT
            var value = logic.GetFolderPath();

            // ASSERT
            Assert.AreEqual(@"T:\TestData\*.txt", value);
        }

        [Test]
        public void GetPrependText() {

            // ARRANGE
            var args = new string[] { @"--prepend-text=aaa### - " };
            var logic = new ArgumentsLogic(args);

            // ACT
            var value = logic.GetPrependText();

            // ASSERT
            Assert.AreEqual(@"aaa### - ", value);
        }

        [Test]
        public void GetFileNumberSeed() {

            // ARRANGE
            var args = new string[] { @"--file-number-seed=666" };
            var logic = new ArgumentsLogic(args);

            // ACT
            var value = logic.GetFileNumberSeed();

            // ASSERT
            Assert.AreEqual(666, value);
        }

        [Test]
        public void MissingFolderPathThrow() {

            // ARRANGE
            var args = new string[] { };
            var logic = new ArgumentsLogic(args);

            // ASSERT
            Assert.Throws<ArgumentException>(
                delegate { logic.GetFolderPath(); });
        }

        [Test]
        public void MissingPrependTextThrow() {

            // ARRANGE
            var args = new string[] { };
            var logic = new ArgumentsLogic(args);

            // ASSERT
            Assert.Throws<ArgumentException>(
                delegate { logic.GetPrependText(); });
        }

        [Test]
        public void MissingFileNumberReturnsDefault() {

            // ARRANGE
            var args = new string[] { };
            var logic = new ArgumentsLogic(args);

            // ACT
            var result = logic.GetFileNumberSeed();

            // ASSERT
            Assert.AreEqual(1, result);
        }

        [Test]
        public void InvalidFileNumberThrow() {
            // ARRANGE
            var args = new string[] { @"--file-number-seed=XXX" };
            var logic = new ArgumentsLogic(args);

            // ASSERT
            Assert.Throws<ArgumentException>(
                delegate { logic.GetFileNumberSeed(); });
        }

        [Test]
        public void GetCommandHelp() {

            // ARRANGE
            var args = new string[] { @"--help" };
            var logic = new ArgumentsLogic(args);

            // ACT
            var value = logic.Command;

            // ASSERT
            Assert.AreEqual(CommandType.Usage, value);
        }

        [Test]
        public void GetCommandRemove() {

            // ARRANGE
            var args = new string[] { @"--remove" };
            var logic = new ArgumentsLogic(args);

            // ACT
            var value = logic.Command;

            // ASSERT
            Assert.AreEqual(CommandType.Remove, value);
        }

        [Test]
        public void GetCommandPrepend() {

            // ARRANGE
            var args = new string[] { @"--prepend" };
            var logic = new ArgumentsLogic(args);

            // ACT
            var value = logic.Command;

            // ASSERT
            Assert.AreEqual(CommandType.Prepend, value);
        }
    }
}
