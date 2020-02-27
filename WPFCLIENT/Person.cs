using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace WPFCLIENT
{
    class Person
    {
        public string NickName;
        public string Password;
        public string Name;
        public string Surname;
        public int GroupID;
        public int ID;
        public Person(string nickname)
        {
            NickName = nickname;
            GetInfoPerson(NickName);
        }
        public void GetInfoPerson(string NickName)
        {
            MySqlConnection PersonInfo = new MySqlConnection(SQL_Explorer.connectionString());

                PersonInfo.Open();
                string Select = "SELECT `id`, `NickName`, `Password`, `Name`, `Surname`, `GroupID` FROM `Users` WHERE Nickname LIKE '" + NickName + "'";
                MySqlCommand command = new MySqlCommand(Select, PersonInfo);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        if (reader != null)
                        {
                            ID = Convert.ToInt32(reader[0].ToString());
                            //  NickName = reader[1].ToString();
                            Password = reader[2].ToString();
                            Name = reader[3].ToString();
                            Surname = reader[4].ToString();
                            GroupID = Convert.ToInt32(reader[5].ToString());
                        }
                    }
                    reader.Close();
                    PersonInfo.Close();
                }
        }
    }
}
