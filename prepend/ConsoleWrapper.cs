using System;

using Prepend.Interfaces;

namespace Prepend {
    public class ConsoleWrapper : IConsole {

        public void WriteLine(string message) {
            Console.WriteLine(message);
        }

        public ConsoleKeyInfo ReadKey() {
            return Console.ReadKey();
        }
    }
}
