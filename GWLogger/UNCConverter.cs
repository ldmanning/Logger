using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Net;

namespace GWLogger
{
    class UNCConverter
    {
        public static string ConvertLocalPathToUNCPath(string fullPath)
        {

            string comp = System.Environment.MachineName;
            string path = fullPath.Replace(":", "$");
            string unc = @"\\" + comp + @"\" + path;
            return unc;
        }

        public static string ConvertLocalPathToIPBasedPath(string localPath)
        {
            string ipBasedPath = "";

            
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select name from win32_share where path ='" +
                localPath.Replace("\\", "\\\\") + "'");
            ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
            if(managementObjectCollection.Count != 0)
            {
                foreach(ManagementObject item in managementObjectCollection)
                {
                    //string computerName = returnMachineIP().ToString();
                    string computerName = Dns.GetHostName();
                    ipBasedPath = item["Name"] as String;
                    ipBasedPath = "\\\\" + computerName + "\\" + ipBasedPath;
                    return ipBasedPath;
                }
            }
            return ipBasedPath;
        }

        private static IPAddress returnMachineIP()
        {
            string hostname = Dns.GetHostName();
            return null;
        }
    }
}
