using Be.Windows.Forms;
using PFC_tool;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace RegTool
{
    public partial class Form1 : Form
    {
        internal const uint WM_DEVICECHANGE = 0x0219;

        SMBus smbus = new SMBus();
        SBS sbs = new SBS("SBS.xml");
        string iniFile;
        bool log_control = false; //timer中控制是否要log的flag
        StreamWriter log_sw;

        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        public static extern uint GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]
        public static extern uint GetPrivateProfileInt(string section, string key, int def, string filePath);

        public Form1()
        {
            InitializeComponent();

            FileInfo fi = new FileInfo("RegTool.ini");
            Console.WriteLine(fi.FullName);
            iniFile = fi.FullName;

            foreach (BasicSBS s in sbs.SBSlist)
            {
                dataGridView1.Rows.Add(s.cmd, s.name, s.RWbit, s.value, " ");
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].ToolTipText = s.Description;
            }
            tabControl1.SelectedIndex = 0;

            UInt16 ver = smbus.GetVersion();
            if (ver != 0)
                label_connect.Text = String.Format("HID-SMBus Device Version : {0}.{1:X2}", ver >> 8, ver & 0xff);
            else
            {
                label_connect.Text = "No device connecting";
                MessageBox.Show("Please check connect");
            }

            hexBox1.ByteProvider = new DynamicByteProvider(new byte[256]);

            tabControl1.TabPages.Remove(tabPage3);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DEVICECHANGE)
            {
                UInt16 ver = smbus.GetVersion();
                label_connect.Text = String.Format("Bridge Version : {0}.{1:X2}", ver >> 8, ver & 0xff);
            }
            base.WndProc(ref m);
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                int cmd = Convert.ToByte(row.Cells[0].Value.ToString(), 16);
                ushort val;
                if (smbus.ReadWord(cmd, out val))
                {
                    row.Cells[3].Value = val.ToString();
                    row.Cells[4].Value = "0x" + val.ToString("X4");
                }
                else
                    row.Cells[3].Value = row.Cells[4].Value = " ";
            }
        }

        private void btn_ReadAll_Click(object sender, EventArgs e)
        {
            for (int cmd = 0; cmd < 256; cmd++)
            {
                byte[] pVal = new byte[1];
                if (SMBus.UsbEedataRead(0, (byte)cmd, pVal))
                    hexBox1.ByteProvider.WriteByte(cmd, pVal[0]);
                else
                    break;
            }
            hexBox1.Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string log_string_value = DateTime.Now.ToString("HH:mm:ss.fff");
            int i = 1;
            StringBuilder sb = new StringBuilder();
            while (GetPrivateProfileString("Scan", i.ToString(), "", sb, sb.Capacity, iniFile) > 0)
            {
                string s = sb.ToString();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                    if (row.Cells[0].Value.ToString() == s)
                    {
                        int cmd = Convert.ToByte(s, 16);
                        byte[] pVal = new byte[1];
                        if (smbus.ReadByte(cmd, pVal))
                        {
                            row.Cells[4].Value = "0x" + pVal[0].ToString("X2");
                            log_string_value += "," + pVal[0];
                        }
                        else
                        {
                            row.Cells[4].Value = " ";
                            log_string_value += ", ";
                        }
                        break;
                    }
                i++;
            }
            if (log_control)
            {
                log_sw.WriteLine(log_string_value);
                log_sw.Flush();
            }
        }

        private void Checkbox_Keep_Scanning_CheckedChanged(object sender, EventArgs e)
        {
            if (Checkbox_Keep_Scanning.Checked)
                timer1.Interval = (int)GetPrivateProfileInt("Scan", "ms", 500, iniFile);
            timer1.Enabled = Checkbox_Keep_Scanning.Checked;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btn_Refresh.PerformClick();
        }

        private void btn_Start_log_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "Scan.csv";
            dlg.Filter = "Scan file (*.csv)|*.csv";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string log_string_name = "Time";
                int i = 1;
                StringBuilder sb = new StringBuilder();
                while (GetPrivateProfileString("Scan", i.ToString(), "", sb, sb.Capacity, iniFile) > 0)
                {
                    string s = sb.ToString();
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                        if (row.Cells[0].Value.ToString() == s)
                        {
                            log_string_name += ',' + row.Cells[1].Value.ToString() + '(' + s + ')';
                            break;
                        }
                    i++;
                }
                log_sw = new StreamWriter(dlg.FileName, false, Encoding.Default);
                log_sw.WriteLine(log_string_name);

                //=== start logging ===//
                log_control = true;
                timer1.Enabled = true;
                btn_Start_log.Enabled = false;
                btn_Refresh.Enabled = false;
                btn_Stop_Logging.Enabled = true;
                Checkbox_Keep_Scanning.Checked = true;
                Checkbox_Keep_Scanning.Enabled = false;
            }
        }

        private void btn_Stop_Logging_Click(object sender, EventArgs e)
        {
            log_sw.Close();
            log_control = false;
            btn_Start_log.Enabled = true;
            btn_Refresh.Enabled = true;
            btn_Stop_Logging.Enabled = false;
            Checkbox_Keep_Scanning.Enabled = true;
        }
    }
}
