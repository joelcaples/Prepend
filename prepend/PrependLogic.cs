using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Prepend {
    public class PrependLogic {

        public delegate void ConfirmationPrompt(string file, string newFileName);

        public void AddPrependText(string folderPath, string prependText, int fileNumber, ConfirmationPrompt confirmationPrompt) {

            foreach (var file in Directory.GetFiles(Path.GetDirectoryName(folderPath), Path.GetFileName(folderPath))) {

                var formattedPrependText = prependText.Clone().ToString();

                for (var i = 10; i > 0; --i) {
                    formattedPrependText = formattedPrependText.Replace(poundage(i), fileNumber.ToString().PadLeft(i, '0'));
                }
                fileNumber++;

                var newFileName = Path.Combine(new DirectoryInfo(file).Parent.FullName, formattedPrependText + Path.GetFileName(file));

                confirmationPrompt(file, newFileName);

            }
        }

        public void RemovePrependedText(string folderPath, string prependText, ConfirmationPrompt confirmationPrompt) {

            for (var i = 10; i > 0; --i) {
                prependText = prependText.Replace(poundage(i), @"(\d)*");
            }

            Regex reg = new Regex(prependText);
            foreach (var file in Directory.GetFiles(Path.GetDirectoryName(folderPath), Path.GetFileName(folderPath)).Where(path => reg.IsMatch(path)).ToList()) {
                string newFileName = Path.Combine(new DirectoryInfo(file).Parent.FullName, Path.GetFileName(file).Substring(reg.Match(Path.GetFileName(file)).Length));
                confirmationPrompt(file, newFileName);
            }
        }

        private static string poundage(int numChars) {

            var retVal = string.Empty;

            for (int i = 0; i < numChars; ++i) {
                retVal += "#";
            }

            return retVal;
        }

    }
}
