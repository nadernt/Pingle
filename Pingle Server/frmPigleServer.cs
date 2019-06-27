using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pingle_Server
{
    public partial class frmPigleServer : Form
    {
        private readonly string BASE_PATH = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        private static HttpListener listener = new HttpListener();
        private static bool terminate = true;

        private static bool NetworkIsAvailable = false;
        private const string USER_MESSAGE = "Please type this address in the client computer application\nthen press start.";
        private readonly string RESOURCE_PATH;
        private readonly string RES_IMAGE_PATH;
        private static Dictionary<string, string> systemInfos = new Dictionary<string, string>();

        class NetWorkList
        {
            public string Name { get; set; }
            public string Address { get; set; }

            public string Description
            {
                get
                {
                    return string.Format("{0}, {1}", Name, Address);
                }
            }
        }

        public frmPigleServer()
        {
            RESOURCE_PATH = BASE_PATH + Path.DirectorySeparatorChar + "resources" + Path.DirectorySeparatorChar;
            RES_IMAGE_PATH = RESOURCE_PATH + "images" + Path.DirectorySeparatorChar;
            InitializeComponent();
        }

        private void AddressChangedCallback(object sender, EventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {

                UpdateUINetStatus(true);
            }
            else
            {
                UpdateUINetStatus(false);
            }
        }

        private static bool NetworkCheck()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                NetworkIsAvailable = true;
                return true;
            }
            else
            {
                NetworkIsAvailable = false;

                return false;
            }

        }

        private bool CheckIfPortIsInUse(int port)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                // Console.WriteLine(endPoint.Port);
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }

            return inUse;
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {

            if (NetworkCheck())
            {
                if (terminate)
                {

                    if (string.IsNullOrEmpty(txtPort.Text.Trim()))
                        txtPort.Text = "80";
                    if (CheckIfPortIsInUse(Convert.ToInt32(txtPort.Text)))
                    {
                        MessageBox.Show("The port is not available or occupied by another program.");
                        return;
                    }
                    terminate = false;
                    listener = new HttpListener();
                    //string[] prefix = { $"http://*:{txtPort.Text.Trim()}/" };
                    string[] prefix = { $"http://{(string)cmbIPList.SelectedValue}:{txtPort.Text.Trim()}/" };
                    //string[] prefix = { "http://127.0.0.1:80/" };
                    btnStartServer.Text = "Stop\nServer";
                    var t = new Task(() => MyListener(prefix, Convert.ToInt32(numericFileSize.Value), Convert.ToInt32(numericUpDown1.Value)));
                    t.Start();
                }
                else
                {
                    btnStartServer.Text = "Start\nServer";
                    terminate = true;
                    if (listener.IsListening)
                    {
                        // listener.Abort();
                        listener.Stop();
                        listener.Close();
                    }
                }
            }
        }

        private static void ProgListnerSysPermissionAsync()
        {

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "netsh.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "http add urlacl url=http://*:80/ user=Everyone listen=yes";

            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
            }

        }

        public static void MyListener(string[] prefixes, int dummyFileSize,int bandwidthLimit)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

             foreach (string s in prefixes)
             {
                 listener.Prefixes.Add(s);
             }
            //listener.Prefixes.Add("http://169.254.201.60:80/");
            listener.Start();

            Console.WriteLine("Listening...");



            while (!terminate)
            {
                try
                {
                    // Note: The GetContext method blocks while waiting for a request. 
                    HttpListenerContext context = listener.GetContext();

                    HttpListenerRequest request = context.Request;
                    //foreach (string query in request.QueryString)
                    //Console.WriteLine(query.ToString());
                    Console.WriteLine("Request: " + request.RawUrl);

                    if (request.RawUrl.Contains("download"))
                    {
                        Task.Factory.StartNew((ctx) =>
                        {
                            WriteFile((HttpListenerContext)ctx, dummyFileSize, bandwidthLimit);
                        }, context, TaskCreationOptions.LongRunning);
                    }
                    else if (request.RawUrl.Contains("report"))
                    {
                        // Obtain a response object.
                        HttpListenerResponse response = context.Response;
                        // Construct a response.
                        string responseString = Pingle.Utility.JsonReport(systemInfos);
                        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                        // Get a response stream and write the response to it.
                        response.ContentLength64 = buffer.Length;
                        Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                    }
                    else
                    {
                        // Obtain a response object.
                        HttpListenerResponse response = context.Response;
                        // Construct a response.
                        string responseString = "PINGLE_OK";
                        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                        // Get a response stream and write the response to it.
                        response.ContentLength64 = buffer.Length;
                        Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                    }
                }
                catch
                {
                    break;
                }

            }

        }

        static void WriteFile(HttpListenerContext ctx, int dummyFileSize,int bandwidthLimit)
        {
            var response = ctx.Response;
            string strFootPrint = new string('C', dummyFileSize * 1000000);
            if (strFootPrint == null)
                throw new System.ArgumentException("Can not allocate memory!");
            var dummyBuffer = Encoding.ASCII.GetBytes(strFootPrint);

            using (MemoryStream fs = new MemoryStream(dummyBuffer))
            {
                try
                {

                    string filename = "test.zip";
                    response.ContentLength64 = fs.Length;
                    response.SendChunked = false;
                    response.ContentType = System.Net.Mime.MediaTypeNames.Application.Octet;
                    response.AddHeader("Content-disposition", "attachment; filename=" + filename);

                    byte[] buffer = new byte[64 * 1024];
                    int read;
                    using (BinaryWriter bw = new BinaryWriter(response.OutputStream))
                    {
                        while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            bw.Write(buffer, 0, read);
                            if(bandwidthLimit>0)
                                Thread.Sleep(bandwidthLimit);
                            bw.Flush();
                        }

                        bw.Close();
                    }
                }
                catch (Exception ex)
                {

                }

                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusDescription = "OK";
                response.OutputStream.Close();

            }
        }

        /// <summary>
        /// Gets the IP address of the current PC
        /// </summary>
        /// <returns></returns>
        private static IPAddress GetIPAddress()
        {
            String strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            foreach (IPAddress ip in addr)
            {
                //Console.WriteLine(ip);
                if (!ip.IsIPv6LinkLocal)
                {
                    return (ip);
                }
            }
            return addr.Length > 0 ? addr[0] : null;
        }

        private void cmbIPList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (NetworkIsAvailable)
            {
                lbShowIP.Text = (string)cmbIPList.SelectedValue + ":";
                txtPort.Left = lbShowIP.Right;
                lbMessageToUser.Text = USER_MESSAGE;
            }
        }

        private bool FireallRuleExist()
        {
            try
            {
                INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                firewallRule.Description = "Allow Pingle Client";
                firewallRule.ApplicationName = BASE_PATH + "\\" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe";
                firewallRule.Enabled = true;
                firewallRule.InterfaceTypes = "All";
                firewallRule.Name = "Pingle Client";

                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                    Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                firewallPolicy.Rules.Item(firewallRule.Name);
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        private void AddFirewallRule(NET_FW_RULE_DIRECTION_ inbound_outbound)
        {
            INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            firewallRule.Description = "Allow Pingle Server";
            firewallRule.ApplicationName = BASE_PATH + "\\" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe";
            firewallRule.Enabled = true;
            firewallRule.InterfaceTypes = "All";
            firewallRule.Name = "Pingle Server";

            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            Thread.Sleep(1000);

            if (inbound_outbound == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN)
                firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
            else
                firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;

            firewallPolicy.Rules.Add(firewallRule);
            Console.WriteLine("Rules applied to Firewall.");
        }

        private void frmPigleServer_Load(object sender, EventArgs e)
        {
            
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C netsh http add iplisten 0.0.0.0";
            process.StartInfo = startInfo;
            process.Start();

            if (!FireallRuleExist())
            {
                AddFirewallRule(NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN);
                AddFirewallRule(NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT);
            }


            cmbIPList.DropDownStyle = ComboBoxStyle.DropDownList;

            lbShowIP.Font = new Font("Microsoft Sans Serif", 40F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            lbShowIP.BackColor = Color.White;
            lbShowIP.ForeColor = Color.Gray;
            lbFileSize.Text = "Upload Size (MB):";
            lbFileSize.ForeColor = Color.Gray;

            numericFileSize.Minimum = 40;
            numericFileSize.Maximum = 2000;
            numericFileSize.ReadOnly = true;
            numericFileSize.BackColor = Color.White;
            numericFileSize.ForeColor = Color.Gray;

            numericUpDown1.Minimum = 0;
            numericUpDown1.Maximum = 100;
            numericUpDown1.ReadOnly = true;
            numericUpDown1.BackColor = Color.White;
            numericUpDown1.ForeColor = Color.Gray;

            txtPort.Font = new Font("Microsoft Sans Serif", 36F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            txtPort.Text = "80";
            txtPort.BackColor = Color.White;
            txtPort.ForeColor = Color.Gray;
            this.ActiveControl = txtPort;
            txtPort.MaxLength = 4;

            lbMessageToUser.Text = USER_MESSAGE;
            lbMessageToUser.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            lbMessageToUser.ForeColor = Color.DarkGray;

            btnStartServer.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            btnStartServer.ForeColor = Color.DimGray;
            btnStartServer.BackColor = Color.White;


            statusStrip1.BackColor = Color.White;

            this.Text = "Pingle Server";
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            NetworkChange.NetworkAddressChanged += new
            NetworkAddressChangedEventHandler(AddressChangedCallback);

            AddressChangedCallback(null, null); //Forcing the event to be raised one time for initialization. 

            txtReport.Multiline = true;
            txtReport.ReadOnly = true;
            txtReport.BackColor = Color.White;


            txtReport.VisibleChanged += (se, ev) =>
            {
                if (txtReport.Visible)
                {
                    txtReport.SelectionStart = txtReport.TextLength;
                    txtReport.ScrollToCaret();
                }
            };

        }

        private void UpdateUINetStatus(bool netState)
        {
            if (netState)
            {
                NetworkIsAvailable = true;

                //string ipAdd = GetIPAddress().ToString();
                this.Invoke((MethodInvoker)delegate
                {

                    lbNetworkAvailablity.Text = "Network is available";
                    List<NetWorkList> netWorkLists = new List<NetWorkList>();
                    cmbIPList.Items.Clear();
                    foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        {
                            foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                {
                                    Console.WriteLine(ni.Name + " " + ip.Address.ToString());
                                    netWorkLists.Add(new NetWorkList { Name = ni.Name.ToString(), Address = ip.Address.ToString() });
                                }
                            }
                        }
                    }
                    cmbIPList.DataSource = netWorkLists;
                    cmbIPList.ValueMember = "Address";
                    cmbIPList.DisplayMember = "Description";

                    lbShowIP.Text = (string)cmbIPList.SelectedValue + ":";
                    txtPort.Left = lbShowIP.Right;
                    lbMessageToUser.Text = USER_MESSAGE;

                    if (lbNetworkAvailablity.Image != null) lbNetworkAvailablity.Image = null;

                });
            }
            else
            {
                NetworkIsAvailable = false;

                this.Invoke((MethodInvoker)delegate
                {
                    cmbIPList.Items.Clear();
                    lbNetworkAvailablity.Image = Image.FromFile(RES_IMAGE_PATH + "no_net.png");
                    lbNetworkAvailablity.Text = "Network is not available!";
                    lbMessageToUser.Text = "Please check your network connection!";
                    lbShowIP.Text = "?.?.?.?:";
                });

            }
        }

        private void frmPigleServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Console.WriteLine("Closing the socket.");
            Thread.Sleep(1000);
            terminate = true;
            if (listener.IsListening)
            {
                listener.Abort();
                listener.Close();
            }
        }

        private void AddItemToList(string item, string value)
        {
            systemInfos.Add(item, value);
        }

        void getSystemInfo()
        {
            txtReport.AppendText("Looking for system information."); txtReport.AppendText(Environment.NewLine);

            List<string> strCpuInfo = Pingle.Utility.GetSystemInfo("Win32_Processor", "Name");

            AddItemToList("Processor", strCpuInfo[0]);
            txtReport.AppendText($"Processor {strCpuInfo[0]}"); txtReport.AppendText(Environment.NewLine);

            List<string> strCacheInfos = Pingle.Utility.GetSystemInfo("Win32_CacheMemory", "MaxCacheSize");

            txtReport.AppendText("Collecting memory detail."); txtReport.AppendText(Environment.NewLine);

            int cacheSize = 0;
            string cacheLevels = "";

            foreach (string strCacheInfo in strCacheInfos)
            {
                if (!string.IsNullOrEmpty(strCacheInfo))
                {
                    cacheSize += int.Parse(strCacheInfo);
                    cacheLevels += "[" + strCacheInfo + "] ";
                }
            }

            AddItemToList("Cache Memory Size", (cacheSize == 0) ? "No cache memory!" : cacheLevels + "=" + cacheSize.ToString());
            txtReport.AppendText($"Cache Memory Size: " + ((cacheSize == 0) ? "No cache memory!" : cacheLevels + "=" + cacheSize.ToString())); txtReport.AppendText(Environment.NewLine);

            string strMemory = Pingle.Utility.GetSystemInfo("Win32_ComputerSystem", "TotalPhysicalMemory")[0];

            Int64 ramMemSize = 0;

            if (Int64.TryParse(strMemory, out ramMemSize))
            {
                ramMemSize = Int64.Parse(strMemory) / 1000000000;
                AddItemToList("RAM Memory Size", ramMemSize.ToString() + " GB");
            }
            else
            {
                AddItemToList("RAM Memory Size", "RAM info is not available.");
            }


            txtReport.AppendText("OS info."); txtReport.AppendText(Environment.NewLine);

            List<string> strOsInfos = Pingle.Utility.GetSystemInfo("Win32_OperatingSystem", "Caption");

            AddItemToList("OS", strOsInfos[0]);
            txtReport.AppendText($"OS: {strOsInfos[0]}"); txtReport.AppendText(Environment.NewLine);

            string strSysType = Pingle.Utility.GetSystemInfo("Win32_ComputerSystem", "SystemType")[0];
            AddItemToList("System Type", (string.IsNullOrEmpty(strSysType) == false) ? strSysType : "Not available.");
            txtReport.AppendText($"System Type: " + ((string.IsNullOrEmpty(strSysType) == false) ? strSysType : "Not available.")); txtReport.AppendText(Environment.NewLine);

            txtReport.AppendText("Domain name server host name."); txtReport.AppendText(Environment.NewLine);
            string strDNSHostName = Pingle.Utility.GetSystemInfo("Win32_ComputerSystem", "DNSHostName")[0];
            AddItemToList("DNS Host Name", (string.IsNullOrEmpty(strDNSHostName) == false) ? strDNSHostName : "Not available.");
            txtReport.AppendText($"DNS Host Name: " + ((string.IsNullOrEmpty(strDNSHostName) == false) ? strDNSHostName : "Not available.")); txtReport.AppendText(Environment.NewLine);


            txtReport.AppendText("Looking for antivirus."); txtReport.AppendText(Environment.NewLine);
            string strAntivirus = Pingle.Utility.AntivirusInstalled();
            AddItemToList("Antivirus Name", (string.IsNullOrEmpty(strAntivirus) == false) ? strAntivirus : "Not available.");
            txtReport.AppendText($"Antivirus Name: " + ((string.IsNullOrEmpty(strAntivirus) == false) ? strAntivirus : "Not available.")); txtReport.AppendText(Environment.NewLine);

            txtReport.AppendText("Looking for firewall."); txtReport.AppendText(Environment.NewLine);
            string strFireWall = Pingle.Utility.GetFirewallName();
            AddItemToList("Firewall Name", (string.IsNullOrEmpty(strFireWall) == false) ? strFireWall : "Not available.");
            txtReport.AppendText($"Firewall Name: " + ((string.IsNullOrEmpty(strFireWall) == false) ? strFireWall : "Not available.")); txtReport.AppendText(Environment.NewLine);

            txtReport.AppendText("Board manufacturer."); txtReport.AppendText(Environment.NewLine);
            string strManufacturer = Pingle.Utility.GetSystemInfo("Win32_ComputerSystem", "Manufacturer")[0];
            AddItemToList("Manufacturer", (string.IsNullOrEmpty(strManufacturer) == false) ? strManufacturer : "Not available.");
            txtReport.AppendText($"Manufacturer: " + ((string.IsNullOrEmpty(strManufacturer) == false) ? strManufacturer : "Not available.")); txtReport.AppendText(Environment.NewLine);

            string strModel = Pingle.Utility.GetSystemInfo("Win32_ComputerSystem", "Model")[0];
            AddItemToList("Model", (string.IsNullOrEmpty(strModel) == false) ? strModel : "Not available.");
            txtReport.AppendText($"Model: " + ((string.IsNullOrEmpty(strModel) == false) ? strModel : "Not available.")); txtReport.AppendText(Environment.NewLine);

            txtReport.AppendText("CPU info."); txtReport.AppendText(Environment.NewLine);
            string strNumberOfCores = Pingle.Utility.GetSystemInfo("Win32_Processor", "NumberOfCores")[0];
            AddItemToList("Number Of Cores", (string.IsNullOrEmpty(strNumberOfCores) == false) ? strNumberOfCores : "Not available.");
            txtReport.AppendText($"Number Of Cores: " + ((string.IsNullOrEmpty(strNumberOfCores) == false) ? strNumberOfCores : "Not available.")); txtReport.AppendText(Environment.NewLine);

            string strNumberOfLogicalProcessors = Pingle.Utility.GetSystemInfo("Win32_ComputerSystem", "NumberOfLogicalProcessors")[0];
            AddItemToList("Number of Logical Processors", (string.IsNullOrEmpty(strNumberOfLogicalProcessors) == false) ? strNumberOfLogicalProcessors : "Not available.");
            txtReport.AppendText($"Number of Logical Processors: " + ((string.IsNullOrEmpty(strNumberOfLogicalProcessors) == false) ? strNumberOfLogicalProcessors : "Not available.")); txtReport.AppendText(Environment.NewLine);

            string strNumberOfProcessors = Pingle.Utility.GetSystemInfo("Win32_ComputerSystem", "NumberOfProcessors")[0];
            AddItemToList("Number Of Processors", (string.IsNullOrEmpty(strNumberOfProcessors) == false) ? strNumberOfProcessors : "Not available.");
            txtReport.AppendText($"Number Of Processors: " + ((string.IsNullOrEmpty(strNumberOfProcessors) == false) ? strNumberOfProcessors : "Not available.")); txtReport.AppendText(Environment.NewLine);


            txtReport.AppendText("Hard Disk info."); txtReport.AppendText(Environment.NewLine);

            string strHddInfo = Pingle.Utility.HDDInfo();
            AddItemToList("HDD Info", (string.IsNullOrEmpty(strHddInfo) == false) ? strHddInfo : "Not available.");
            txtReport.AppendText($"HDD Info: " + ((string.IsNullOrEmpty(strHddInfo) == false) ? strHddInfo : "Not available.")); txtReport.AppendText(Environment.NewLine);

            txtReport.AppendText("Hard Disk read/write speed."); txtReport.AppendText(Environment.NewLine);

            long diskSpeed = 0;

            for (int i = 0; i < 10; i++)
            {
                diskSpeed += Pingle.Utility.diskSpeed();
            }

            decimal diskSpeedDeci = diskSpeed / 10;

            txtReport.AppendText("Hard Disk read/write finished."); txtReport.AppendText(Environment.NewLine);

            AddItemToList("Disk Write-Read Speed(Bps)", diskSpeedDeci.ToString());
            txtReport.AppendText($"Disk Write/Read Speed(Bps): " + diskSpeedDeci.ToString()); txtReport.AppendText(Environment.NewLine);


            txtReport.AppendText("Jobe done!."); txtReport.AppendText(Environment.NewLine);
            txtReport.AppendText("Now press \"Start Server\" button."); txtReport.AppendText(Environment.NewLine);
        }

        private void frmPigleServer_Shown(object sender, EventArgs e)
        {
            getSystemInfo();
        }


    }
}