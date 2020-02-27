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
        private string NickName;
        private string Pass;
        private MySql.Data.MySqlClient.MySqlConnection SQL;
        //answers
        static string[] Answers = new string[3]
        {
            " User, Pass содержит некорректные символы или строки пустые; ",
            " User содержит некорректные символы или строка пустая; ",
            " Pass содержит некорректные символы или строка пустая; "
        };
        //bool
        public static bool error_key { get; set; } = true;
        public static bool valide_key { get; set; } = false;
        //SQL
        SQL_Explorer SE;

        public ErrorExplorer(string name, string pass,Label lblErr, MySql.Data.MySqlClient.MySqlConnection sql)
        {
            NickName = name; Pass = pass; lblERROR = lblErr; SQL = sql;
        }
        private static bool UserPassContains(string str)
        {
            return !string.IsNullOrEmpty(str) && !Regex.IsMatch(str, @"[^a-zA-z\d_]") && str.Length<9;
        }
        private static string LOGErrorText(string name, string pass)
        {
            error_key = true;
            if (UserPassContains(name) == false)
            {
                if (UserPassContains(name) == false && UserPassContains(pass) == false)
                {
                    return Answers[0];
                }
                else return Answers[1];
            }
            else if (UserPassContains(pass) == false)
            {
                if (UserPassContains(name) == false && UserPassContains(pass) == false)
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
        public void activateLOG()
        {           
            if (!valide_key)
            {
                lblERROR.Content = LOGErrorText(NickName, Pass);
                if (!error_key) { SE = new SQL_Explorer(NickName, Pass, lblERROR, 1, SQL); SE.ValidOpening(); if (SE.ActivateSession == true) { SE.Session(); if (SE.ActivateSession) { valide_key = true; MessageBox.Show(SE.User.Name.ToString()); } } }
            }
            else lblERROR.Content = " Session was initialized! ";
        }

    }
}

