using System.Text.RegularExpressions;

namespace TRAReconciler
{
    internal class Program
    {
        internal static Regex rx = new Regex("@[0-9]+", RegexOptions.Compiled);
        static void Main(string[] args)
        {
            Console.WriteLine("WARNING: It is highly recommended that you backup your translation files before running this program.");
            Console.WriteLine("Provide the absolute path to the mod root directory.");
            string modPath = Console.ReadLine();
            if(modPath != null)
            {
                VerifyDirectory(modPath);
            }
            else
            {
                Environment.Exit(0);
            }
            Console.WriteLine("Provide the absolute path to the english language translation root directory.");
            string traPath = Console.ReadLine();
            if (traPath != null)
            {
                VerifyDirectory(traPath);
            }
            else
            {
                Environment.Exit(0);
            }
            FileProcessor processor = new FileProcessor(modPath, traPath);

        }
        static void VerifyDirectory(string path)
        {
            if(!Directory.Exists(path))
            {
                Console.WriteLine("The path provided does not exist. Exiting.");
                Environment.Exit(0);
            }
        }
    }
}
