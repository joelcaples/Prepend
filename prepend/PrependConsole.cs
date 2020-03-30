using Microsoft.Extensions.DependencyInjection;
using Prepend.Interfaces;
using Prepend.Lib;
using System;
using System.IO;
using System.IO.Abstractions;

namespace Prepend {

    public class PrependConsole {

        private readonly IArgumentsLogic _argumentsLogic;
        private readonly IConsole _console;
        private readonly IFileSystem _fileSystem;

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

            _console.WriteLine(
            $@"
            Usage: prepend [Command] 
                    --folder-path=<value> 
                    --prepend-text=<value>
                    --file-number-seed=<value>

            Commands
                --help                        Print help

                --remove                      Removes matching prepend-text

            Parameters
                --folder-path=<value>         Folder path/File pattern for affected files

                --prepend-text=<value>        Prepend text pattern

                --file-number-seed=<value>    Will replace any instances of '#' in 
                                                the filenmae with a number that is incremented 
                                                for each file within the folder.

                                              Default = 1


            Examples:
                prepend --folder-path=D:\Files\*.mkv --prepend-text=""s01e### - "" --file-number-seed=0
                prepend --folder-path=D:\Files\*.mkv --prepend-text=""s01e### - "" --remove
            ");
        }
    }
}
