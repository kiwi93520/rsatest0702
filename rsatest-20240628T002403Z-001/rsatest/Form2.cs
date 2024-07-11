using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace rsatest
{
    public partial class Form2 : Form
    {
        private RSACryptoServiceProvider mingRsa;
        private RSACryptoServiceProvider yourRsa;
        private byte[] receivedEncryptedMessage;
        private byte[] receivedSignature;
        public Form1 form1;

        public Form2(Form1 form1Instance)
        {
            InitializeComponent();
            form1= form1Instance;
        }

        private void generate_Click(object sender, EventArgs e)
        {
            mingRsa = new RSACryptoServiceProvider(2048);
            textBox1.Text =mingRsa.ToXmlString(false); 
            textBox2.Text =mingRsa.ToXmlString(true);
            MessageBox.Show("Keys generated.");
        }

        public void ReceiveMessage(byte[] encryptedMessage, byte[] signature)
        { 

            textBox3.Text=Convert.ToBase64String(encryptedMessage);
            receivedEncryptedMessage = encryptedMessage;
            receivedSignature = signature;
            
        }
        public string GetPublicKey()
        {
            return textBox1.Text;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void exchange_Click(object sender, EventArgs e)
        {
            textBox5.Text=form1.GetPublicKey();
            yourRsa = new RSACryptoServiceProvider();
            yourRsa.FromXmlString(textBox5.Text);
        }

        private void A_public_decrypt_Click(object sender, EventArgs e)
        {
            try 
            {
                if (receivedEncryptedMessage == null || receivedSignature == null)
                {
                    MessageBox.Show("No message to decrypt and verify.");
                    return;
                }
                // Decrypt the message
                byte[] decryptedMessageBytes = mingRsa.Decrypt(receivedEncryptedMessage, false);
                string decryptedMessage = Encoding.UTF8.GetString(decryptedMessageBytes);

                // Verify the signature

                byte[] hash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(decryptedMessage));
                bool isSignatureValid = yourRsa.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), receivedSignature);

                if (isSignatureValid)
                {
                    MessageBox.Show("Signature is valid. Message: " + decryptedMessage);
                }
                else
                {
                    MessageBox.Show("Signature is invalid.");
                }

            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
    }
}
