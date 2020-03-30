using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Prepend
{
    class Prepend
    {
        static void Main(string[] args) {

            try {
                if (args.ToList().Contains("--remove")) {
                    RemovePrependedText(FolderPath(args.ToList()), PrependText(args.ToList()));
                } else {
                    AddPrependText(FolderPath(args.ToList()), PrependText(args.ToList()), FileNumberSeed(args.ToList()));
                }
            } catch (ArgumentException ex) {
                Console.WriteLine(ex.Message);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Usage();
            }
        }

        private static void AddPrependText(string folderPath, string prependText, int fileNumber) {

            foreach (var file in Directory.GetFiles(Path.GetDirectoryName(folderPath), Path.GetFileName(folderPath))) {

                var formattedPrependText = prependText.Clone().ToString();

                for (var i = 10; i > 0; --i) {
                    formattedPrependText = formattedPrependText.Replace(poundage(i), fileNumber.ToString().PadLeft(i, '0'));
                }
                fileNumber++;

                var newFileName = Path.Combine(new DirectoryInfo(file).Parent.FullName, formattedPrependText + Path.GetFileName(file));

                ShowRenameDialog(file, newFileName);

            }
        }

        private static void RemovePrependedText(string folderPath, string prependText) {

            for (var i = 10; i > 0; --i) {
                prependText = prependText.Replace(poundage(i), @"(\d)*");
            }

            Regex reg = new Regex(prependText);
            foreach (var file in Directory.GetFiles(Path.GetDirectoryName(folderPath), Path.GetFileName(folderPath)).Where(path => reg.IsMatch(path)).ToList()) {
                string newFileName = Path.Combine(new DirectoryInfo(file).Parent.FullName, Path.GetFileName(file).Substring(reg.Match(Path.GetFileName(file)).Length));
                ShowRenameDialog(file, newFileName);
            }
        }

        private static string FolderPath(List<string> args) {

            var folderPath = args.FirstOrDefault(_ => _.ToLower().StartsWith("--folder-path="));
            if (folderPath == null)
            {
                throw new ArgumentException("Invalid Folder Path");
            }
            return folderPath.Substring("--folder-path".Length+1);
        }

        private static string PrependText(List<string> args) {

            var prependText = args.FirstOrDefault(_ => _.ToLower().StartsWith("--prepend-text="));
            if (prependText == null)
            {
                throw new ArgumentException("Invalid Prepend Text");
            }
            return prependText.Substring("--prepend-text=".Length);
        }

        private static int FileNumberSeed(List<string> args) {

            var prependText = args.FirstOrDefault(_ => _.ToLower().StartsWith("--file-number-seed="));
            if (prependText == null) {
                return (1);
            }

            if(int.TryParse(prependText.Substring("--file-number-seed=".Length), out var intTemp)) {
                return intTemp;
            } else {
                throw new ArgumentException("Invalid format for File Number Seed");
            }
        }

        public static void ShowRenameDialog(string file, string newFileName) {

            var result = ' ';

            while (result != 'y' && result != 'n') {

                Console.WriteLine($"Rename {Path.GetFileName(file)} to: ");
                Console.WriteLine($"       {Path.GetFileName(newFileName)}?");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Enter y or n");

                result = Console.ReadKey().KeyChar;
                if (result == 'y') {
                    File.Move(file, newFileName);
                }

                Console.WriteLine(string.Empty);
            }
        }

        private static string poundage(int numChars) {

            var retVal = string.Empty;

            for(int i=0;i<numChars;++i) {
                retVal += "#";
            }

            return retVal;
        }

        private static void Usage() {

            Console.WriteLine("Usage...");
        }
    }
}
