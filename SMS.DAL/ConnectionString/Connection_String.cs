using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.DAL
{
    public sealed class Connection_String
    {
        public string Database = "sms";
        public string Server = "localhost";
        public string Port = "3306";
        public string Uid = "root";
        public String ConnectionString = "";

        private static Connection_String _Connection_String;
        public static Connection_String m_Connection_String
        {
            get
            {
                if (_Connection_String == null)
                {
                    _Connection_String = new Connection_String();
                }
                return _Connection_String;
            }
        }

        private Connection_String()
        {
            ReadDatabaseFile();
            ConnectionString = @"Server=" + Server + "; port=" + Port + "; Database=" + Database + "; Uid=" + Uid + "; Pwd=7120020@123; default command timeout=1000;CHARSET=utf8";
        }

        void ReadDatabaseFile()
        {
            string line;
            int i = 0;
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("Database.txt"))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (i == 0 && line.Trim() != "")
                        {
                            Server = line;
                        }
                        if (i == 1 && line.Trim() != "")
                        {
                            Port = line;
                        }
                        if (i == 2 && line.Trim() != "")
                        {
                            Database = line;
                        }
                        if (i == 3 && line.Trim() != "")
                        {
                            Uid = line;
                        }
                        i++;
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                using (StreamWriter outputFile = new StreamWriter("Database.txt"))
                {
                    outputFile.WriteLine("localhost");
                    outputFile.WriteLine("3306");
                    outputFile.WriteLine("sms");
                    outputFile.WriteLine("root");
                }
            }
        }
    }
}
