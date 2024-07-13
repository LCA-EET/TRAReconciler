using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRAReconciler
{
    internal class TRAReference
    {
        private int _referenceID = 0;
        private string _referenceText = "";
        internal TRAReference(int referenceID, string referenceText)
        {
            _referenceID = referenceID;
            _referenceText = referenceText;
        }
        internal string ReferenceText
        {
            get
            {
                return _referenceText;
            }
        }
    }
}
