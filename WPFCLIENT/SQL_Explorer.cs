using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;
using System.Text.RegularExpressions;
using System.Management;
using System.IO;
using System.Net;
using System.Net.Mail;
using MySql.Data.MySqlClient;
namespace WPFCLIENT
{
    class SQL_Explorer
    {
        private Label lblERROR;
        private Crypt crypt;
        private Mail_Explorer ME;
        //Session
        public bool ActivateSession { get; set; } = false;

        //Login
        public string LogUser { get; set; } = "";
        public string LogPass { get; set; } = "";
        //Registration

        //Person
        public Person User;
        //SQL
        private static MySqlConnection SqlConnection;
        public SQL_Explorer(Label lblErr, MySqlConnection sql)
        {
             lblERROR = lblErr; SqlConnection = sql; crypt = new Crypt(lblERROR); ME = new Mail_Explorer();
        }
        public static string connectionString()
        {
            string server = "remotemysql.com";
            string database = "zcy327Y8WW";
            string uid = "zcy327Y8WW";
            string password = "CJYsv1bAPM";
            return "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        }
        public static string InsertNewUser(string regNickName, string regPass, string regName, string regSurname)
        {
            return "INSERT INTO `Users` (`NickName`, `Password`, `Name`, `Surname`) VALUES ('" + regNickName + "', '" + regPass + "', '" + regName + "', '" + regSurname + "')";
        }
        public void Session()
        {
            if(ActivateSession)
            {
              User = new Person(LogUser);
            }
        }
        public void ValidOpeningLOG(string LogUser, string LogPass)
        {
            if (ActivateSession == false&&SqlConnection.State==System.Data.ConnectionState.Open)
            {
                    string try_user = null;
                    string try_pass = null;
                    string Select = "SELECT `ID`, `NickName`, `Password`, `Name`, `Surname` FROM `Users` WHERE Nickname LIKE '" + LogUser + "'";
                    MySqlCommand command = new MySqlCommand(Select, SqlConnection);
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            if (reader != null)
                            {                               
                                try_user = reader[1].ToString();
                                try_pass = crypt.Decrypt(reader[2].ToString());
                                if (try_pass != LogPass)
                                {
                                    lblERROR.Content = " Неправильный Pass ";
                                }
                                else
                                {
                                    lblERROR.Content = " Connection successful! ";
                                    ActivateSession = true;
                                    SqlConnection.Close();
                                    break;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        lblERROR.Content = " NickName не найден ";
                    }
                    reader.Close();
            }
            else { lblERROR.Content = " Session was initialized! "; }
        }
        public void ValidOpeningREG(string RegNickName, string RegEmail, string RegName, string RegSurname,string RegPass)
        {
            if (ActivateSession == false && SqlConnection.State == System.Data.ConnectionState.Open)
            {
                string Select = "SELECT `ID`, `NickName` FROM `Users` WHERE Nickname LIKE '" + RegNickName + "'";
                bool blocUserFlag = true;
                MySqlCommand command = new MySqlCommand(Select, SqlConnection);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader != null) { blocUserFlag = false; break; }
                    }
                }
                reader.Close();
                if (blocUserFlag == true)
                {
                    using (StreamWriter sw = new StreamWriter("validationToken.txt")) { sw.WriteLine(RegNickName + " " + RegEmail + " " + RegName + " " + RegSurname + " " + crypt.Encypt(RegPass)); }
                    ME.FTPUploadFile("validationToken.txt", RegNickName);
                    File.Delete("validationToken.txt");
                    lblERROR.Content = " Connection successful! ";
                    ActivateSession = true;
                    SqlConnection.Close();
                }
                else lblERROR.Content = " Такой NickName уже существует ";
            }
            else { lblERROR.Content = " Session was initialized! "; }
        }
    }
}
