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
using System.Globalization;
using System.Text.RegularExpressions;
namespace WPFCLIENT
{
    public class ErrorExplorer
    {
        //session ErrorExplorer
        private static Label lblERROR;
        private MySql.Data.MySqlClient.MySqlConnection SQL;
        //answers
        static string[] Answers = new string[8]
        {
            " NickName, Password содержит некорректные символы или строки пустые; ",
            " NickName содержит некорректные символы или строка пустая; ",
            " Password содержит некорректные символы или строка пустая; ",
            " Name содержит некорректные символы или строка пустая; ",
            " Surname содержит некорректные символы или строка пустая; ",
            " RePassword содержит некорректные символы или строка пустая; ",
            " Pass и RePass не совпадают; ",
            " Email не корректный "
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
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        private static bool UserPassContains(string str)
        {
            return !string.IsNullOrEmpty(str) && !Regex.IsMatch(str, @"[^a-zA-z\d_]");
        }
        private static bool RegistrationContains(string str)
        {
            return !string.IsNullOrEmpty(str) && !Regex.IsMatch(str, @"[^a-zA-z\d_]");
        }
        private static string REGErrorText(string nickname, string email, string name, string surname, string pass, string repass)
        {
            error_key = true;
            if (RegistrationContains(nickname) == false || nickname.Length < 5) { return Answers[1]; }
            else if (IsValidEmail(email)==false) { return Answers[7]; }
            else if(RegistrationContains(name) == false) { return Answers[3]; }
            else if(RegistrationContains(surname) == false) { return Answers[4]; }
            else if(RegistrationContains(pass) == false || pass.Length < 9) { return Answers[2];}
            else if (RegistrationContains(repass) == false) { return Answers[5]; }
            else if (RegistrationContains(pass) == true && RegistrationContains(repass) == true && pass != repass) { return Answers[6]; }
            else { error_key = false; return ""; }
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
        public void activateLOG(string NickName, string Pass)
        {           
            if (!valide_key)
            {
                lblERROR.Content = LOGErrorText(NickName, Pass);
                if (!error_key) { SE = new SQL_Explorer(lblERROR, SQL); SE.ValidOpeningLOG(NickName, Pass); if (SE.ActivateSession == true) { SE.Session(); if (SE.ActivateSession) { valide_key = true; MessageBox.Show(SE.User.Name.ToString()); } } }
            }
            else lblERROR.Content = " Session was initialized! ";
        }
        public void activateREG(string NickName, string Email, string Name, string Surname, string Pass, string RePass)
        {
            if(!valide_key)
            {
                lblERROR.Content = REGErrorText(NickName, Email, Name, Surname, Pass, RePass);
                if (!error_key) { SE = new SQL_Explorer(lblERROR, SQL); SE.ValidOpeningREG(NickName, Email, Name, Surname, Pass); if (SE.ActivateSession == true) { SE.Session(); if (SE.ActivateSession) { valide_key = true; MessageBox.Show(SE.User.Name.ToString()); /*Site.Send(PersonInfo);Site.generateKeyLing()*/} } }
            }
            else lblERROR.Content = " Session was initialized! ";
        }
        public static void CleanToRes()
        {
            error_key = true; valide_key = false;
        }
    }
}

