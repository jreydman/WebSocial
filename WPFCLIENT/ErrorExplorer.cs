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
        private Label lblERROR;
        private string Name;
        private string Pass;
        //answers
        static string[] Answers = new string[3]
        {
            " User, Pass содержит некорректные символы или строки пустые; ",
            " User содержит некорректные символы или строка пустая; ",
            " Pass содержит некорректные символы или строка пустая; "
        };
        //bools
        public static bool error_key { get; set; } = true;

        public ErrorExplorer(string name, string pass,Label lblErr)
        {
            Name = name; Pass = pass; lblERROR = lblErr;
        }
        private static bool UserPassContains(string str)
        {
            return !string.IsNullOrEmpty(str) && !Regex.IsMatch(str, @"[^a-zA-z\d_]") && str.Length<9;
        }
        private static string UserPassErrorText(string name, string pass)
        {
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
            lblERROR.Content = UserPassErrorText(Name, Pass);
        }

    }
}

