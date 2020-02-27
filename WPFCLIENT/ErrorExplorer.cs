using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;
using System.Text.RegularExpressions;
namespace WPFCLIENT
{
    public class ErrorExplorer
    {
        //session ErrorExplorer
        private static Label lblERROR;
        private MySql.Data.MySqlClient.MySqlConnection SQL;
        //answers
        static string[] Answers = new string[7]
        {
            " NickName, Password содержит некорректные символы или строки пустые; ",
            " NickName содержит некорректные символы или строка пустая; ",
            " Password содержит некорректные символы или строка пустая; ",
            " Name содержит некорректные символы или строка пустая; ",
            " Surname содержит некорректные символы или строка пустая; ",
            " RePassword содержит некорректные символы или строка пустая; ",
            " Pass и RePass не совпадают; "
        };
        //bool
        public static bool error_key { get; set; } = true;
        public static bool valide_key { get; set; } = false;
        //SQL
        SQL_Explorer SE;

        public ErrorExplorer(Label lblErr, MySql.Data.MySqlClient.MySqlConnection sql)
        {
            lblERROR = lblErr; SQL = sql;
        }
        private static bool UserPassContains(string str)
        {
            return !string.IsNullOrEmpty(str) && !Regex.IsMatch(str, @"[^a-zA-z\d_]");
        }
        private static bool RegistrationContains(string str)
        {
            return !string.IsNullOrEmpty(str) && !Regex.IsMatch(str, @"[^a-zA-z\d_]");
        }
        private static string LOGErrorText(string name, string pass)
        {
            error_key = true;
            if (UserPassContains(name) == false)
            {
                if (UserPassContains(name) == false && UserPassContains(pass) == false || pass.Length < 9)
                {
                    return Answers[0];
                }
                else return Answers[1];
            }
            else if (UserPassContains(pass) == false || pass.Length < 9)
            {
                if (UserPassContains(name) == false && UserPassContains(pass) == false || pass.Length < 9)
                    {
                      return Answers[0];
                    }
                else return Answers[2];
            }
            else 
            {
                error_key = false;
                return "";               
            }
        }
        private static string REGErrorText(string nickname, string name, string surname, string pass, string repass)
        {
            error_key = true;
            if (RegistrationContains(nickname) == false || nickname.Length < 5) { return Answers[1]; }
            else if(RegistrationContains(name) == false) { return Answers[3]; }
            else if(RegistrationContains(surname) == false) { return Answers[4]; }
            else if(RegistrationContains(pass) == false || pass.Length < 9) { return Answers[2];}
            else if (RegistrationContains(repass) == false) { return Answers[5]; }
            else if (RegistrationContains(pass) == true && RegistrationContains(repass) == true && pass != repass) { return Answers[6]; }
            else { error_key = false; return ""; }
        }
        public void activateLOG(string NickName, string Pass)
        {           
            if (!valide_key)
            {
                lblERROR.Content = LOGErrorText(NickName, Pass);
                if (!error_key) { SE = new SQL_Explorer(lblERROR, SQL); SE.ValidOpeningLOG(NickName, Pass); if (SE.ActivateSession == true) { SE.Session(); if (SE.ActivateSession) { valide_key = true; MessageBox.Show(SE.User.Name.ToString()); } } }
            }
            else lblERROR.Content = " Session was initialized! ";
        }
        public void activateREG(string NickName, string Name, string Surname, string Pass, string RePass)
        {
            if(!valide_key)
            {
                lblERROR.Content = REGErrorText(NickName, Name, Surname, Pass, RePass);
                if (!error_key) { SE = new SQL_Explorer(lblERROR, SQL); SE.ValidOpeningREG(NickName, Name, Surname, Pass); if (SE.ActivateSession == true) { SE.Session(); if (SE.ActivateSession) { valide_key = true; MessageBox.Show(SE.User.Name.ToString()); } } }
            }
            else lblERROR.Content = " Session was initialized! ";
        }

    }
}

