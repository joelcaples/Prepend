using Microsoft.Extensions.DependencyInjection;
using Prepend.Interfaces;
using System;
using System.IO;
using System.IO.Abstractions;

namespace Prepend {

    public class PrependConsole {

        private IArgumentsLogic _argumentsLogic;
        private IConsole _console;
        private IFileSystem _fileSystem;

        public PrependConsole(IServiceCollection serviceCollection) {
            _argumentsLogic = serviceCollection.BuildServiceProvider().GetRequiredService<IArgumentsLogic>();
            _console = serviceCollection.BuildServiceProvider().GetRequiredService<IConsole>();
            _fileSystem = serviceCollection.BuildServiceProvider().GetRequiredService<IFileSystem>();
        }

        public void Run() {

            try {
                var prependLogic = new PrependLogic(_fileSystem);

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
                _console.WriteLine(ex.Message);
                Usage();
            } catch (Exception ex) {
                _console.WriteLine(ex.Message);
                Usage();
            }
        }

        public bool ShowRenameDialog(string file, string newFileName) {

            var result = ' ';

            while (result != 'y' && result != 'n') {

                _console.WriteLine($"Rename {Path.GetFileName(file)} to: ");
                _console.WriteLine($"       {Path.GetFileName(newFileName)}?");
                _console.WriteLine(string.Empty);
                _console.WriteLine("Enter y or n");

                result = _console.ReadKey().KeyChar;
                if (result == 'y') {
                    return true;
                }

                _console.WriteLine(string.Empty);
            }
            return false;
        }

        private void Usage() {

            _console.WriteLine("Usage: Prepend [Command] --folder-path=<value> --prepend-text=<value>");
            _console.WriteLine("");
            _console.WriteLine("Commands");
            _console.WriteLine("    --help                    Print help");
            _console.WriteLine("    --remove                  Removes matching prepend-text");
            _console.WriteLine("");
            _console.WriteLine("Parameters");
            _console.WriteLine("    --folder-path=<value>     Folder path/File pattern for affected files");
            _console.WriteLine("    --prepend-text=<value>    Prepend text pattern");
        }
    }
}
