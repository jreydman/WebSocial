using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;
using System.Text.RegularExpressions;
using System.Management;
using MySql.Data.MySqlClient;
namespace WPFCLIENT
{
    class SQL_Explorer
    {
        private Label lblERROR;
        //Session
        public bool ActivateSession { get; set; } = false;

        //Login
        public string LogUser { get; set; } = "";
        public string LogPass { get; set; } = "";
        public Person User;
        private int ConfigActivity { get; set; } = 0;   //0 - NOT ACTIVITY ; 1 - LOGIN ; 2 - REGISTRATION ;
        private static MySqlConnection LOGSqlConnection;
        public SQL_Explorer(string Log_Exp_User, string Log_Exp_Pass, Label lblErr, int conf, MySqlConnection sql)
        {
            LogUser = Log_Exp_User; LogPass = Log_Exp_Pass; lblERROR = lblErr; ConfigActivity=conf; LOGSqlConnection = sql;
        }
        public static string connectionString()
        {
            string server = "remotemysql.com";
            string database = "zcy327Y8WW";
            string uid = "zcy327Y8WW";
            string password = "CJYsv1bAPM";
            return "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        }
        public void Session()
        {
            if(ActivateSession)
            {
              User = new Person(LogUser);
            }
        }
        public void ValidOpening()
        {
            if (ActivateSession == false&&LOGSqlConnection.State==System.Data.ConnectionState.Open)
            {
                if (ConfigActivity == 1) //LOGIN
                {
                    string try_user = null;
                    string try_pass = null;
                    string Select = "SELECT `ID`, `NickName`, `Password`, `Name`, `Surname` FROM `Users` WHERE Nickname LIKE '" + LogUser + "'";
                    MySqlCommand command = new MySqlCommand(Select, LOGSqlConnection);
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            if (reader != null)
                            {
                                Crypt validePass = new Crypt(lblERROR);
                                try_user = reader[1].ToString();
                                try_pass = validePass.Decrypt(reader[2].ToString());
                                if (try_pass != LogPass)
                                {
                                    lblERROR.Content = " Неправильный Pass ";
                                }
                                else
                                {
                                    lblERROR.Content = " Connection successful! ";
                                    ActivateSession = true;
                                    LOGSqlConnection.Close();
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
                else if (ConfigActivity == 2) //REGISTRATION
                {

                }
            }
            else { lblERROR.Content = " Session was initialized! "; }
        }
    }
}
