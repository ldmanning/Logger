using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWLogger
{
    class clsException
    {
        private Exception ex;

        public clsException(Exception ex, string passedInfo,string callerName, string sourceFile, int lineNumber)
        {
            this.ex = ex;
            this.callerName = callerName;
            this.sourceFile = sourceFile;
            this.lineNumber = lineNumber;
            this.passedInfo = passedInfo;
        }

        private string passedInfo;

        public string PassedInfo
        {
            get { return passedInfo; }
            set { passedInfo = value; }
        }

        public Exception Ex
        {
            get { return ex; }
            set { ex = value; }
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
