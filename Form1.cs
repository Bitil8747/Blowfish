using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Security;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Blowfish
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		string text;
		int q, p, g, h, x, y, k, r, s, hv;
		Random rn = new Random();

		private void H()
		{
			h = 2;
			if (Math.Pow(h, (p - 1) / q) % p == 1)
			{
				while (Math.Pow(h, (p - 1) / q) % p != 1)
					h = rn.Next(1, p - 1);
			}
		}
		private void G()
		{
			H();
			g = (int)Math.Pow(h, (p - 1) / q) % p;
		}

		private void Move(Control A)
		{
			panel3.Height = A.Height;
			panel3.Top = A.Top;
        }

		private void K()
		{
			k = (int)Math.Pow(k, p - 2) % p;
		}
		private void S(string sms)
		{
			byte[] hash;
			SHA1Managed sha1 = new SHA1Managed();
			hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(sms));
			StringBuilder sb = new StringBuilder(hash.Length*2);
			foreach (byte b in hash)
				sb.Append(b.ToString("X2"));
			sb.ToString();
			for (int i =0; i<sb.Length; i+=4)
			{
				hv += Convert.ToInt32(sb[i]);
			}
			text = Convert.ToString(sb);
		}

		private void bunifuButton1_Click(object sender, EventArgs e)
		{
			Move(bunifuButton1);
			S(bunifuTextBox1.Text);
			if (bunifuTextBox1.Text != "")
			{
				S(bunifuTextBox1.Text);
				bunifuTextBox2.Clear();
				bunifuTextBox2.Text = text;
			}
			else
				MessageBox.Show("Введите сообщение","Ошибка",MessageBoxButtons.OK);
			q = 11;
			p = 23;
			while (r == 0 && s == 0)//signing the massege
			{
				k = rn.Next(1, q);
				r = ((int)Math.Pow(g, k) % p) % q;
				K();
				s = (hv + Convert.ToInt32(bunifuTextBox6.Text) * r) * k;
			}
			bunifuTextBox6.Text = r + ", " + s;
		}

		private void bunifuButton2_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void panel1_MouseDown(object sender, MouseEventArgs e)
		{
			panel1.Capture = false;
			Message m = Message.Create(Handle,0xa1,new IntPtr(2),IntPtr.Zero);
			WndProc(ref m);
		}

		private void bunifuButton3_Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
		}

		private void bunifuButton6_Click(object sender, EventArgs e)
		{
			G();
			if (bunifuTextBox6.Text == "")
			{
				x = rn.Next(0,q);
				bunifuTextBox6.Text = Convert.ToString(x);
			}
			if (bunifuTextBox5.Text == "")
			{
				y = (int)Math.Pow(g,x) % p;
				bunifuTextBox5.Text = Convert.ToString(x);
			}
		}

		private void bunifuButton4_Click(object sender, EventArgs e)
		{
			Move(bunifuButton4);
		}

		private void bunifuButton5_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			if (ofd.ShowDialog() == DialogResult.OK)//открываем диалоговое окно
			{
				bunifuTextBox1.Text = File.ReadAllText(ofd.FileName);
			}
				Move(bunifuButton5);
		}
	}
}
