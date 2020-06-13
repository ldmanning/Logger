using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.IO;

namespace GWLogger
{
    public class Logger
    {

        public enum loggingLevel
        {
            Info = 0,
            Warning = 1,
            Error = 2
        }

        private static Logger current;
        public static Logger Current { get { return current; } }

        
        private static List<clsException> exceptionList;
        private static List<clsInfo> infoList;
        private string programName;
        private loggingLevel logLevel = loggingLevel.Info;        
        private string logDirectory;
        private string logFilePath;
        private string passedInfo;

        private StreamWriter logWriter;

        private DateTime logCreated;

        public static void Open(string programName, loggingLevel logLevel, string logDirectory, bool log = false)
        {
            current = new Logger(programName, logLevel, logDirectory, log);
        }

        public Logger(string programName, loggingLevel logLevel, string logDirectory, bool log)
        {
            exceptionList = new List<clsException>();
            infoList = new List<clsInfo>();

            this.programName = programName;
            this.logLevel = logLevel;
            this.logDirectory = logDirectory;
            this.logCreated = DateTime.Now;
            this.logFilePath = logDirectory + logCreated.ToString("MM-yyyy");                     
            
            current = this;
        }

        public void Log(Exception ex, string passedInfo = "",
            [System.Runtime.CompilerServices.CallerMemberName] string pMemberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string pSourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int pSourceLineNumber = 0)
        {
            
        
            exceptionList.Add(new clsException(ex,passedInfo,pMemberName,pSourceFilePath,pSourceLineNumber));

        }

        public void Log(string info,
            [System.Runtime.CompilerServices.CallerMemberName] string pMemberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string pSourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int pSourceLineNumber = 0)
        {
            infoList.Add(new clsInfo(info,pMemberName, pSourceFilePath, pSourceLineNumber));
        }

        private void LogToFile(string info, loggingLevel logLevel, string memberName, string filePath, int lineNumber)
        {
            try
            {
                if (logWriter == null)
                {
                    Logger.Open(this.programName, this.logLevel, this.logDirectory, true);
                }

                if (logWriter != null)
                {
                    logWriter.BaseStream.Seek(0, SeekOrigin.End);
                    DateTime now = DateTime.Now;

                    string location = "";
                    if (filePath != "")
                    {
                        location = String.Format("Method: {0} Line# {1} File: {2}", memberName, lineNumber, filePath);
                    }

                    logWriter.WriteLine(logLevel.ToString() + " " + location);
                    logWriter.WriteLine("\t\t" + info);
                    logWriter.Flush();
                }             
                
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LogToFile(clsException ex, StreamWriter logWriter)
        {
            string location = String.Format("Method: {0} Line# {1} File: {2} Additional Info: {3}", ex.CallerName, ex.LineNumber, ex.SourceFile, ex.PassedInfo);
            logWriter.WriteLine(loggingLevel.Error + " " + location);
            logWriter.WriteLine("\t\t" + ex.Ex);
            logWriter.Flush();

        }

        private void LogToFile(clsInfo I, StreamWriter logWriter)
        {
            string location = String.Format("Method: {0} Line# {1} File: {2}", I.CallerName, I.LineNumber, I.SourceFile);
            logWriter.WriteLine(loggingLevel.Info + " " + location);
            logWriter.WriteLine("\t\t" + I.S);
            logWriter.Flush();
        }

        

        public void Close(object sender, EventArgs e)
        {       

            
            if (exceptionList.Any())
            {
                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }
                logFilePath += "\\" + logCreated.ToString("dd-MM-yyyHH-mm") + ".log";
                FileStream fileStream = new FileStream(logFilePath, FileMode.Create, FileAccess.Write);
                logWriter = new StreamWriter(fileStream);

                foreach (clsException cEx in exceptionList)
                {
                    LogToFile(cEx, logWriter);
                }


                string uncPath = UNCConverter.ConvertLocalPathToUNCPath(logFilePath);
                sendEmail("View log <a href=\"" + uncPath + "\">here</a>", "Errors detected in " + programName);
            }

            if (infoList.Any())
            {
                if (!Directory.Exists(logFilePath + "InfoOnly"))
                {
                    Directory.CreateDirectory(logFilePath + "InfoOnly");
                }
                logFilePath += "InfoOnly\\" + logCreated.ToString("dd-MM-yyyHH-mm") + ".log";
                FileStream fileStream = new FileStream(logFilePath, FileMode.Create, FileAccess.Write);
                logWriter = new StreamWriter(fileStream);
                foreach (clsInfo s in infoList)
                {
                    LogToFile(s, logWriter);
                }
                
            }
        }       

        

        private static void sendEmail(string body, string subject, string to = "IT@gicw.org", string from = "IT@gicw.org")
        {
            try
            {                
                MailMessage mail = new MailMessage(from, to);
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.UseDefaultCredentials = false;
                client.Host = "mail.gicw.org";
                mail.IsBodyHtml = true;
                mail.Subject = subject;
                mail.Body = body;
                client.Send(mail);
            }
            catch (Exception ex)
            {

            }
        }


    }
}
