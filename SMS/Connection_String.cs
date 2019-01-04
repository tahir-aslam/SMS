using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS
{
    public static class Connection_String
    {
        public static String con_string = @"Server=" + MainWindow.Server + "; port=" + MainWindow.Port + "; Database=" + MainWindow.Database + "; Uid=" + MainWindow.Uid + "; Pwd=7120020@123; default command timeout=1000;CHARSET=utf8";
        //public static String con_string = @"Server=" + MainWindow.Server + "; port=" + MainWindow.Port + "; Database=" + MainWindow.Database + "; Uid=" + MainWindow.Uid + "; Pwd=laptopbscs12345; default command timeout=1000;CHARSET=utf8";
        //public static String con_string = @"Server = 192.169.57.154; Database = tahir123_providence; Uid = tahir123_nasir ; Pwd =providence@scenario;default command timeout=280;";
        public static String con_string_web = @"Server = 143.95.238.92; Database = tahir123_ehsan; Uid = tahir123_tahir ; Pwd =scenario123;default command timeout=180;";
        public static string tahir123_sms_security = @"Server =143.95.238.92; Database = tahir123_sms_security; Uid = tahir123_license ; Pwd=&T7yH[4UA^-k;default command timeout=180;";
    }
}
