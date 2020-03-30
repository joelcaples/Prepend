using System;
using System.Collections.Generic;
using System.Text;

namespace Prepend.Interfaces {
    public interface IArgumentsLogic {
        CommandType Command { get; }
        string GetFolderPath();
        string GetPrependText();
        int GetFileNumberSeed();
    }
}
