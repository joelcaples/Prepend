using Prepend.Interfaces;
using System;
using System.Collections.Generic;

namespace Prepend.Tests {
    class FakeConsole : IConsole {
        public void WriteLine(string message) {
            Output.Add(message);
        }

        public ConsoleKeyInfo ReadKey() {
            return new ConsoleKeyInfo();
        }

        public List<string> Output { get; } = new List<string>();
    }
}
