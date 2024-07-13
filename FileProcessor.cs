using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TRAReconciler
{
    internal class FileProcessor
    {
        private string _modPath, _traPath;
        private Dictionary<string, TRAFile> _traFiles;
        public FileProcessor(string modPath, string traPath) 
        {
            _traFiles = new Dictionary<string, TRAFile>();
            _modPath = modPath;
            _traPath = traPath;
            ProcessTRAFiles();
            ProcessComponentFiles(".baf");
            ProcessComponentFiles(".d");
            Console.WriteLine("Writing reconciled TRAs...");
            foreach(TRAFile traFile in _traFiles.Values)
            {
                traFile.WriteTRAFile();
            }
        }
        private void ProcessComponentFiles(string extension)
        {
            string[] componentFilePaths = Directory.GetFiles(_modPath, "*" + extension, SearchOption.AllDirectories);
            foreach (string componentPath in componentFilePaths)
            {
                string fileID = FileID(componentPath);
                Console.WriteLine(componentPath);
                if (_traFiles.ContainsKey(fileID))
                {
                    Console.WriteLine("Processing component file: " + componentPath);
                    ComponentFile cf = new ComponentFile(componentPath, _traFiles[fileID]);
                }
            }
        }

        private void ProcessTRAFiles()
        {
            string[] traFilePaths = Directory.GetFiles(_traPath, "*.tra");
            foreach(string traFilePath in traFilePaths)
            {
                TRAFile traFile = new TRAFile(traFilePath);
                _traFiles.Add(FileID(traFilePath), traFile);
            }
        }

        private string FileID(string path)
        {
            string[] splitPath;
            if (path.Contains("\\"))
            {
                splitPath = path.Split("\\", StringSplitOptions.TrimEntries);
            }
            else
            {
                splitPath = path.Split("/", StringSplitOptions.TrimEntries);
            }
            string filename = splitPath[splitPath.Length - 1].ToLower();
            return filename.Split(".")[0];
        }
    }
}
