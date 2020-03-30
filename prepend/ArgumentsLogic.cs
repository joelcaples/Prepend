using System;
using System.Linq;

namespace Prepend {

    public class ArgumentsLogic {

        private readonly string[] _args;
        public ArgumentsLogic(string[] args) {
            _args = args;
        }

        public string FolderPath {
            get {
                var folderPath = _args.FirstOrDefault(_ => _.ToLower().StartsWith("--folder-path="));
                if (folderPath == null) {
                    throw new ArgumentException("Invalid Folder Path");
                }
                return folderPath.Substring("--folder-path".Length + 1);
            }
        }

        public string PrependText {
            get {
                var prependText = _args.FirstOrDefault(_ => _.ToLower().StartsWith("--prepend-text="));
                if (prependText == null) {
                    throw new ArgumentException("Invalid Prepend Text");
                }
                return prependText.Substring("--prepend-text=".Length);
            }
        }

        public int FileNumberSeed {
            get {
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
}
