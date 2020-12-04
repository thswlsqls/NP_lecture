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

namespace AsyncGUISimpleServer
{

    public partial class ChatForm : Form
    {
        Socket mainSock;  //서버 소켓
        IPAddress thisAddress;
        // Socket Client;
        delegate void AppendTextDelegate(Control ctrl, string s);
        AppendTextDelegate _textAppender;

        delegate void AddItemDelegate(Control ctrl, string s);
        AddItemDelegate _ItemAdder;

        delegate void RemoveItemDelegate(Control ctrl, object o);
        RemoveItemDelegate _ItemRemover;

        Dictionary<string, Socket> connectedClients;
        //Dictionary<int, Dictionary<string, Socket>> connectedClients_grp;
        int clientNum;
        string idList="";

        public ChatForm()
        {
            InitializeComponent();
            mainSock = new Socket(AddressFamily.InterNetwork,
                                  SocketType.Stream,
                                  ProtocolType.Tcp);

            _textAppender = new AppendTextDelegate(AppendText);
            _ItemAdder = new AddItemDelegate(AddItem);
            _ItemRemover = new RemoveItemDelegate(RemoveItem);
            
            connectedClients = new Dictionary<string, Socket>();
            clientNum = 0;
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
        void AddItem(Control ctrl, string s)
        {
            if (ctrl.InvokeRequired) ctrl.Invoke(_ItemAdder, ctrl, s);
            else
            {
                ConListBox.Items.Add(s);
            }
        }

        void RemoveItem(Control ctrl, object o)
        {
            if (ctrl.InvokeRequired) ctrl.Invoke(_ItemRemover, ctrl, o);
            else
            {
                ConListBox.Items.Remove(o);
            }
        }

        private void OnFormLoaded(object sender, EventArgs e)
        {
            Dictionary<int, Dictionary<string, Socket>> connectedClients_grp;
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                mainSock.Close();
            }
            catch { }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            int port;
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
            IPEndPoint serverEP = new IPEndPoint(thisAddress, port);
            mainSock.Bind(serverEP);
            mainSock.Listen(10);

            AppendText(txtHistory, "서버시작");
            mainSock.BeginAccept(AcceptCallback, null);
        }

        void AcceptCallback(IAsyncResult ar)
        {
            Socket client = mainSock.EndAccept(ar);

            AppendText(txtHistory, string.Format
                ("클라이언트 (@ {0}가 연결되었습니다", client.RemoteEndPoint));
            mainSock.BeginAccept(AcceptCallback, null);

            AsyncObject obj = new AsyncObject(4096);
            obj.workingSocket = client;

            client.BeginReceive(obj.Buffer, 0, obj.BufferSize, 0,
                DataReceived, obj);
        }
        void DataReceived(IAsyncResult ar)
        {
            AsyncObject obj = ar.AsyncState as AsyncObject;
            int received = obj.workingSocket.EndReceive(ar);
            string text = Encoding.UTF8.GetString(obj.Buffer);
            AppendText(txtHistory, text);

            string[] token = text.Split(':');
            string cmd = token[0];
            string fromID = null;
            string toID = null;

            if (cmd.Equals("ID"))
            {
                clientNum++;
                fromID = token[1].Trim();
                // 소켓이랑 id 랑 저장
                connectedClients.Add(fromID, obj.workingSocket);
                byte[] bDts = Encoding.UTF8.GetBytes("LOGIN_SUCC:");
                AddItem(ConListBox, fromID);
         
                //foreach (string id in connectedClients.Keys)
                //{
                idList += string.Format("{0}:", fromID); //연결을 요청한 클라이언트의 아이디를 ":"구분자를 사용하여 문자열에 붙인다.
                //}
                Console.WriteLine("접속자 목록:" + idList);
                string[] ConListToken = idList.Split(':');

                //byte[] bDts2 = Encoding.UTF8.GetBytes("ConList:" + idList);
                //AppendText(txtHistory, idList);
                //string[] ConListToken = idList.Split(':');

                //obj.workingSocket.Send(bDts);
                //sendAll(obj.workingSocket, obj.Buffer);

                //obj.workingSocket.Send(bDts2);
                //sendAll(obj.workingSocket, bDts2);

            }
            else if (cmd.Equals("BR"))
            {
                fromID = token[1].Trim();
                string msg = token[2];
                byte[] bDts = Encoding.UTF8.GetBytes
                    ("BR:" + fromID + ":" + msg + ":");
                sendAll(obj.workingSocket, bDts);
                byte[] bDts2 = Encoding.UTF8.GetBytes("BR_SUCC:");
                obj.workingSocket.Send(bDts2); //BR메시지를 보낸 클라이언트에게 전송한다.
            }
            else if (cmd.Equals("TO"))
            {
                fromID = token[1].Trim();
                toID = token[2].Trim();
                string msg = token[3];

                connectedClients.TryGetValue(toID, out Socket socket);
                sendTo(socket, obj.Buffer);
                byte[] bDts2 = Encoding.UTF8.GetBytes("TO_SUCC:");
                obj.workingSocket.Send(bDts2); //TO메시지를 보낸 클라이언트에게 전송한다.
            }
            else if (cmd.Equals("ConList"))
            {
                byte[] bDts = Encoding.UTF8.GetBytes("ConListSEND:" + idList);
                obj.workingSocket.Send(bDts);
                //sendAll(obj.workingSocket, bDts);
            }
            else if (cmd.Equals("CLOSE"))
            {
                fromID = token[1].Trim();
                RemoveItem(ConListBox, fromID);
                connectedClients.Remove(fromID);
                //byte[] bDts = Encoding.UTF8.GetBytes("연결을 종료했습니다." );
                //obj.workingSocket.Send(bDts);
            }
            else if(cmd.Equals("GRP"))
            {

                byte[] bDts = Encoding.UTF8.GetBytes("GRPGEN:" + token[2]);
                Console.WriteLine(token[3]);
                Console.WriteLine(token[4]);
                Console.WriteLine(token[5]);
                int length = token.Length;
                Console.WriteLine(length);

                for (int i = 3; i < length-2; i++)
                {
                    console.writeline(token[i]);

                    connectedclients.trygetvalue(token[i], out socket socket);
                    sendto(socket, bdts);
                }
            }
            else
            {
                string msg = token[1];
                byte[] bDts = Encoding.UTF8.GetBytes(cmd + ':' + msg);
            }
            obj.ClearBuffer();
            obj.workingSocket.BeginReceive(obj.Buffer, 0, obj.BufferSize, 0,
                DataReceived, obj);
        }

        void sendTo(Socket socket, byte[] buffer)
        {
            try
            {
                socket.Send(buffer);
            }
            catch
            {
                try
                {
                    socket.Dispose();
                }
                catch { }
            }

        }

        void sendAll(Socket except, byte[] buffer)
        {
            foreach (Socket socket in connectedClients.Values)
            {
                if (socket != except)
                {
                    try
                    {
                        socket.Send(buffer);
                    }
                    catch
                    {
                        try
                        {
                            socket.Dispose();
                        }
                        catch { }
                    }
                }
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!mainSock.IsBound)
            {
                return;
            }
            string tts = txtTTS.Text.Trim();
            if (string.IsNullOrEmpty(tts))
            {
                return;
            }
            //if (!Client.IsBound)
            //{
            //    return;
            //}

            byte[] bDts = Encoding.UTF8.GetBytes("server: " + tts);

            AppendText(txtHistory, "server : " + tts);
            sendAll(null, bDts);
            txtTTS.Clear();
        }

        private void ConListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}




