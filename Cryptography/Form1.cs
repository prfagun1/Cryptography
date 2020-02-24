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

namespace WindowsFormsApplication2
{
    public partial class fCriptografiaContingenciaAutorizador : Form
    {
        public fCriptografiaContingenciaAutorizador(){
            InitializeComponent();
        }


        public string DecryptAES(string cipherText, string key){
            string EncryptionKey = key;
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public string EncryptAES(string clearText, string key) {
            string EncryptionKey = key;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create()) {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }


        private void bDescriptografar_Click(object sender, EventArgs e){
            try{
                tTexto02.Text = this.DecryptAES(tTextoCriptografado02.Text, tChaveDeDescriptografia.Text);
            }
            catch(Exception erro) {
                tTexto02.Text = "Erro ao descriptografar";
            }


        }

        private void bCriptografar_Click(object sender, EventArgs e)  {
            try { 
                tTextoCriptografado01.Text = this.EncryptAES(tTexto01.Text, tChaveDeCriptografia.Text);
            }
            catch(Exception erro) {
                tTexto01.Text = "Erro ao criptogragar: " + e.ToString();
            }
        }

        private void bCopiar01_Click(object sender, EventArgs e){
            Clipboard.SetText(tTextoCriptografado01.Text);
        }

        private void button1_Click(object sender, EventArgs e){
            Clipboard.SetText(tTexto02.Text);
        }

        private void bGerarHash_Click(object sender, EventArgs e){
            String texto = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + tChaveDeCriptografia.Text;

            tTextoCriptografado01.Text = this.EncryptAES(texto, tChaveDeCriptografia.Text).Replace("/","");
        }
    }
}
