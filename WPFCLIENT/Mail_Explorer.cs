using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
namespace WPFCLIENT
{
    class Mail_Explorer
    {
        public static string GetTocken()
        {

            int num_letters = 8;
            int num_words = 5;

            char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

            Random rand = new Random();
            string word = "";
            for (int i = 1; i <= num_words; i++)
            {
                for (int j = 1; j <= num_letters; j++)
                {
                    int letter_num = rand.Next(0, letters.Length - 1);
                    word += letters[letter_num];
                }
            }
            return word;
        }
        public void FTPUploadFile(string filename, string nickname)
        {
            FileInfo toUpload = new FileInfo(filename);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://k95580v8.beget.tech/public_html/Tokens/" + nickname+".txt");
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential("k95580v8_admin", "wscXh3D1910");
            Stream ftpStream = request.GetRequestStream();
            FileStream fileStream = File.OpenRead(filename);
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            do
            {
                bytesRead = fileStream.Read(buffer, 0, 1024);
                ftpStream.Write(buffer, 0, bytesRead);
            }
            while (bytesRead != 0);
            fileStream.Close();
            ftpStream.Close();
        }
    }
}
