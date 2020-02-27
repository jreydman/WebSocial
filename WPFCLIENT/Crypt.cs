using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;
using System.Text.RegularExpressions;
using System.Management;
using Crypter;
namespace WPFCLIENT
{
    class Crypt
    {
        private static string key { get; } = "0xK2608";
        //cryter.dll
        private static MyCrypt crypt = new MyCrypt(key);
        private Label lblERROR;
        //strings
        private string Content { get; set; } = null;
        public Crypt( Label LBL)
        {
          lblERROR = LBL;
        }
        public void EncryptStr(string str) { if (str != null) { Content = crypt.EncryptData(str); }  else lblERROR.Content = "ERROR"; }
        public void DecryptStr(string str) { if (str != null) { Content = crypt.DecryptData(str); } else lblERROR.Content = "ERROR"; }
        public string Encypt(string str) { EncryptStr(str); return Content; }
        public string Decrypt(string str) { DecryptStr(str); return Content; }
    }
}
