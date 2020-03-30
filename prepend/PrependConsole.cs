using Microsoft.Extensions.DependencyInjection;
using Prepend.Interfaces;
using System;
using System.IO;
using System.IO.Abstractions;

namespace Prepend {
    public class PrependConsole {

        private IArgumentsLogic _argumentsLogic;

        public PrependConsole(IServiceCollection serviceCollection) {
            _argumentsLogic = serviceCollection.BuildServiceProvider().GetRequiredService<IArgumentsLogic>();
        }

        public void Run() {

            try {
                var prependLogic = new PrependLogic(new FileSystem());

                switch (_argumentsLogic.Command) {
                    case CommandType.Prepend:
                        prependLogic.AddPrependText(_argumentsLogic.GetFolderPath(), _argumentsLogic.GetPrependText(), _argumentsLogic.GetFileNumberSeed(), ShowRenameDialog);
                        break;
                    case CommandType.Remove:
                        prependLogic.RemovePrependedText(_argumentsLogic.GetFolderPath(), _argumentsLogic.GetPrependText(), ShowRenameDialog);
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

        public static bool ShowRenameDialog(string file, string newFileName) {

            var result = ' ';

            while (result != 'y' && result != 'n') {

                Console.WriteLine($"Rename {Path.GetFileName(file)} to: ");
                Console.WriteLine($"       {Path.GetFileName(newFileName)}?");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Enter y or n");

                result = Console.ReadKey().KeyChar;
                if (result == 'y') {
                    return true;
                }

                Console.WriteLine(string.Empty);
            }
            return false;
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
