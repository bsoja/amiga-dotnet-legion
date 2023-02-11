using System.Reflection;

namespace AmigaNet.Legion.DesktopApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            // dotnet core sets current directory to the src folder by default
            // we need to change it to the folder where executable file location is
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

            var libsLoader = new MonoGameLibLoader();
            libsLoader.LoadLibs();

            var langId = "en";
            var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../../original/legion");

            if (args.Length >= 2)
            {
                langId = args[0];
                dataPath = args[1];
            }

            var resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), "data", langId);

            using (var game = new LegionGame(resourcesPath, dataPath))
            {
                game.Run();
            }
        }
    }
}



