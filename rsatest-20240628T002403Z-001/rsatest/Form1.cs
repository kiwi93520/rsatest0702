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

namespace rsatest
{
    
    public partial class Form1 : Form
    {
        private RSACryptoServiceProvider myRsa;
        private RSACryptoServiceProvider mingRsa;
        private Form2 form2;
        public Form1(Form2 form2Instance)
        {
            InitializeComponent();
            form2 = form2Instance;
        }

        private void exchange_Click(object sender, EventArgs e)
        {
            textBox5.Text = form2.GetPublicKey();
            mingRsa = new RSACryptoServiceProvider();
            mingRsa.FromXmlString(textBox5.Text);
        }
       

        private void generate_Click(object sender, EventArgs e)
        {
            myRsa = new RSACryptoServiceProvider(2048);
            textBox1.Text = myRsa.ToXmlString(false);
            textBox2.Text = myRsa.ToXmlString(true);
            MessageBox.Show("Keys generated.");
        }

        private void B_public_encrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (mingRsa == null)
                {

                    mingRsa.FromXmlString(form2.GetPublicKey());
                }

                string message = textBox3.Text;
                if (message == null)
                {
                    MessageBox.Show("Please enter the text!");
                }

                // Encrypt the message
                byte[] encryptedMessage = mingRsa.Encrypt(Encoding.UTF8.GetBytes(message), false);


                // Sign the message
                byte[] hash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(message));
                byte[] signature = myRsa.SignHash(hash, CryptoConfig.MapNameToOID("SHA256"));

                // Send encrypted message and signature to Form2
                form2.ReceiveMessage(encryptedMessage, signature);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        public string GetPublicKey()
        {
            return textBox1.Text;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
