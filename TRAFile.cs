using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TRAReconciler
{
    internal class TRAFile
    {
        private HashSet<int> _usedReferences;
        private Dictionary<int, TRAReference> _references;
        private string _filePath;
        private bool _found = false;
        internal TRAFile(string path)
        {
            _filePath = path;
            _usedReferences = new HashSet<int>();
            _references = new Dictionary<int, TRAReference>();
            if (File.Exists(path))
            {
                ReadTRA();
            }
        }
        internal void AddUsedReference(int referenceID)
        {
            _usedReferences.Add(referenceID);
        }
        internal void MarkFound()
        {
            _found = true;
        }
        private void ReadTRA()
        {
            string traText = File.ReadAllText(_filePath);
            MatchCollection matches = Program.rx.Matches(traText);
            for(int i = 0; i < matches.Count; i++)
            {
                Match currentMatch = matches[i];
                int referenceID = int.Parse(currentMatch.Value.Substring(1));
                int nextMatchIndex = traText.Length - 1;
                int equalsIndex = traText.IndexOf("=", currentMatch.Index);
                string referenceText = "";
                if (i < (matches.Count - 1)){
                    nextMatchIndex = matches[i + 1].Index - 1;
                    referenceText = traText.Substring(equalsIndex + 1, (nextMatchIndex) - (equalsIndex + 1));
                }
                else
                {
                    referenceText = traText.Substring(equalsIndex + 1);
                }
                _references.Add(referenceID, new TRAReference(referenceID, referenceText));
            }
        }
        internal void WriteTRAFile()
        {
            if(_usedReferences.Count > 0)
            {
                List<int> refsSorted = _usedReferences.ToList<int>();
                refsSorted.Sort();
                string toWrite = "";
                for(int i = 0; i < refsSorted.Count; i++)
                {
                    int usedReference = refsSorted[i];
                    if (_references.ContainsKey(usedReference))
                    {
                        toWrite += "@" + usedReference + "=" + (_references[usedReference].ReferenceText).Trim() + Environment.NewLine;
                    }
                }
                File.WriteAllText(_filePath, toWrite);
            }
        }
    }
}
