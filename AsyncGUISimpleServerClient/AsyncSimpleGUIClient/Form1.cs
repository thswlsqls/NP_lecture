using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncSimpleGUIClient
{
    using AsyncSimpleGUIClient;
    public partial class Form1 : Form
    {
        delegate void AddItemDelegate(Control ctrl, string s);
        AddItemDelegate _ItemAdder;

        delegate void RemoveItemDelegate(Control ctrl, int i);
        RemoveItemDelegate _ItemRemover;


        public Form1()
        {
            InitializeComponent();
            _ItemAdder = new AddItemDelegate(AddItem);
            _ItemRemover = new RemoveItemDelegate(RemoveItem);
        }

        void AddItem(Control ctrl, string s)
        {
            if (ctrl.InvokeRequired) ctrl.Invoke(_ItemAdder, ctrl, s);
            else
            {
                ConListBox.Items.Add(s);
            }
        }

        void RemoveItem(Control ctrl, int i)
        {
            if (ctrl.InvokeRequired) ctrl.Invoke(_ItemRemover, ctrl, i);
            else
            {
                ConListBox.Items.RemoveAt(i);
            }
        }
        //private string Form1_value;

        //public string Passvalue
        //{
        //    get { return this.Form1_value; }
        //    set { this.Form1_value = value; }  // 다른폼(Form1)에서 전달받은 값을 쓰기
        //}
        
        //string[] ConListToken = ChatClient.IdList.Split(':');

        private void Form1_Load(object sender, EventArgs e)
        {
            //string IdList = Passvalue;
            
            Console.WriteLine("form1:접속자목록:" + ChatClient.ConListToken);
            Console.WriteLine(ChatClient.ConListToken[0]);
            for(int i = 1; i< ChatClient.ConListToken.Length; i++)
            {
                AddItem(ConListBox, ChatClient.ConListToken[i]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
