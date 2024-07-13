using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TRAReconciler
{
    internal class ComponentFile
    {
        private TRAFile _associatedTRA;
        internal ComponentFile(string path, TRAFile associatedTRA)
        {
            _associatedTRA = associatedTRA;
            if (File.Exists(path))
            {
                ReadComponentFile(path);
            }
        }
        private void ReadComponentFile(string path)
        {      
            string fileText = File.ReadAllText(path);
            MatchCollection matches = Program.rx.Matches(fileText);
            foreach(Match match in matches)
            {
                int usedReference = int.Parse(match.Value.Substring(1));
                _associatedTRA.AddUsedReference(usedReference);
            }
        }
    }
}
