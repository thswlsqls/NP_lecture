using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace AsyncSimpleGUIClient
{
    public partial class Form2 : Form
    {
        Socket mainSock;
        IPAddress thisAddress;
        public static string[] ConListToken;
        public static int groupnum = 1000;

        delegate void AppendTextDelegate(Control ctrl, string s);
        AppendTextDelegate _textAppender;

        //delegate void AddItemDelegate(Control ctrl, string s);
        //AddItemDelegate _ItemAdder;

        string nameID; 
        public Form2()
        {
            InitializeComponent();
            mainSock = new Socket(AddressFamily.InterNetwork,
               SocketType.Stream, ProtocolType.Tcp);

            _textAppender = new AppendTextDelegate(AppendText);

            if (mainSock.Connected)
            {
                //MsgBoxHelper.Error("이미 연결되어 있습니다!");
                return;
            }

            if (thisAddress == null)
            {
                thisAddress = IPAddress.Loopback;
            }
            IPEndPoint serverEP = new IPEndPoint(thisAddress, ChatClient.port); //server
            try
            {
                mainSock.Connect(thisAddress, ChatClient.port);
            }
            catch // (Exception ex)
            {
                // MsgBoxHelper.Error("연결에 실패했습니다!\n오류 내용: {0}", MessageBoxButtons.OK, ex.Message);
                return;
            }
            // 서버로 ID  전송
            //nameID = txtID.Text.Trim(); //id를 입력 받아 공백을 제거한다.
            AppendText(txtHistory, "서버와 연결되었습니다");
            groupnum = groupnum + 1;
            string msg = "GRP:" + groupnum + ":" + ChatClient.nameID + ":" + Form1.selectedidList;
            Console.WriteLine(msg);
            byte[] data = Encoding.UTF8.GetBytes(msg);
            mainSock.Send(data);

            //AsyncObject obj = new AsyncObject(4096);
            //obj.WorkingSocket = mainSock;
            //mainSock.BeginReceive(obj.Buffer, 0, obj.BufferSize, 0, DataReceived, obj);
        }

        void AppendText(Control ctrl, string s)
        {
            if (ctrl.InvokeRequired) ctrl.Invoke(_textAppender, ctrl, s);
            else
            {
                string source = ctrl.Text;
                ctrl.Text = source + Environment.NewLine + s;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
