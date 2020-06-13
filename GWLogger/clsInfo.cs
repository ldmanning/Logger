using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWLogger
{
    class clsInfo
    {
        private string s;

        public clsInfo(string s, string callerName, string sourceFile, int lineNumber)
        {
            this.s = s;
            this.callerName = callerName;
            this.sourceFile = sourceFile;
            this.lineNumber = lineNumber;            
        }       

        public string S
        {
            get { return s; }
            set { s = value; }
        }
        private string callerName;

        public string CallerName
        {
            get { return callerName; }
            set { callerName = value; }
        }
        private string sourceFile;

        public string SourceFile
        {
            get { return sourceFile; }
            set { sourceFile = value; }
        }
        private int lineNumber;

        public int LineNumber
        {
            get { return lineNumber; }
            set { lineNumber = value; }
        }
    }
}
