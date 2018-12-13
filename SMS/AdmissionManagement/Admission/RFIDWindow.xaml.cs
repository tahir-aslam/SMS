using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SMS.Models;
using System.Diagnostics;
using SMS.DAL;

namespace SMS.AdmissionManagement.Admission
{
    /// <summary>
    /// Interaction logic for RFIDWindow.xaml
    /// </summary>
    public partial class RFIDWindow : Window
    {
        admission adm_obj;
        string textInput = "";
        RfidDAL rfidDAL;

        public RFIDWindow(admission obj)
        {
            InitializeComponent();
            rfidDAL = new RfidDAL();
            this.adm_obj = obj;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.Key.ToString());
            string input = e.Key.ToString();

            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrEmpty(textInput) && !string.IsNullOrWhiteSpace(textInput) && textInput.Length >= 5)
                {
                    try
                    {
                        rfid_assignment obj = new rfid_assignment()
                        {
                            session_id = Convert.ToInt32(MainWindow.session.id),
                            card_holder_id = Convert.ToInt32(adm_obj.id),
                            card_no = textInput,
                            created_by = MainWindow.emp_login_obj.emp_user_name,
                            date_time = DateTime.Now,
                            emp_id = Convert.ToInt32(MainWindow.emp_login_obj.id),
                            is_std = "Y",
                        };

                        if (rfidDAL.InserRfidCard(obj) > 0)
                        {
                            v_register.Visibility = Visibility.Visible;
                            v_alreadyregister.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            v_register.Visibility = Visibility.Collapsed;
                            v_alreadyregister.Visibility = Visibility.Visible;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToLower().Contains("duplicate entry"))
                        {
                            v_register.Visibility = Visibility.Collapsed;
                            v_alreadyregister.Visibility = Visibility.Visible;
                        }
                        else 
                        {
                            MessageBox.Show(ex.Message);
                        }
                        Debug.WriteLine(ex);
                        
                    }
                }
                else 
                {
                    MessageBox.Show("Invalid RFID No");
                }
            }
            else
            {
                if (input.Contains("Number"))
                {
                    input = input.Substring(6);
                    textInput = textInput + input;
                }
                if (input.Contains("D"))
                {
                    input = input.Substring(1);
                    textInput = textInput + input;
                }
                if (input.Equals(189))
                {
                    input = "-";
                    textInput = textInput + input;
                }

            }
        }
    }
}
