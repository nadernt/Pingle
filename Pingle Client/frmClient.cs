using NetFwTypeLib;
using PingleClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Pingle
{
    public partial class frmClient : Form
    {
        private readonly string BASE_PATH = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private readonly string RESOURCE_PATH;
        private readonly string RES_IMAGE_PATH;
        private const string OK = "PINGLE_OK";
        private const int SPILINE_TICKNESS = 3;
        private const string OUTBOUND_AVERAGE = "Pings";
        private const string INBOUND_AVERAGE = "Pings";
        private const string BANDWIDTH_AVERAGE = "MBps";

        private int portNUMBER = 80;
        private const string ADDRESS_POSTFIX = "/index/";
        private const string DOWNLOAD_POSTFIX = "/download";
        private const string SERVER_HWINFO_POSTFIX = "/report";
        private Series seriesOutBound;
        private Series seriesInBound;
        private Series seriesBandwidth;
        private bool blATestIsGoing = false;

        private CancellationTokenSource tskPingOutBound;
        private CancellationTokenSource tskPingInBound;
        private CancellationTokenSource tskNetBandwidth;

        private WebClient clientDownload = new WebClient();

        private static bool NetworkIsAvailable = false;
        private NetResponse netReponse = new NetResponse();
        private List<PingValues> pingValuesOfInbound = new List<PingValues>();
        private List<PingValues> pingValuesOfOutbound = new List<PingValues>();
        private Dictionary<long,long> netValuesOfBandwidth = new Dictionary<long, long>();
        private string strServerHwInfo;

        private bool blTestSuccess =false;
        private const int OUTBOUND_PING_BYTE_LENGTH = 32;
        private const int INBOUND_PING_BYTE_LENGTH = 32;
        private const int OUTBOUND_PING_TIMEOUT = 12000;
        private const int INBOUND_PING_TIMEOUT = 5000;


        private Dictionary<string, string> systemInfos = new Dictionary<string, string>();
        private List<KeyValuePair<string, string>> BandwidthInfos = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// Error codes GetIpNetTable returns that we recognise
        /// </summary>
        const int ERROR_INSUFFICIENT_BUFFER = 122;
        /// <summary>
        /// MIB_IPNETROW structure returned by GetIpNetTable
        /// DO NOT MODIFY THIS STRUCTURE.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct MIB_IPNETROW
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwIndex;
            [MarshalAs(UnmanagedType.U4)]
            public int dwPhysAddrLen;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac0;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac1;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac2;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac3;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac4;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac5;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac6;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac7;
            [MarshalAs(UnmanagedType.U4)]
            public int dwAddr;
            [MarshalAs(UnmanagedType.U4)]
            public int dwType;
        }

        /// <summary>
        /// GetIpNetTable external method
        /// </summary>
        /// <param name="pIpNetTable"></param>
        /// <param name="pdwSize"></param>
        /// <param name="bOrder"></param>
        /// <returns></returns>
        [DllImport("IpHlpApi.dll")]
        [return: MarshalAs(UnmanagedType.U4)]
        static extern int GetIpNetTable(IntPtr pIpNetTable,
              [MarshalAs(UnmanagedType.U4)] ref int pdwSize, bool bOrder);

        public frmClient()
        {
            RESOURCE_PATH = BASE_PATH + Path.DirectorySeparatorChar + "resources" + Path.DirectorySeparatorChar;
            RES_IMAGE_PATH = RESOURCE_PATH + "images" + Path.DirectorySeparatorChar;
            InitializeComponent();
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

        private void init()
        {
            txtReport.VisibleChanged += (sender, e) =>
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
                lbNetworkAvailablity.Text = "Server is available: " + GetIPAddress();
                if (lbNetworkAvailablity.Image != null) lbNetworkAvailablity.Image = null;

            }
            else
            {
                NetworkIsAvailable = false;
                lbNetworkAvailablity.Image = Image.FromFile(RES_IMAGE_PATH + "no_net.png");
                lbNetworkAvailablity.Text = "Network is not available!";
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
            firewallRule.Description = "Allow Pingle Client";
            firewallRule.ApplicationName = BASE_PATH + "\\" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe";
            firewallRule.Enabled = true;
            firewallRule.InterfaceTypes = "All";
            firewallRule.Name = "Pingle Client";

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

        private void frmClient_Load(object sender, EventArgs e)
        {
            if (!FireallRuleExist())
            {
                AddFirewallRule(NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN);
                AddFirewallRule(NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT);
            }

            tsTxtIpAddress.Text = "x.x.x.x";
            tsTxtIpPort.Text = portNUMBER.ToString();

            tsBtnRunTest.Image = Image.FromFile(RES_IMAGE_PATH + "play.png");
            tsBtnStopTest.Image = Image.FromFile(RES_IMAGE_PATH + "stop.png");
            tsBtnAbout.Image = Image.FromFile(RES_IMAGE_PATH + "info.png");
            chartOutboundConnection.Series.Clear();
            chartInboundConnection.Series.Clear();
            chartNetBandWidth.Series.Clear();

            seriesOutBound = chartOutboundConnection.Series.Add(OUTBOUND_AVERAGE);
            seriesInBound = chartInboundConnection.Series.Add(INBOUND_AVERAGE);
            seriesBandwidth = chartNetBandWidth.Series.Add(BANDWIDTH_AVERAGE);

            chartOutboundConnection.Series[OUTBOUND_AVERAGE].BorderWidth = SPILINE_TICKNESS;
            chartInboundConnection.Series[INBOUND_AVERAGE].BorderWidth = SPILINE_TICKNESS;
            chartNetBandWidth.Series[BANDWIDTH_AVERAGE].BorderWidth = SPILINE_TICKNESS;

            seriesOutBound.ChartType = SeriesChartType.Spline;
            seriesInBound.ChartType = SeriesChartType.Spline;
            seriesBandwidth.ChartType = SeriesChartType.Spline;

            chartInboundConnection.Titles.Add("Inbound Connection");
            chartOutboundConnection.Titles.Add("Outbound Connection");
            chartNetBandWidth.Titles.Add("Inbound Bandwidth");

            lvResults.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvResults.Columns.Add("Query", 100);
            lvResults.Columns.Add("Result", 200);
            lvResults.View = View.Details;
            lvResults.GridLines = true;
            lvResults.FullRowSelect = true;

            txtReport.Multiline = true;
            txtReport.ReadOnly = true;
            txtReport.BackColor = Color.White;

            panelLoading.Visible = false;
            panelLoading.Location = new Point((this.Width - panelLoading.Width) / 2, (this.Height - panelLoading.Height) / 2);

            NetworkChange.NetworkAddressChanged += new
            NetworkAddressChangedEventHandler(AddressChangedCallback);

            AddressChangedCallback(null, null); //Forcing the event to be raised one time for initialization. 

            if (NetworkCheck())
                init();

        }


        private string stringPingleUrlPortBinder(string address)
        {
            if (string.IsNullOrEmpty(tsTxtIpPort.Text.Trim()))
                 tsTxtIpPort.Text = "80";
            return "http://" + address + ":" + tsTxtIpPort.Text.Trim();
        }

        public async Task<bool> LookupPingleServerAsync(string uri, TimeSpan timeOut, bool applyTimeOut)
        {

            HttpClient client = new HttpClient();

            // Call asynchronous network methods in a try/catch block to handle exceptions
            try
            {
                if (applyTimeOut)
                    client.Timeout = timeOut;

                /*HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();*/
                // Above three lines can be replaced with new helper method below
                string responseBody = await client.GetStringAsync(uri);

                //Console.WriteLine(responseBody);
                if (string.IsNullOrEmpty(responseBody))
                    return false;
                return (responseBody.IndexOf(OK) > -1) ? true : false;
            }
            catch (HttpRequestException e)
            {

                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return false;
            }
            finally
            {
                client.Dispose();
            }

        }

        public async Task<string> FetchPingleServerHWInfoAsync(string uri, TimeSpan timeOut, bool applyTimeOut)
        {

            HttpClient client = new HttpClient();

            // Call asynchronous network methods in a try/catch block to handle exceptions
            try
            {
                if (applyTimeOut)
                    client.Timeout = timeOut;

                /*HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();*/
                // Above three lines can be replaced with new helper method below
                string responseBody = await client.GetStringAsync(uri);

                //Console.WriteLine(responseBody);
                if (string.IsNullOrEmpty(responseBody))
                    return null;
                else
                    return responseBody;
            }
            catch (HttpRequestException e)
            {

                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }
            finally
            {
                client.Dispose();
            }

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

        private void AddItemToList(string item, string value)
        {
            DisplayLoadingDialog(true);
            systemInfos.Add(item, value);
            lvResults.Items.Add(new ListViewItem(new string[2] { item, value }));
        }

        private void DisplayLoadingDialog(bool show)
        {
            if (show) panelLoading.Visible = true; else panelLoading.Visible = false;
        }

        private NetResponse CheckInternetConnection(string hostNameorAddress, int dataLengthinByte, int timeoutOfPing)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;

            string data = "";

            for (int i = 0; i < dataLengthinByte; i++) data += "a";

            byte[] buffer = Encoding.ASCII.GetBytes(data);

            PingReply reply = pingSender.Send(hostNameorAddress, timeoutOfPing, buffer, options);

            NetResponse netResponse = new NetResponse();

            if (reply.Status == IPStatus.Success)
            {
                netResponse.Address = reply.Address.ToString();
                netResponse.Sucess = true;
                netResponse.RoundtripTime = reply.RoundtripTime;
                netResponse.Ttl = reply.Options.Ttl;
                netResponse.Lengh = reply.Buffer.Length;
                netResponse.DontFragment = reply.Options.DontFragment;
            }
            else
            {
                netResponse.Sucess = false;
            }
            return netResponse;
        }

        /// <summary>
        /// Get the IP and MAC addresses of all known devices on the LAN
        /// </summary>
        /// <remarks>
        /// 1) This table is not updated often - it can take some human-scale time 
        ///    to notice that a device has dropped off the network, or a new device
        ///    has connected.
        /// 2) This discards non-local devices if they are found - these are multicast
        ///    and can be discarded by IP address range.
        /// </remarks>
        /// <returns></returns>
        private static Dictionary<IPAddress, PhysicalAddress> GetAllDevicesOnLAN()
        {
            Dictionary<IPAddress, PhysicalAddress> all = new Dictionary<IPAddress, PhysicalAddress>();
            // Add this PC to the list...
            all.Add(GetIPAddress(), GetMacAddress());
            int spaceForNetTable = 0;
            // Get the space needed
            // We do that by requesting the table, but not giving any space at all.
            // The return value will tell us how much we actually need.
            GetIpNetTable(IntPtr.Zero, ref spaceForNetTable, false);
            // Allocate the space
            // We use a try-finally block to ensure release.
            IntPtr rawTable = IntPtr.Zero;
            try
            {
                rawTable = Marshal.AllocCoTaskMem(spaceForNetTable);
                // Get the actual data
                int errorCode = GetIpNetTable(rawTable, ref spaceForNetTable, false);
                if (errorCode != 0)
                {
                    // Failed for some reason - can do no more here.
                    throw new Exception(string.Format(
                      "Unable to retrieve network table. Error code {0}", errorCode));
                }
                // Get the rows count
                int rowsCount = Marshal.ReadInt32(rawTable);
                IntPtr currentBuffer = new IntPtr(rawTable.ToInt64() + Marshal.SizeOf(typeof(Int32)));
                // Convert the raw table to individual entries
                MIB_IPNETROW[] rows = new MIB_IPNETROW[rowsCount];
                for (int index = 0; index < rowsCount; index++)
                {
                    rows[index] = (MIB_IPNETROW)Marshal.PtrToStructure(new IntPtr(currentBuffer.ToInt64() +
                                                (index * Marshal.SizeOf(typeof(MIB_IPNETROW)))
                                               ),
                                                typeof(MIB_IPNETROW));
                }
                // Define the dummy entries list (we can discard these)
                PhysicalAddress virtualMAC = new PhysicalAddress(new byte[] { 0, 0, 0, 0, 0, 0 });
                PhysicalAddress broadcastMAC = new PhysicalAddress(new byte[] { 255, 255, 255, 255, 255, 255 });
                foreach (MIB_IPNETROW row in rows)
                {
                    IPAddress ip = new IPAddress(BitConverter.GetBytes(row.dwAddr));
                    byte[] rawMAC = new byte[] { row.mac0, row.mac1, row.mac2, row.mac3, row.mac4, row.mac5 };
                    PhysicalAddress pa = new PhysicalAddress(rawMAC);
                    if (!pa.Equals(virtualMAC) && !pa.Equals(broadcastMAC) && !IsMulticast(ip))
                    {
                        //Console.WriteLine("IP: {0}\t\tMAC: {1}", ip.ToString(), pa.ToString());
                        if (!all.ContainsKey(ip))
                        {
                            all.Add(ip, pa);
                        }
                    }
                }
            }
            finally
            {
                // Release the memory.
                Marshal.FreeCoTaskMem(rawTable);
            }
            return all;
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
                if (!ip.IsIPv6LinkLocal)
                {
                    return (ip);
                }
            }
            // Console.WriteLine("GGGGG" + addr.Length);
            return addr.Length > 0 ? addr[0] : null;
        }
        /// <summary>
        /// Gets the IP address of the current PC
        /// </summary>
        /// <returns></returns>
        private IPAddress PopulateMyIPAddress(ref ToolStripDropDownButton ts)
        {
            String strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            foreach (IPAddress ip in addr)
            {
                if (!ip.IsIPv6LinkLocal)
                {
                    ts.DropDownItems.Add(ip.ToString());
                    return (ip);
                }
            }
            return addr.Length > 0 ? addr[0] : null;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        /// <summary>
        /// Gets the MAC address of the current PC.
        /// </summary>
        /// <returns></returns>
        private static PhysicalAddress GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }

        /// <summary>
        /// Returns true if the specified IP address is a multicast address
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static bool IsMulticast(IPAddress ip)
        {
            bool result = true;
            if (!ip.IsIPv6Multicast)
            {
                byte highIP = ip.GetAddressBytes()[0];
                if (highIP < 224 || highIP > 239)
                {
                    result = false;
                }
            }
            return result;
        }

        private void frmClient_Resize(object sender, EventArgs e)
        {
            txtReport.Left = 0;
            txtReport.Width = this.Width;
            panelLoading.Location = new Point((this.Width - panelLoading.Width) / 2, (this.Height - panelLoading.Height) / 2);
        }

        private void tsTxtIpPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tsTxtIpPort_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(tsTxtIpPort.Text, "^[0-9]*$"))
            {
                tsTxtIpPort.Text = "80";
                tsTxtIpPort.Text = "80";
            }
        }
        private void tsTxtPingsNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tsTxtPingsNum_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(tsTxtPingsNum.Text, "^[0-9]*$"))
            {
                tsTxtPingsNum.Text = "60";
                tsTxtPigsPerMinuts.Text = "60";
            }
        }

        private void tsTxtPigsPerMinuts_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(tsTxtPigsPerMinuts.Text, "^[0-9]*$"))
            {
                tsTxtPigsPerMinuts.Text = "60";
                tsTxtPigsPerMinuts.Text = "60";
            }
        }

        private void tsTxtPigsPerMinuts_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void CancelTheTest()
        {
            if (tskPingOutBound != null)
            {
                tskPingOutBound.Cancel();
                tskPingInBound.Cancel();
                tskNetBandwidth.Cancel();
                clientDownload.CancelAsync();
            }

            blATestIsGoing = false;
            DisplayLoadingDialog(false);
        }

        private async void tsBtnRunTest_ClickAsync(object sender, EventArgs e)
        {
            if (blATestIsGoing)
                return;

            lbCancelProcess.Click += new EventHandler((sen, evn) =>
            {
                blTestSuccess = false;
                CancelTheTest();
                return;
            });

            blTestSuccess = false;

            chartOutboundConnection.Series.Clear();
            chartInboundConnection.Series.Clear();
            chartNetBandWidth.Series.Clear();

            seriesOutBound = chartOutboundConnection.Series.Add(OUTBOUND_AVERAGE);
            seriesInBound = chartInboundConnection.Series.Add(INBOUND_AVERAGE);
            seriesBandwidth = chartNetBandWidth.Series.Add(BANDWIDTH_AVERAGE);

            chartOutboundConnection.Series[OUTBOUND_AVERAGE].BorderWidth = SPILINE_TICKNESS;
            chartInboundConnection.Series[INBOUND_AVERAGE].BorderWidth = SPILINE_TICKNESS;
            chartNetBandWidth.Series[BANDWIDTH_AVERAGE].BorderWidth = SPILINE_TICKNESS;

            seriesOutBound.ChartType = SeriesChartType.Spline;
            seriesInBound.ChartType = SeriesChartType.Spline;
            seriesBandwidth.ChartType = SeriesChartType.Spline;

            EnableDisableZooming(false);

            if (NetworkIsAvailable)
            {
                UpdateUINetStatus(true);

                int pingPerMinuts = 0, pingNumbers = 0;

                bool blSuccessNum = Int32.TryParse(tsTxtPigsPerMinuts.Text.Trim(), out pingPerMinuts);

                if (pingPerMinuts == 0 || !blSuccessNum)
                {
                    MessageBox.Show("Enter correct number for ping per minuts!");
                    return;
                }
                tsTxtPigsPerMinuts.Text = pingPerMinuts.ToString();

                blSuccessNum = Int32.TryParse(tsTxtPingsNum.Text.Trim(), out pingNumbers);

                if (pingNumbers == 0 || !blSuccessNum)
                {
                    MessageBox.Show("Enter correct number for ping number!");
                    return;
                }
                tsTxtPingsNum.Text = pingNumbers.ToString();

                if (string.IsNullOrEmpty(tsTxtIpAddress.Text) || string.IsNullOrWhiteSpace(tsTxtIpAddress.Text))
                {
                    MessageBox.Show("The IP address is not entered!");
                    return;
                }

                if (!Utility.CheckIpValidation(tsTxtIpAddress.Text))
                {
                    MessageBox.Show("The entered IP address is not valid or correct format!");
                    return;
                }

                txtReport.AppendText("Preparing the lookup process."); txtReport.AppendText(Environment.NewLine);

                DisplayLoadingDialog(true);

                txtReport.AppendText("Check the provided server ip."); txtReport.AppendText(Environment.NewLine);

                var pingServer = await Task.Run(async () =>
                {
                    var hitServer = false;
                    for (int i = 0; i < 4; i++)
                    {
                        try
                        {
                            hitServer = await LookupPingleServerAsync(stringPingleUrlPortBinder(tsTxtIpAddress.Text.Trim()), TimeSpan.FromMilliseconds(10000), false);
                            if (hitServer)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    txtReport.AppendText("Found the server."); txtReport.AppendText(Environment.NewLine);
                                });
                                return true;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                txtReport.AppendText("Did not find the server!"); txtReport.AppendText(Environment.NewLine);
                            });
                            return false;
                        }
                    }
                    return hitServer;


                });

                var getServerHWInfo = await Task.Run(async () =>
                {
                    try
                    {
                        strServerHwInfo = await FetchPingleServerHWInfoAsync(stringPingleUrlPortBinder(tsTxtIpAddress.Text.Trim()) + SERVER_HWINFO_POSTFIX, TimeSpan.FromMilliseconds(10000), false);
                        if (!string.IsNullOrEmpty(strServerHwInfo.ToString().Trim()))
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                txtReport.AppendText("Get server hardware specification."); txtReport.AppendText(Environment.NewLine);
                            });
                            
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            txtReport.AppendText("Did not find the server!"); txtReport.AppendText(Environment.NewLine);
                        });
                        return false;
                    }

                    return true;

                });


                // Test is starting.
                blATestIsGoing = true;

                if (!pingServer || !getServerHWInfo)
                {
                    CancelTheTest();
                    txtReport.AppendText("Process failed!"); txtReport.AppendText(Environment.NewLine);
                    MessageBox.Show("The provided IP <" + tsTxtIpAddress.Text + "> did not respond as expected.\nPlease check the Server is running on another machine and the server ip is valid.");
                    return;
                }

                systemInfos = new Dictionary<string, string>();
                BandwidthInfos = new List<KeyValuePair<string, string>>();

                pingValuesOfOutbound = new List<PingValues>();
                pingValuesOfInbound = new List<PingValues>();

                blTestSuccess = true;

                lvResults.Items.Clear();
                lvResults.Update(); // In case there is databinding
                lvResults.Refresh(); // Redraw items

                systemInfos.Add("Time of Benchmark", DateTime.Now.ToLocalTime().ToString("M-d-yyyy hh:mm:ss tt"));

                txtReport.AppendText("Checking outbound connection is available."); txtReport.AppendText(Environment.NewLine);

                var pingGoogle = await Task.Run(() =>
                {
                    netReponse = new NetResponse();
                    for (int i = 0; i < 4; i++)
                        netReponse = CheckInternetConnection("www.google.com", OUTBOUND_PING_BYTE_LENGTH, OUTBOUND_PING_TIMEOUT);
                    return netReponse.Sucess;
                });

                txtReport.AppendText(pingGoogle ? "Outbound check success.\n" : "Outbound check failed."); txtReport.AppendText(Environment.NewLine);

                AddItemToList("Internet Connection", pingGoogle ? "Yes" : "No");

                txtReport.AppendText("Collecting the clients number on the provided network."); txtReport.AppendText(Environment.NewLine);

                Dictionary<IPAddress, PhysicalAddress> alls = GetAllDevicesOnLAN();

                AddItemToList("Number of Clients on Network", alls.Count.ToString());
                txtReport.AppendText(alls.Count.ToString() + " clients found."); txtReport.AppendText(Environment.NewLine);


                foreach (KeyValuePair<IPAddress, PhysicalAddress> kvp in alls)
                {
                    Console.WriteLine("IP : {0}\n MAC {1}", kvp.Key, kvp.Value);
                    txtReport.AppendText(string.Format("IP : {0}\n MAC {1}", kvp.Key, kvp.Value)); txtReport.AppendText(Environment.NewLine);
                }

                txtReport.AppendText("Looking for system information."); txtReport.AppendText(Environment.NewLine);
                List<string> strCpuInfo = Utility.GetSystemInfo("Win32_Processor", "Name");
                AddItemToList("Processor", strCpuInfo[0]);

                List<string> strCacheInfos = Utility.GetSystemInfo("Win32_CacheMemory", "MaxCacheSize");

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

                string strMemory = Utility.GetSystemInfo("Win32_ComputerSystem", "TotalPhysicalMemory")[0];

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

                AddItemToList("OS", Utility.GetSystemInfo("Win32_OperatingSystem", "Caption")[0]);

                string strSysType = Utility.GetSystemInfo("Win32_ComputerSystem", "SystemType")[0];
                AddItemToList("System Type", (string.IsNullOrEmpty(strSysType) == false) ? strSysType : "Not available.");

                txtReport.AppendText("Domain name server host name."); txtReport.AppendText(Environment.NewLine);
                string strDNSHostName = Utility.GetSystemInfo("Win32_ComputerSystem", "DNSHostName")[0];
                AddItemToList("DNS Host Name", (string.IsNullOrEmpty(strDNSHostName) == false) ? strDNSHostName : "Not available.");

                txtReport.AppendText("Looking for antivirus."); txtReport.AppendText(Environment.NewLine);
                string strAntivirus = Pingle.Utility.AntivirusInstalled();
                AddItemToList("Antivirus Name", (string.IsNullOrEmpty(strAntivirus) == false) ? strAntivirus : "Not available.");

                txtReport.AppendText("Looking for firewall."); txtReport.AppendText(Environment.NewLine);
                string strFireWall = Pingle.Utility.GetFirewallName();
                AddItemToList("Firewall Name", (string.IsNullOrEmpty(strFireWall) == false) ? strFireWall : "Not available.");

                txtReport.AppendText("Board manufacturer."); txtReport.AppendText(Environment.NewLine);
                string strManufacturer = Utility.GetSystemInfo("Win32_ComputerSystem", "Manufacturer")[0];
                AddItemToList("Manufacturer", (string.IsNullOrEmpty(strManufacturer) == false) ? strManufacturer : "Not available.");

                string strModel = Utility.GetSystemInfo("Win32_ComputerSystem", "Model")[0];
                AddItemToList("Model", (string.IsNullOrEmpty(strModel) == false) ? strModel : "Not available.");

                txtReport.AppendText("CPU info."); txtReport.AppendText(Environment.NewLine);
                string strNumberOfCores = Utility.GetSystemInfo("Win32_Processor", "NumberOfCores")[0];
                AddItemToList("Number Of Cores", (string.IsNullOrEmpty(strNumberOfCores) == false) ? strNumberOfCores : "Not available.");

                string strNumberOfLogicalProcessors = Utility.GetSystemInfo("Win32_ComputerSystem", "NumberOfLogicalProcessors")[0];
                AddItemToList("Number of Logical Processors", (string.IsNullOrEmpty(strNumberOfLogicalProcessors) == false) ? strNumberOfLogicalProcessors : "Not available.");

                string strNumberOfProcessors = Utility.GetSystemInfo("Win32_ComputerSystem", "NumberOfProcessors")[0];
                AddItemToList("Number Of Processors", (string.IsNullOrEmpty(strNumberOfProcessors) == false) ? strNumberOfProcessors : "Not available.");

                txtReport.AppendText("Hard Disk info."); txtReport.AppendText(Environment.NewLine);
                string strHddInfo = Utility.HDDInfo();
                AddItemToList("HDD Info", (string.IsNullOrEmpty(strHddInfo) == false) ? strHddInfo : "Not available.");

                txtReport.AppendText("Hard Disk read-write speed."); txtReport.AppendText(Environment.NewLine);

                long diskSpeed = 0;

                for (int i = 0; i < 10; i++)
                {
                    diskSpeed += Utility.diskSpeed();
                }

                decimal diskSpeedDeci = diskSpeed / 10;

                AddItemToList("Disk Write-Read Speed(Bps)", diskSpeedDeci.ToString());

                txtReport.AppendText("Hard Disk read/write finished."); txtReport.AppendText(Environment.NewLine);

                lvResults.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

                txtReport.AppendText("Try to ping inbound server."); txtReport.AppendText(Environment.NewLine);

                tskPingOutBound = new CancellationTokenSource();
                tskPingInBound = new CancellationTokenSource();
                tskNetBandwidth = new CancellationTokenSource();

                CancellationToken cancekTokOutBound = tskPingOutBound.Token;
                CancellationToken cancekTokInBound = tskPingInBound.Token;
                CancellationToken cancekTokNetBandWidth = tskNetBandwidth.Token;

                int pingCount = 0;
                try
                {
                    await Task.Factory.StartNew(() =>
                    {

                        int timeCounter = 0;

                        while (true)
                        {
                            pingCount++;
                            Thread.Sleep(60000 / pingPerMinuts);
                            try
                            {
                                netReponse = new NetResponse();
                                netReponse = CheckInternetConnection(tsTxtIpAddress.Text, INBOUND_PING_BYTE_LENGTH, INBOUND_PING_TIMEOUT);

                                Console.WriteLine("Success:{0}, TTL:{1} Time:{2}", netReponse.Sucess, netReponse.Ttl, netReponse.RoundtripTime);

                                pingValuesOfInbound.Add(new PingValues(netReponse.Address, netReponse.Sucess,
                                  netReponse.RoundtripTime, netReponse.Ttl, netReponse.DontFragment, netReponse.Lengh, DateTime.Now.Ticks));

                                this.Invoke((MethodInvoker)delegate
                                 {
                                     txtReport.AppendText(string.Format("Success:{0}, TTL:{1} Time:{2}", netReponse.Sucess, netReponse.Ttl, netReponse.RoundtripTime)); txtReport.AppendText(Environment.NewLine);

                                     if (!cancekTokInBound.IsCancellationRequested)
                                         seriesInBound.Points.AddXY((timeCounter += 10 /* 10000 / 1000 */), netReponse.RoundtripTime);
                                 });

                                if (cancekTokInBound.IsCancellationRequested || (pingNumbers <= pingCount)) break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                pingValuesOfInbound.Add(new PingValues(netReponse.Address, false,
                                  0, 0, false, 0, DateTime.Now.Ticks));

                                this.Invoke((MethodInvoker)delegate
                                {
                                    if (!cancekTokInBound.IsCancellationRequested)
                                        seriesInBound.Points.AddXY((timeCounter += 10), 0);
                                });
                            }
                            finally
                            {
                              

                            }
                        }
                    }, cancekTokInBound);
                }
                catch (TaskCanceledException tskCancelEx)
                {
                    txtReport.AppendText("Job 1 canceled."); txtReport.AppendText(Environment.NewLine);
                }



                txtReport.AppendText("Try to check bandwidth."); txtReport.AppendText(Environment.NewLine);

                
                try
                {
                    await Task.Factory.StartNew(() =>
                    {
                        int downloadChunks = 1;
                        long LastUpdate=0L;
                        long BytesDownloadedInTimeSpan = 0;
                        long LastBytesReceived = 0;
                        Stopwatch stopWatch = new Stopwatch();
                        long TotalBytes = 0;

                        Uri uri = new Uri(stringPingleUrlPortBinder(tsTxtIpAddress.Text.Trim()) + DOWNLOAD_POSTFIX);

                        clientDownload = new WebClient();

                        clientDownload.DownloadFileCompleted += new AsyncCompletedEventHandler((sender_complete,event_complete)=> {
                            long time = stopWatch.ElapsedMilliseconds;
                            if (time == 0)
                                time = 1;
                            long totalBps = TotalBytes / time;

                            stopWatch.Stop();
                            Console.WriteLine($"Finished {totalBps} {(time / 1000)}s");
                        });
                        
                        clientDownload.DownloadProgressChanged += new DownloadProgressChangedEventHandler((sender_progress, event_progress) => {
                            try
                            {
                                if (event_progress.ProgressPercentage != 100)
                                {

                                    TotalBytes = event_progress.TotalBytesToReceive;


                                    if (LastUpdate >= 1000)
                                    {
                                       
                                       
                                        long ElapsedMiliSeconds = LastUpdate + stopWatch.ElapsedMilliseconds ;

                                        Console.WriteLine("{0}ms", ElapsedMiliSeconds);

                                      //  if (BytesDownloadedInTimeSpan == 0)
                                       //     BytesDownloadedInTimeSpan = event_progress.BytesReceived;

                                        float MBps = ((float)BytesDownloadedInTimeSpan / ElapsedMiliSeconds)/1000.0f;

                                        LastUpdate = 0;
                                        BytesDownloadedInTimeSpan = 0;

                                       
                                        if (!double.IsInfinity(MBps))
                                        {
                                            Console.WriteLine($"{downloadChunks}: {MBps} Mbps = {ElapsedMiliSeconds}ms");

                                            this.Invoke((MethodInvoker)delegate
                                            {
                                                txtReport.AppendText(MBps + " = " + ElapsedMiliSeconds); txtReport.AppendText(Environment.NewLine);

                                                if (!cancekTokNetBandWidth.IsCancellationRequested) {
                                                    seriesBandwidth.Points.AddXY(downloadChunks++, MBps);
                                                    BandwidthInfos.Add(new KeyValuePair<string, string>(ElapsedMiliSeconds.ToString(), MBps.ToString()));
                                                }
                                            });
                                        }
                                       
                                    }
                                    else
                                    {
                                        BytesDownloadedInTimeSpan += event_progress.BytesReceived - LastBytesReceived;
                                        LastUpdate += stopWatch.ElapsedMilliseconds;
                                        stopWatch.Restart();
                                    }
                                    LastBytesReceived = event_progress.BytesReceived;
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                this.Invoke((MethodInvoker)delegate
                                    {
                                        txtReport.AppendText("Failed: " + ex.Message); txtReport.AppendText(Environment.NewLine);

                                    });
                                
                            }
                            finally
                            {
                            }
                        
                        });

                        LastUpdate = 0;
                        TotalBytes = 0;
                        stopWatch = new Stopwatch();
                      
                        try
                        {
                            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".txt";

                            var t = clientDownload.DownloadFileTaskAsync(uri, fileName);

                            stopWatch.Start();

                            t.Wait();
                        }
                        catch (AggregateException aex)
                        {
                            Console.WriteLine(aex.Message);
                        }

                    }, cancekTokNetBandWidth);
                }
                catch (TaskCanceledException tskCancelEx)
                {
                    txtReport.AppendText("Job 2 canceled."); txtReport.AppendText(Environment.NewLine);
                }
               


                if (pingGoogle)
                {
                    try
                    {
                        txtReport.AppendText("Try to ping outbound server (ping www.google.com)."); txtReport.AppendText(Environment.NewLine);

                        pingCount = 0;

                        await Task.Factory.StartNew(() =>
                        {

                            int timeCounter = 0;
                            while (true)
                            {

                                pingCount++;
                                Thread.Sleep(60000 / pingPerMinuts);
                                try
                                {
                                    netReponse = new NetResponse();
                                    netReponse = CheckInternetConnection("www.google.com", OUTBOUND_PING_BYTE_LENGTH, OUTBOUND_PING_TIMEOUT);

                                    Console.WriteLine("Success:{0}, TTL:{1} Time:{2}", netReponse.Sucess, netReponse.Ttl, netReponse.RoundtripTime);
                                    pingValuesOfOutbound.Add(new PingValues(netReponse.Address, netReponse.Sucess,
                                    netReponse.RoundtripTime, netReponse.Ttl, netReponse.DontFragment, netReponse.Lengh, DateTime.Now.Ticks));

                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        txtReport.AppendText(string.Format("Success:{0}, TTL:{1} Time:{2}", netReponse.Sucess, netReponse.Ttl, netReponse.RoundtripTime)); txtReport.AppendText(Environment.NewLine);
                                        if (!cancekTokOutBound.IsCancellationRequested)
                                            seriesOutBound.Points.AddXY((timeCounter += 10), netReponse.RoundtripTime);
                                    });

                                    if (cancekTokOutBound.IsCancellationRequested || (pingNumbers <= pingCount)) break;
                                }
                                catch (Exception ex)
                                {
                                    pingValuesOfOutbound.Add(new PingValues(netReponse.Address, false,
                                        0, 0, false, 0, DateTime.Now.Ticks));
                                    Console.WriteLine(ex.Message);
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        if (!cancekTokOutBound.IsCancellationRequested)
                                            seriesOutBound.Points.AddXY((timeCounter += 10), 0);
                                    });
                                }
                            }

                        }, cancekTokOutBound);

                    }
                    catch (TaskCanceledException tskCancelEx)
                    {
                        txtReport.AppendText("Job 3 canceled."); txtReport.AppendText(Environment.NewLine);
                    }
                }

                txtReport.AppendText("Process completed."); txtReport.AppendText(Environment.NewLine);
                blATestIsGoing = false;
                DisplayLoadingDialog(false);
                EnableDisableZooming(true);



            }
        }

        private void EnableDisableZooming(bool showZoom)
        {
            ChartArea crtInbound = chartInboundConnection.ChartAreas[0];  
            ChartArea crtOutbound = chartOutboundConnection.ChartAreas[0];
            ChartArea crtNetBandWidth = chartNetBandWidth.ChartAreas[0];


            if (showZoom)
            {
                crtInbound.AxisX.ScaleView.Zoomable = true;
                crtInbound.CursorX.AutoScroll = true;
                crtInbound.CursorX.IsUserSelectionEnabled = true;

                crtOutbound.AxisX.ScaleView.Zoomable = true;
                crtOutbound.CursorX.AutoScroll = true;
                crtOutbound.CursorX.IsUserSelectionEnabled = true;

                crtNetBandWidth.AxisX.ScaleView.Zoomable = true;
                crtNetBandWidth.CursorX.AutoScroll = true;
                crtNetBandWidth.CursorX.IsUserSelectionEnabled = true;
            }
            else
            {
                crtInbound.AxisX.ScaleView.Zoomable = false;
                crtInbound.CursorX.AutoScroll = false;
                crtInbound.CursorX.IsUserSelectionEnabled = false;

                crtOutbound.AxisX.ScaleView.Zoomable = false;
                crtOutbound.CursorX.AutoScroll = false;
                crtOutbound.CursorX.IsUserSelectionEnabled = false;

                crtNetBandWidth.AxisX.ScaleView.Zoomable = false;
                crtNetBandWidth.CursorX.AutoScroll = false;
                crtNetBandWidth.CursorX.IsUserSelectionEnabled = false;
            }
        }

        private void frmClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            CancelTheTest();
        }

        private void tsBtnStopTest_Click(object sender, EventArgs e)
        {
            blTestSuccess = false;
            CancelTheTest();
        }
        private static string removeBrackets(string jsdonString)
        {
            return jsdonString.TrimStart('[').TrimEnd(']');
        }

        private void WriteCSVFile(string filename,string filepath,string filecontent)
        {
            filename = filepath + "\\" + filename + " " + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".csv";

            StreamWriter sw = new StreamWriter(filename,false, Encoding.ASCII);
            sw.Write(filecontent);
            sw.Close();
        }

        private void tsBtnSendReport_Click(object sender, EventArgs e)
        {
            if (blATestIsGoing)
                return;

            if (blTestSuccess)
            {
                FolderBrowserDialog folderDlg = new FolderBrowserDialog();
                folderDlg.ShowNewFolderButton = true;

                DialogResult result = folderDlg.ShowDialog();

                if (result == DialogResult.OK)
                {

                    string bandwidthInfo = $"Elapsed Ms,Mbps{Environment.NewLine}";
                    if (BandwidthInfos.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> bandwidhInfo in BandwidthInfos)
                        {

                            bandwidthInfo += $"{bandwidhInfo.Key},{bandwidhInfo.Value}{Environment.NewLine}";
                        }
                    }
                    WriteCSVFile("Band Width", folderDlg.SelectedPath, bandwidthInfo);


                    string outbound = "Address,Sucess,Roundtrip Time,Ttl,Dont Fragment,Lengh,Time" + Environment.NewLine;

                    if (pingValuesOfOutbound.Count > 0)
                    {
                        foreach (PingValues pingValues in pingValuesOfOutbound)
                        {
                            outbound += $"{pingValues.Address},{pingValues.Sucess},{pingValues.RoundtripTime}," +
                                        $"{pingValues.Ttl},{pingValues.DontFragment},{pingValues.Lengh}," +
                                        $"{new DateTime(pingValues.Time).ToString("yyyy-MM-dd HH:mm:ss.fff")}{Environment.NewLine}";
                        }
                    }
                    WriteCSVFile("Outbound Connection", folderDlg.SelectedPath, outbound);


                    string inbound = "Address,Sucess,Roundtrip Time,Ttl,Dont Fragment,Lengh,Time" + Environment.NewLine;

                    if (pingValuesOfInbound.Count > 0)
                    {
                        foreach (PingValues pingValues in pingValuesOfInbound)
                        {
                            inbound += $"{pingValues.Address},{pingValues.Sucess},{pingValues.RoundtripTime}," +
                                        $"{pingValues.Ttl},{pingValues.DontFragment},{pingValues.Lengh}," +
                                        $"{new DateTime(pingValues.Time).ToString("yyyy-MM-dd HH:mm:ss.fff")}{Environment.NewLine}";
                        }
                    }

                    WriteCSVFile("Inbound Connection", folderDlg.SelectedPath, inbound);


                    string clientSystemInfo = "";

                    if (systemInfos.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> systemInfo in systemInfos)
                        {

                            clientSystemInfo += $"{systemInfo.Key},{systemInfo.Value}{Environment.NewLine}";
                        }
                    }

                    WriteCSVFile("Client System Resources", folderDlg.SelectedPath, clientSystemInfo);

                    strServerHwInfo = strServerHwInfo.Replace("\":\"", ",");
                    strServerHwInfo = strServerHwInfo.Replace("\",\"", Environment.NewLine);
                    strServerHwInfo = strServerHwInfo.Replace("\"}","");
                    strServerHwInfo = strServerHwInfo.Replace("{\"", "");
                    WriteCSVFile("Server System Resources", folderDlg.SelectedPath, removeBrackets(strServerHwInfo));
                    MessageBox.Show("The report files saved in the path.","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
          }

        private void tsBtnAbout_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Pingle v1.0.\n\nThis software is free for use and modification by your own responsibility.\nCredit graphic resources:\nwww.flaticon.com\n\nNader Naderi", "About Pingle", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
