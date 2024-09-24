using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Transfer_File
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender,System.EventArgs e)
        {
            openFileDialog.ShowDialog();
            tbFilename.Text = openFileDialog.FileName;
        }



        private void btnSend_Click(object sender, System.EventArgs e)
        {
            Stream fileStream = File.OpenRead(tbFilename.Text);
            // Alocate memory space for the file
            byte[] fileBuffer = new byte[fileStream.Length];
            fileStream.Read(fileBuffer, 0, (int)fileStream.Length);
            // Open a TCP/IP Connection and send the data
            TcpClient clientSocket = new TcpClient(tbServer.Text, 8080);
            NetworkStream networkStream = clientSocket.GetStream();
            networkStream.Write(fileBuffer, 0, fileBuffer.GetLength(0));
            networkStream.Close();
        }

        private void tbServer_TextChanged(object sender, EventArgs e)
        {

        }


        private void tbFilename_TextChanged(object sender, EventArgs e)
        {

        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
