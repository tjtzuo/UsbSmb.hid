using PFC_tool;
using SMBTool.Properties;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace SMBTool
{
    public partial class Form1 : Form
    {
        SMBus smbus = new SMBus();

        public Form1()
        {
            InitializeComponent();

            ushort version = smbus.GetVersion();
            label1.Text = string.Format("USB-SMbus Module Version : {0}.{1}", version / 256, version % 256);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte addr = Convert.ToByte(tbAddr.Text, 16);

            foreach (string line in textBox1.Lines)
            {
                string[] packets = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (packets.Length > 0)
                {
                    List<byte> datas = new List<byte>();
                    byte cmd = Convert.ToByte(packets[0], 16);

                    for (int i = 1; i < packets.Length; i++)
                        datas.Add(Convert.ToByte(packets[i], 16));

                    if (SMBus.UsbSmbWrite(0, addr, cmd, 0, (byte)datas.Count, datas.ToArray()))
                        label1.Text = "Write Success";
                    else
                        label1.Text = "Write Failed";
                }

                Thread.Sleep(Convert.ToInt32(tbDelay.Text));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte addr = Convert.ToByte(tbAddr.Text, 16),
                 cmd = Convert.ToByte(textBox2.Text, 16),
                 len = Convert.ToByte(tbCount.Text);

            byte[] datas = new byte[len];

            textBox3.Text = "";

            if (SMBus.UsbSmbRead(0, addr, cmd, 0, len, datas))
            {
                foreach (byte val in datas)
                    textBox3.Text += val.ToString("X2") + ' ';

                label1.Text = "Read Success";
            }
            else
            {
                label1.Text = "Read Failed";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
        }

    }
}
