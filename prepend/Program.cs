using Microsoft.Extensions.DependencyInjection;
using Prepend.Interfaces;
using System.IO.Abstractions;

namespace Prepend {

    class Prepend {

        static void Main(string[] args) {

            var serviceCollection = new ServiceCollection()
                    .AddSingleton<IArgumentsLogic>(new ArgumentsLogic(args))
                    .AddSingleton<IConsole>(new ConsoleWrapper())
                    .AddSingleton<IFileSystem>(new FileSystem());

            var app = new PrependConsole(serviceCollection);

            app.Run();
        }
    }
}
