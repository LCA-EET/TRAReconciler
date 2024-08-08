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
        private bool _tphMode;
        private string _modPath, _traPath;
        private Dictionary<string, TRAFile> _traFiles;
        private TRAFile _lastProcessed;
        public FileProcessor(string modPath, string traPath, bool tphMode) 
        {
            _tphMode = tphMode;
            _traFiles = new Dictionary<string, TRAFile>();
            _modPath = modPath;
            _traPath = traPath;
            if (tphMode)
            {
                ProcessTPH();
            }
            else
            {
                ProcessNonTPH();
            }
        }
        private void ProcessTPH()
        {
            ProcessTRAFile(_traPath);
            ProcessComponentFiles(".tph");
            ProcessComponentFiles(".tp2");
            Console.WriteLine("Writing reconciled TRA...");
            _lastProcessed.WriteTRAFile();
        }
        private void ProcessNonTPH()
        {
            ProcessTRAFiles();
            ProcessComponentFiles(".baf");
            ProcessComponentFiles(".d");
            Console.WriteLine("Writing reconciled TRAs...");
            foreach (TRAFile traFile in _traFiles.Values)
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
                if (!_tphMode)
                {
                    if (_traFiles.ContainsKey(fileID))
                    {
                        Console.WriteLine("Processing component file: " + componentPath);
                        ComponentFile cf = new ComponentFile(componentPath, _traFiles[fileID]);
                    }
                }
                else
                {
                    Console.WriteLine("Processing component file: " + componentPath);
                    ComponentFile cf = new ComponentFile(componentPath, _lastProcessed);
                }
                
            }
        }
        private void ProcessTRAFile(string traFilePath)
        {
            _lastProcessed = new TRAFile(traFilePath);
            _traFiles.Add(FileID(traFilePath), _lastProcessed);
        }
        private void ProcessTRAFiles()
        {
            string[] traFilePaths = Directory.GetFiles(_traPath, "*.tra");
            foreach(string traFilePath in traFilePaths)
            {
                ProcessTRAFile(traFilePath);
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
