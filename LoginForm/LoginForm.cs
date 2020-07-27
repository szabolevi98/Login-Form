using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginForm
{
    public partial class LoginForm : Form
    {
        readonly Dictionary<string, string> users = new Dictionary<string, string>()
        {
            {"Admin", "F7C3BC1D808E04732ADF679965CCC34CA7AE3441"}, //123456789
            {"Test", "8CB2237D0679CA88DB6464EAC60DA96345513964"} //12345
        };

        public LoginForm()
        {
            InitializeComponent();
        }

        public static string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", string.Empty);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Moving the borderless form with MouseDown&Move
        private Point mousePoint;
        private void MouseDownMethod(object sender, MouseEventArgs e)
        {
            mousePoint = e.Location;
        }
        private void MouseMoveMethod(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int dx = e.Location.X - mousePoint.X;
                int dy = e.Location.Y - mousePoint.Y;
                this.Location = new Point(this.Location.X + dx, this.Location.Y + dy);
            }
        }

        private void UserNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (UserNameTextBox.Text == "Username")
            {
                UserNameTextBox.Text = string.Empty;
            }
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            if (PasswordTextBox.Text == "Password")
            {
                PasswordTextBox.Text = string.Empty;
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string username = UserNameTextBox.Text;
            string password = PasswordTextBox.Text;
            if (username == string.Empty)
            {
                MessageBox.Show("Please enter your user name!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (password == string.Empty)
            {
                MessageBox.Show("Please enter your password!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                string passwordSHA1 = Hash(password);
                //MessageBox.Show(passwordSHA1); //For debugging
                bool canLogin = false;
                foreach (var item in users)
                {
                    if (String.Equals(item.Key, username, StringComparison.OrdinalIgnoreCase) && item.Value == passwordSHA1) //Ignore username upper/lowercase
                    {
                        canLogin = true;
                    }
                }
                if (canLogin)
                {
                    this.Hide();
                    MainForm main = new MainForm(username);
                    main.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username/Password wrong!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
    }
}
