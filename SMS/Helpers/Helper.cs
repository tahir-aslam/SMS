using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SMS.Helpers
{
    public static class Helper
    {

        public static Action<bool> ShowLoginScreen;
        public static int Index { get; set; } = 0;       

        public static string DateFormat { get; set; } = "MM/dd/yyyy HH:mm:ss";        
        public static string Card_TemplateFront { get; set; } = "default";
        public static string Card_TemplateBack { get; set; } = "default";
        
        public static string GetSystemDateTimeFormat()
        {
            try
            {
                {
                    return DateFormat;
                }
            }
            catch (Exception ex)
            {


            }
            return DefaultDateFormat;
        }
        public static string DefaultDateFormat { get; set; } = "MM/dd/yyyy HH:mm:ss";
        
        public static string GetCurrentTimeZoneGMT()
        {

            TimeZoneInfo.ClearCachedData();
            TimeZoneInfo local = TimeZoneInfo.Local;
            local = TimeZoneInfo.Local;
            string founder = local.DisplayName;

            Console.WriteLine(local.DisplayName);
            founder = founder.Replace("UTC", "GMT");
            var first = founder.Split(')')[0];
            first = first.Remove(0, 1);
            first = first.Replace(":00", "");
            if (first == "GMT+00")
            {
                first = "GMT ";
            }
            return first;
        }

        public static byte[] AudioStreamToByteArray(FileStream stream, int initialLength)
        {
            try
            {
                if (initialLength < 1)
                {
                    initialLength = 32768;
                }
                byte[] buffer = new byte[initialLength];
                int read = 0;
                int chunk;
                while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
                {
                    read += chunk;
                    // If we've reached the end of our buffer, check to see if there's
                    // any more information
                    if (read == buffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        // End of stream? If so, we're done
                        if (nextByte == -1)
                        {
                            return buffer;
                        }
                        // Nope. Resize the buffer, put in the byte we've just
                        // read, and continue
                        byte[] newBuffer = new byte[buffer.Length * 2];
                        Array.Copy(buffer, newBuffer, buffer.Length);
                        newBuffer[read] = (byte)nextByte;
                        buffer = newBuffer;
                        read++;
                    }
                }
                // Buffer is now too big. Shrink it.
                byte[] ret = new byte[read];
                Array.Copy(buffer, ret, read);
                return ret;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<List<T>> splitList<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
        
        public static bool PingIp(string IpAddress)
        {
            bool pingable = false;
            Ping pinger = null;
            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(IpAddress, 2000);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }



        public static byte[] ConvertBitmapSourceToByteArray(string filepath)
        {
            var image = new BitmapImage(new Uri(filepath));
            byte[] data;
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        public static bool IpRange(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 0 && i <= 255;
        }

        public static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        public static BitmapImage BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public static byte[] ImageSourceToBytes(ImageSource imageSource)
        {
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;
            BitmapEncoder encoder = new JpegBitmapEncoder();
            if (bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        public static BitmapImage ByteArrayToBitmapImage(byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        //If you change any thing in this fucntion then you also change in HelperDAL class in DAL project
        public static string Encrypt(string input, string key = "lkaju-x3h4-f5f45")
        {
            try
            {
                byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {

                //LogHelper.WriteException(ex.Message, source: "Quanika");
                return string.Empty;
            }
        }  //

        //If you change any thing in this fucntion then you also change in HelperDAL class in DAL project
        public static string Decrypt(string input, string key = "lkaju-x3h4-f5f45", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] long callerLineNumber = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(input)) return string.Empty;

                byte[] inputArray = Convert.FromBase64String(input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                //LogHelper.WriteException(ex.Message, source: "Quanika");
                return string.Empty;
            }

        }

        public static void AddValue(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
            config.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Minimal);
        }
        public static string getValue(string key)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(
                                        System.Reflection.Assembly.GetExecutingAssembly().Location);
                return config.AppSettings.Settings[key].Value;
            }
            catch (Exception ex)
            {
                //LogHelper.WriteException(ex.Message, source: "Quanika");
                return "";
            }
        }

        public static String HexConverter(Color c)
        {
            String rtn = String.Empty;
            try
            {
                rtn = "#FF" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
            }
            catch (Exception ex)
            {
                //LogHelper.WriteException(ex.Message.ToString(), source: "Quanika");
            }
            return rtn;
        }

        //public static BlockingCollection<EventPool> EventPools { get; set; } = new BlockingCollection<EventPool>();




        //public static async void AddEventToPool(string source)
        //{
        //    try
        //    {
        //        await Task.Run(() =>
        //        {

        //            if (source.Contains("["))
        //            {
        //                var eventList = JsonConvert.DeserializeObject<List<EventLogs>>(source);
        //                if (eventList != null && eventList.Count > 0)
        //                {

        //                    EventPools.Add(new Helpers.EventPool { Events = eventList });
        //                }
        //            }

        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteException(ex.Message, customMessage: "Crashed due to Json Conversion");
        //    }
        //}



     
        public static string GetPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static int GetHowManyTimeOccurenceCharInString(string text, char c)
        {
            int count = 0;
            foreach (char ch in text)
            {
                if (ch.Equals(c))
                {
                    count++;
                }
            }
            return count;
        }

        public static void CenterWindowOnScreen(Window yourParentWindow)
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = yourParentWindow.Width;
            double windowHeight = yourParentWindow.Height;
            yourParentWindow.Left = (screenWidth / 2) - (windowWidth / 2);
            yourParentWindow.Top = (screenHeight / 2) - (windowHeight / 2);
        }              
       
        public static void ExportToCsv(DataGrid dgDisplay, string pathOfCSVWithFileName)
        {
            int h = 0;
            h = dgDisplay.Items.Count;
            dgDisplay.SelectionMode = DataGridSelectionMode.Extended;
            dgDisplay.SelectAllCells();
            dgDisplay.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dgDisplay);
            dgDisplay.UnselectAllCells();
            String result = (string)Clipboard.GetData(System.Windows.DataFormats.CommaSeparatedValue);
            File.AppendAllText(pathOfCSVWithFileName, result, UnicodeEncoding.UTF8);
            dgDisplay.SelectionMode = DataGridSelectionMode.Single;
        }
       
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }       
      

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null)
                yield break;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child != null && child is T)
                {
                    yield return (T)child;
                }

                foreach (T childOfChild in FindVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }

        public static byte[] MakeThumbnail(byte[] myImage, int thumbWidth, int thumbHeight)
        {
            using (MemoryStream ms = new MemoryStream())
            using (System.Drawing.Image thumbnail = System.Drawing.Image.FromStream(new MemoryStream(myImage)).GetThumbnailImage(thumbWidth, thumbHeight, null, new IntPtr()))
            {
                thumbnail.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        
        /// <summary>
        /// Will Check The Sent Model is A1001 or Not
        /// </summary>
        /// <param name="controllerModel"></param>
        /// <returns></returns>
        public static bool IsA1001(string controllerModel)
        {
            return controllerModel.Equals("A1001", StringComparison.InvariantCultureIgnoreCase);
        }

        public static string GetDemoVendorPath(int Index, string CamId = "")
        {
            string path = "C:\\";
            try
            {
                if (!string.IsNullOrEmpty(CamId))
                {
                    Index = Convert.ToInt16(System.Text.RegularExpressions.Regex.Replace(CamId, "[^0-9]+", string.Empty));
                }
                string combineIndex = "DemoVideo" + Index;
                string res = Helper.GetPath() + Helper.getValue(combineIndex);
                if (!string.IsNullOrEmpty(res))
                {
                    path = res;
                }
            }
            catch (Exception ex)
            {

            }
            return path;
        }

        
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }
        public static bool WindowActive<T>(string name = "") where T : Window
        {
            Window win = Application.Current.Windows.OfType<T>().FirstOrDefault();
            win.WindowState = WindowState.Normal;
            win.Activate();
            return true;
        }

        public static bool WindowCLose<T>(string name = "") where T : Window
        {
            try
            {
                Window win = Application.Current.Windows.OfType<T>().FirstOrDefault();
                win.Close();
            }
            catch
            {
            }
            return true;
        }



        public static bool IsIpAddressRange(string fromIpAddress, string toIpAddress)
        {
            try
            {
                return StringToIPAddress(toIpAddress) < StringToIPAddress(fromIpAddress);
            }
            catch (Exception)
            {
                return false;
            }

        }
        static private uint StringToIPAddress(string IPAddr)
        {
            System.Net.IPAddress oIP = System.Net.IPAddress.Parse(IPAddr);
            byte[] byteIP = oIP.GetAddressBytes();
            uint ip = (uint)byteIP[0] << 24;
            ip += (uint)byteIP[1] << 16;
            ip += (uint)byteIP[2] << 8;
            ip += (uint)byteIP[3];

            return ip;
        }


       

        public static string IOControllerToken(string type)
        {
            string result = "";
            try
            {
                var uuid = Guid.NewGuid().ToString("D");
                var uuidSplit = uuid.Split('-');
                switch (type)
                {
                    case "Token":
                        result = $"io-{uuidSplit[uuidSplit.Length - 1]}";
                        break;
                    case "UUID":
                        result = uuid;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteException(ex.Message, "Quanika");
            }
            return result;
        }

        public static void ClearIpAddressBorders(List<TextBox> list)
        {
            try
            {
                foreach (var item in list)
                {
                    item.ToolTip = null;
                    item.BorderBrush = Brushes.Transparent;
                }
            }
            catch (Exception ex)
            {
               // LogHelper.WriteException(ex.Message, "Quanika");
            }
        }

      

    }
}
