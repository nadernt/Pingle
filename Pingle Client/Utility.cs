using PingleClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Pingle
{
    public class Utility
    {

        public static bool CheckIpValidation(string ipAddr)
        {
            IPAddress IP;
            bool flag = IPAddress.TryParse(ipAddr, out IP);
            return flag ? true : false;
        }

        /*
         * https://docs.microsoft.com/en-us/windows/desktop/cimwin32prov/computer-system-hardware-classes
         */
        public static List<string> GetSystemInfo(string hardwareInfo, string syntax)
        {
            ManagementObjectSearcher sInfo = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM " + hardwareInfo);
            List<string> collectedInfo = new List<string>();

            foreach (ManagementObject info in sInfo.Get())
            {
                collectedInfo.Add(Convert.ToString(info[syntax]));
                Console.WriteLine(Convert.ToString(info[syntax]));
            }
            return collectedInfo;
        }

        public static int diskSpeed()
        {

            byte[] data = new byte[1024];

            string path = System.IO.Path.GetTempFileName();

            int bytesPerSecond = 0;

            using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

                watch.Start();

                for (int i = 0; i < 1024; i++) fs.Write(data, 0, data.Length);

                fs.Flush();

                watch.Stop();

                bytesPerSecond = (int)((data.Length * 1024) / watch.Elapsed.TotalSeconds);
            }

            System.IO.File.Delete(path);

            Console.WriteLine(bytesPerSecond);

            return bytesPerSecond;

        }
        public static string HDDInfo()
        {
            ManagementObjectSearcher mosDisks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            string hddInfo = "";
            foreach (ManagementObject moDisk in mosDisks.Get())
            {
                hddInfo += "[";
                hddInfo += moDisk["MediaType"].ToString() + " | ";
                hddInfo += moDisk["InterfaceType"].ToString() + " | ";
                hddInfo += moDisk["Model"].ToString();
                hddInfo += "] ";
            }
            return hddInfo;
        }

        public static string GetFirewallName()
        {
            try
            {
                //select the proper wmi namespace depending of the windows version
                string WMINameSpace = System.Environment.OSVersion.Version.Major > 5 ? "SecurityCenter2" : "SecurityCenter";

                ManagementScope Scope;
                Scope = new ManagementScope(String.Format("\\\\{0}\\root\\{1}", "localhost", WMINameSpace), null);

                Scope.Connect();
                ObjectQuery Query = new ObjectQuery("SELECT * FROM FirewallProduct");
                ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Scope, Query);
                string strFireWallName="";
                foreach (ManagementObject WmiObject in Searcher.Get())
                {

                    strFireWallName += "["+ WmiObject["displayName"] + "]";
                }

                return strFireWallName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public static string AntivirusInstalled()
        {
            string WMINameSpace = System.Environment.OSVersion.Version.Major > 5 ? "SecurityCenter2" : "SecurityCenter";

            string wmipathstr = @"\\" + Environment.MachineName + @"\root\" + WMINameSpace;
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmipathstr, "SELECT * FROM AntivirusProduct");
                ManagementObjectCollection instances = searcher.Get();

                string strVirousScannerName = "";
                foreach (ManagementObject WmiObject in searcher.Get())
                {

                    strVirousScannerName += "[" + WmiObject["displayName"] + "]";
                }

                return strVirousScannerName;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public static string JsonReport(Dictionary<string, string> systemInformation)
        {
            var serializer = new DataContractJsonSerializer(systemInformation.GetType(), new DataContractJsonSerializerSettings()
            {
                UseSimpleDictionaryFormat = true
            });

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, systemInformation);

                byte[] bytes = stream.ToArray();
                string jsons = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                //Console.WriteLine(jsons);
                return jsons;
            }
        }

        public static string JsonReport(List<KeyValuePair<string, string>> systemInformation)
        {
            var serializer = new DataContractJsonSerializer(systemInformation.GetType(), new DataContractJsonSerializerSettings()
            {
                UseSimpleDictionaryFormat = true
            });

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, systemInformation);

                byte[] bytes = stream.ToArray();
                string jsons = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                //Console.WriteLine(jsons);
                return jsons;
            }
        }
        public static string JsonReport(List<PingValues> systemInformation)
        {
            var serializer = new DataContractJsonSerializer(systemInformation.GetType(), new DataContractJsonSerializerSettings()
            {
                UseSimpleDictionaryFormat = true
            });

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, systemInformation);

                byte[] bytes = stream.ToArray();
                string jsons = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                //Console.WriteLine(jsons);
                return jsons;
            }
        }
    }
}
