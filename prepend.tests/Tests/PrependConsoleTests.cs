using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Prepend.Interfaces;

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
                    .AddSingleton<IConsole>(fakeConsole);

            var app = new PrependConsole(serviceCollection);

            // ACT
            app.Run();

            // ASSERT
            Assert.That(fakeConsole.Output.Count > 0 && fakeConsole.Output[0].StartsWith("Usage"));
        }
    }
}
