using System;

namespace Prepend.Interfaces {
    public interface IConsole {

        void WriteLine(string message);
        ConsoleKeyInfo ReadKey();
    }
}
