﻿using System.IO.Abstractions;
using System.Linq;
using System.Text.RegularExpressions;

namespace Prepend.Lib {
    public class PrependLogic {

        public delegate bool ConfirmationPrompt(string file, string newFileName);

        private readonly IFileSystem _fileSystem;
        public PrependLogic(IFileSystem fileSystem) {
            _fileSystem = fileSystem;
        }

        public void AddPrependText(string folderPath, string prependText, int fileNumberSeed, ConfirmationPrompt confirmationPrompt) {

            var fileNumber = fileNumberSeed;

            foreach (var file in _fileSystem.Directory.GetFiles(_fileSystem.Path.GetDirectoryName(folderPath), _fileSystem.Path.GetFileName(folderPath))) {

                var formattedPrependText = prependText.Clone().ToString();

                for (var i = 10; i > 0; --i) {
                    formattedPrependText = formattedPrependText.Replace(Poundage(i), fileNumber.ToString().PadLeft(i, '0'));
                }
                fileNumber++;

                var newFileName = _fileSystem.Path.Combine(_fileSystem.DirectoryInfo.FromDirectoryName(file).Parent.FullName, formattedPrependText + _fileSystem.Path.GetFileName(file));

                if(confirmationPrompt(file, newFileName))
                    _fileSystem.File.Move(file, newFileName);
            }
        }

        public void RemovePrependedText(string folderPath, string prependText, ConfirmationPrompt confirmationPrompt) {

            for (var i = 10; i > 0; --i) {
                prependText = prependText.Replace(Poundage(i), @"(\d)*");
            }

            Regex reg = new Regex(prependText);
            foreach (var file in _fileSystem.Directory.GetFiles(_fileSystem.Path.GetDirectoryName(folderPath), _fileSystem.Path.GetFileName(folderPath)).Where(path => reg.IsMatch(path)).ToList()) {
                string newFileName = _fileSystem.Path.Combine(_fileSystem.DirectoryInfo.FromDirectoryName(file).Parent.FullName, _fileSystem.Path.GetFileName(file).Substring(reg.Match(_fileSystem.Path.GetFileName(file)).Length));
                if(confirmationPrompt(file, newFileName)) {
                    _fileSystem.File.Move(file, newFileName);
                }
            }
        }

        private static string Poundage(int numChars) {

            var retVal = string.Empty;

            for (int i = 0; i < numChars; ++i) {
                retVal += "#";
            }

            return retVal;
        }

    }
}
