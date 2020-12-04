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
    
    public partial class ChatClient : Form
    {
        Socket mainSock;
        IPAddress thisAddress;
        //public static string IdList;
        public static string[] ConListToken;

        public static int port;
        public static string nameID;

        delegate void AppendTextDelegate(Control ctrl, string s);
        AppendTextDelegate _textAppender;

        //delegate void AddItemDelegate(Control ctrl, string s);
        //AddItemDelegate _ItemAdder;

        //string nameID; //ID
        
        //string [] token;

        public ChatClient()
        {
            InitializeComponent();
            mainSock = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            _textAppender = new AppendTextDelegate(AppendText);
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

        private void OnFormLoaded(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!mainSock.IsBound)
            {
                //MsgBoxHelper.Warn("서버가 실행되고 있지 않습니다!");
                return;
            }

            // 보낼 텍스트

            string tts =  txtTTS.Text.Trim(); //보낼 텍스트의 공백을 제거한다.
            if (string.IsNullOrEmpty(tts))
            {
                //MsgBoxHelper.Warn("텍스트가 입력되지 않았습니다!");
                txtTTS.Focus();
                return;
            }
            string[] token = tts.Split(':'); //입력값을 ':'구분자를 기준으로 구분하여 배열에 저장한다.

            if (token[0].Equals("TO"))
            {
                byte[] bDts = Encoding.UTF8.GetBytes
                    ("TO:" + nameID + ":" + token[1]+":" + token[2]);
                mainSock.Send(bDts);
            }
            else
            {
                byte[] bDts = Encoding.UTF8.GetBytes
                 ("BR:" + nameID + ":" + token[0] + ":");
                mainSock.Send(bDts);
            }
        }

        void DataReceived(IAsyncResult ar)
        {
            AsyncObject obj = (AsyncObject)ar.AsyncState;

            // 데이터 수신을 끝낸다.
            int received = obj.WorkingSocket.EndReceive(ar);

            string text = Encoding.UTF8.GetString(obj.Buffer);

            string[] token = text.Split(':');
            string cmd = token[0];
            if (cmd.Equals("LOGIN_SUCC"))
            {
                AppendText(txtHistory, "로그인 성공");
            }
            else if (cmd.Equals("BR_SUCC"))
            {
                AppendText(txtHistory, "브로드캐스트 성공");
            }
            else if (cmd.Equals("TO_SUCC"))
            {
                AppendText(txtHistory, "To 성공");
            }
            else if (cmd.Equals("TO"))
            {
                string fromID = token[1];
                string toID = token[2];
                string msg = token[3];
                AppendText(txtHistory, string.Format
                    ("[From: {0}][To: {1}] {2}", fromID, toID, msg));
            }
            else if (cmd.Equals("BR"))
            {
                AppendText(txtHistory, "[전체]" + token[1]+":"+token[2]);
            }
            else if (cmd.Equals("server"))
            {
                AppendText(txtHistory, "[공지]"+token[1]);
            }
            else if (cmd.Equals("ConListSEND"))
            {
                //IdList = text;
                ConListToken = text.Split(':'); //
            }
            else if (cmd.Equals("GRPGEN"))
            {
                DialogResult result = MessageBox.Show("채팅에 초대되었습니다.", "", MessageBoxButtons.YesNo);
                if(result == DialogResult.Yes)
                {
                    Form2 form2 = new Form2();
                    form2.Show();
                }
                else
                {

                }
            }
            else
            {
                AppendText(txtHistory, text);
            }

            obj.ClearBuffer();
            obj.WorkingSocket.BeginReceive(obj.Buffer, 0, obj.BufferSize, 0, DataReceived, obj);


        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (mainSock.Connected)
            {
                //MsgBoxHelper.Error("이미 연결되어 있습니다!");
                return;
            }
            //int port;
            if (!int.TryParse(txtPort.Text, out port))
            {
                txtPort.Focus();
                txtPort.SelectAll();
                return;
            }
            if (thisAddress == null)
            {
                thisAddress = IPAddress.Loopback;
                txtAddress.Text = thisAddress.ToString();
            }
            IPEndPoint serverEP = new IPEndPoint(thisAddress, port); //server
            try
            {
                mainSock.Connect(thisAddress, port);
            }
            catch // (Exception ex)
            {
                // MsgBoxHelper.Error("연결에 실패했습니다!\n오류 내용: {0}", MessageBoxButtons.OK, ex.Message);
                return;
            }
            // 서버로 ID  전송
            nameID = txtID.Text.Trim(); //id를 입력 받아 공백을 제거한다.
            AppendText(txtHistory,  "서버와 연결되었습니다");
            string msg = "ID:" + nameID + ":";
            byte[] data = Encoding.UTF8.GetBytes(msg);
            mainSock.Send(data);

            AsyncObject obj = new AsyncObject(4096);
            obj.WorkingSocket = mainSock;
            mainSock.BeginReceive(obj.Buffer, 0, obj.BufferSize, 0, DataReceived, obj);
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mainSock != null) //      if (!mainSock.IsBound)
            {
                mainSock.Disconnect(false);
                mainSock.Close();
            }
        }

        private void btnConList_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("서버에서 수신한 접속자 목록이 저장되었는지 확인합니다: " + ChatClient.IdList);

            string msg = "ConList:" + nameID + ":";
            byte[] data = Encoding.UTF8.GetBytes(msg);
            mainSock.Send(data);

            Form1 newform1 = new Form1();
            //newform1.Passvalue = IdList;
            newform1.Show();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            AppendText(txtHistory, "연결 종료");
            string msg = "CLOSE:"; //+ nameID + ":";
            byte[] data = Encoding.UTF8.GetBytes(msg);
            mainSock.Send(data);
            //mainSock.Disconnect(true); //이 소켓을 다시 연결할 수 있으며 연결을 종료한다.
            
        }
    }
}




