using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPTester
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            txtBoxHost.Text = TcpOps.TcpHost;
            txtBoxPort.Text = TcpOps.TcpPort.ToString();
            TcpOps.StatusMessage += TcpOpsOnStatusMessage;
            toolStripStatusLabel1.Text = "";
        }

        private void TcpOpsOnStatusMessage(string obj)
        {
            Invoke(new Action<string>(s => toolStripStatusLabel1.Text = s), obj);
        }

        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            TcpOps.TcpHost = txtBoxHost.Text;
            TcpOps.TcpPort = Convert.ToInt32(txtBoxPort.Text);

            if (TcpOps.Connect())
            {
                var response = TcpOps.Message(txtBoxMsg.Text);
                txtBoxResp.Text = response;
            }
        }
    }
}
