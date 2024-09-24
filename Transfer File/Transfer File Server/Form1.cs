using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Transfer_File_Server
{
    
    public partial class Form1 : System.Windows.Forms.Form
    {
        private ArrayList alSockets;

        public Form1()
        {
            InitializeComponent();
            Form1_Load();
        }

        private void Form1_Load()
        {
            IPHostEntry IPHost = Dns.GetHostByName(Dns.GetHostName());
            lblStatus.Text = "My IP address is " + IPHost.AddressList[0].ToString();
            alSockets = new ArrayList();
            Thread thdListener = new Thread(new ThreadStart(listenerThread));
            thdListener.Start();
        }

        private void lbConnections_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

        public void listenerThread()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 8080);
            tcpListener.Start();
            while (true)
            {
                Socket handlerSocket = tcpListener.AcceptSocket();
                if (handlerSocket.Connected)
                {
                    UpdateConnectionList(handlerSocket.RemoteEndPoint.ToString() + " connected.");
                    lock (this)
                    {
                        alSockets.Add(handlerSocket);
                    }
                    ThreadStart thdstHandler = new ThreadStart(handlerThread);
                    Thread thdHandler = new Thread(thdstHandler);
                    thdHandler.Start();
                }
            }
        }

        public void handlerThread()
        {
            Socket handlerSocket = (Socket)alSockets[alSockets.Count - 1];
            NetworkStream networkStream = new NetworkStream(handlerSocket);
            int thisRead = 0;
            int blockSize = 1024;
            Byte[] dataByte = new Byte[blockSize];

            lock (this)
            {
                Stream fileStream = File.OpenWrite("C:\\Users\\tiowe\\Documents\\upload.txt");
                while (true)
                {
                    thisRead = networkStream.Read(dataByte, 0, blockSize);
                    fileStream.Write(dataByte, 0, thisRead);
                    if (thisRead == 0) break;
                }
                fileStream.Close();
            }

            UpdateConnectionList("File Written");
            handlerSocket = null;
        }


        private void UpdateConnectionList(string message)
        {
            if (lbConnections.InvokeRequired)
            {
                lbConnections.Invoke(new Action<string>(UpdateConnectionList), message);
            }
            else
            {
                lbConnections.Items.Add(message);
            }
        }
    }

}
