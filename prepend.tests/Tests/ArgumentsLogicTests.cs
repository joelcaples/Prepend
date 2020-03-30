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
    }
}
