using Prepend.Interfaces;
using Prepend.Lib;
using System;
using System.Linq;

namespace Prepend {

    public class ArgumentsLogic : IArgumentsLogic {

        private readonly string[] _args;
        public ArgumentsLogic(string[] args) {
            _args = args;
        }

        public CommandType Command {
            get {
                if (_args.ToList().Contains("--help")) {
                    return CommandType.Usage;
                } else if (_args.ToList().Contains("--remove")) {
                    return CommandType.Remove;
                } else {
                    return CommandType.Prepend;
                }
            }
        }

        public string GetFolderPath() {
            var folderPath = _args.FirstOrDefault(_ => _.ToLower().StartsWith("--folder-path="));
            if (folderPath == null) {
                throw new ArgumentException("Invalid Folder Path");
            }
            return folderPath.Substring("--folder-path".Length + 1);
        }

        public string GetPrependText() {
            var prependText = _args.FirstOrDefault(_ => _.ToLower().StartsWith("--prepend-text="));
            if (prependText == null) {
                throw new ArgumentException("Invalid Prepend Text");
            }
            return prependText.Substring("--prepend-text=".Length);
        }

        public int GetFileNumberSeed() {
            var prependText = _args.FirstOrDefault(_ => _.ToLower().StartsWith("--file-number-seed="));
            if (prependText == null) {
                return (1);
            }

            if (int.TryParse(prependText.Substring("--file-number-seed=".Length), out var intTemp)) {
                return intTemp;
            } else {
                throw new ArgumentException("Invalid format for File Number Seed");
            }
        }

    }
}
