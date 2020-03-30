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
                var argLogic = new ArgumentsLogic(args);
                var prependLogic = new PrependLogic();
                
                switch(argLogic.Command) {
                    case CommandType.Prepend:
                        prependLogic.AddPrependText(argLogic.FolderPath, argLogic.PrependText, argLogic.FileNumberSeed, ShowRenameDialog);
                        break;
                    case CommandType.Remove:
                        prependLogic.RemovePrependedText(argLogic.FolderPath, argLogic.PrependText, ShowRenameDialog);
                        break;
                    default:
                        Usage();
                        break;
                }

            } catch (ArgumentException ex) {
                Console.WriteLine(ex.Message);
                Usage();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Usage();
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

        private static void Usage() {

            Console.WriteLine("Usage: Prepend [Command] --folder-path=<value> --prepend-text=<value>");
            Console.WriteLine("");
            Console.WriteLine("Commands");
            Console.WriteLine("    --help                    Print help");
            Console.WriteLine("    --remove                  Removes matching prepend-text");
            Console.WriteLine("");
            Console.WriteLine("Parameters");
            Console.WriteLine("    --folder-path=<value>     Folder path/File pattern for affected files");
            Console.WriteLine("    --prepend-text=<value>    Prepend text pattern");
        }
    }
}
