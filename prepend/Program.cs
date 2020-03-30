using Microsoft.Extensions.DependencyInjection;
using Prepend.Interfaces;


namespace Prepend {

    class Prepend {

        static void Main(string[] args) {

            var serviceCollection = new ServiceCollection()
                    .AddSingleton<IArgumentsLogic>(new ArgumentsLogic(args));

            var app = new PrependConsole(serviceCollection);

            app.Run();
        }
    }
}
