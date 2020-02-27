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
        public SQL_Explorer(string Log_Exp_User, string Log_Exp_Pass, Label lblErr)
        {
            LogUser = Log_Exp_User; LogPass = Log_Exp_Pass; lblERROR = lblErr;
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
            MySqlConnection LOGSqlConnection = new MySqlConnection(connectionString());
            LOGSqlConnection.Open();
                
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
                                lblERROR.Content = "Неправильный Pass";
                            }
                            else
                            {
                                lblERROR.Content = " Connection successful! ";
                                ActivateSession = true;                            
                                break;
                            }
                            break;
                        }
                    }
                    reader.Close();
                    LOGSqlConnection.Close();
                }
                else
                {
                  lblERROR.Content = " NickName не найден";
                }
        }
    }
}
