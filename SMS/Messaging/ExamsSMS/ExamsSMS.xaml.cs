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
using SMS.Models;
using MySql.Data.MySqlClient;
using SMS.Upload;
using System.Collections.ObjectModel;
using SMS.ViewModels;
using SUT.PrintEngine.Utils;
using System.Windows.Markup;
using System.Data;
using SMS.Messaging.BrandedSms;
using SMS.Messaging.SmsOption;
using System.ComponentModel;
using SMS.DAL;

namespace SMS.Messaging.ExamsSMS
{
    /// <summary>
    /// Interaction logic for ExamsSMS.xaml
    /// </summary>
    public partial class ExamsSMS : Page
    {
        List<exam> exam_list;
        List<classes> classes_list;
        List<sections> sections_list;
        ObservableCollection<subjects> subjects_list;
        List<admission> adm_list;
        List<exam_data_entry> ede_list;
        public static List<exam_data_entry> ede_exam_list;
        exam_data_entry ede_obj;
        ExamViewModel evm;
        List<double> col_width;
        exam_data_entry ede_obj1;
        List<admission> std_nos;
        string exam_name;
        string class_name;
        public static bool isbranded = false;
        ExamsDAL examsDAL;

        public ExamsSMS()
        {
            InitializeComponent();
            examsDAL = new ExamsDAL();
            exam_list = examsDAL.get_all_exams();
            exam_cmb.Focus();
            exam_cmb.ItemsSource = exam_list;
            exam_list.Insert(0, new exam() { exam_name = "---Select Exam---", id = "-1" });
            exam_cmb.SelectedIndex = 0;

            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;
            strength_textblock.Text = exam_entry_grid.Items.Count.ToString();
            ede_exam_list = new List<exam_data_entry>();
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            foreach (var item in ede_exam_list)
            {
                item.Checked = checkBox.IsChecked.Value;
            }
            exam_entry_grid.Items.Refresh();

        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            exam_entry_grid.SelectedItem = e.Source;
            ede_obj1 = new exam_data_entry();
            ede_obj1 = (exam_data_entry)exam_entry_grid.SelectedItem;
            foreach (exam_data_entry ede in ede_exam_list)
            {
                if (ede_obj1.std_id == ede.std_id)
                {
                    ede.Checked = checkBox.IsChecked.Value;
                }
            }

        }
        // ---------------------- Exam Selection Changed ---------------------------
        private void exam_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (exam_cmb.SelectedIndex > 0)
            {
                class_cmb.IsEnabled = true;
                class_cmb.SelectedIndex = 0;
                exam exe = (exam)exam_cmb.SelectedItem;
                exam_name = exe.exam_name;
            }
            else
            {
                class_cmb.IsEnabled = false;
                class_cmb.SelectedIndex = 0;
            }

        }

        //------------------------  Class Selection Changed -------------------------
        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            classes c = (classes)class_cmb.SelectedItem;
            string id = c.id;

            if (class_cmb.SelectedIndex != 0)
            {
                section_cmb.IsEnabled = true;
                get_all_sections(id);
                sections_list.Insert(0, new sections() { section_name = "---Select Section---", id = "-1" });
                section_cmb.ItemsSource = sections_list;
                section_cmb.SelectedIndex = 0;
                classes cl = (classes)class_cmb.SelectedItem;
                class_name = cl.class_name;
            }
            else
            {
                section_cmb.IsEnabled = false;
                section_cmb.SelectedIndex = 0;
            }
        }

        // ----------------------  Sections Selection changed---------------------------
        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (section_cmb.SelectedIndex > 0)
            {
                sections s = (sections)section_cmb.SelectedItem;
                classes c = (classes)class_cmb.SelectedItem;
                exam ex = (exam)exam_cmb.SelectedItem;

                if (Convert.ToInt32(s.id) > 0)
                {
                    exam_img_grid.Visibility = Visibility.Hidden;

                    ede_exam_list = examsDAL.GetAllExamDataEntryBySection(Convert.ToInt32(MainWindow.session.id), Convert.ToInt32(ex.id), Convert.ToInt32(s.id));
                    exam_entry_grid.ItemsSource = ede_exam_list.GroupBy(x => x.std_id).Select(y => y.First()).ToList();
                    send_btn.Visibility = Visibility.Visible;
                    exam_entry_grid.Visibility = Visibility.Visible;
                }
            }
            else
            {
                exam_img_grid.Visibility = Visibility.Visible;
                exam_entry_grid.Visibility = Visibility.Collapsed;
            }

        }

        public void get_all_classes()
        {
            classes_list = new List<classes>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_classes";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            classes classes = new classes()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                            };
                            classes_list.Add(classes);

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //------------              Get All Sections   ------------------------
        public void get_all_sections(string id)
        {
            sections_list = new List<sections>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_subjects where class_id = " + id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sections section = new sections()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                section_name = Convert.ToString(reader["section_name"].ToString()),
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),


                            };
                            sections_list.Add(section);

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void send_btn_Click(object sender, RoutedEventArgs e)
        {
            admission adm;
            std_nos = new List<admission>();
            string message = "";
            foreach (exam_data_entry ede in exam_entry_grid.Items.OfType<exam_data_entry>().Where(x => x.Checked == true))
            {

                message = exam_name + " Results. Class " + class_name + ". ";
                message = message + ede.std_name + " has got " + ede.obtained_marks + "/" + ede.total_marks + " with " + ede.percentage + "%. ";
                foreach (exam_data_entry ede_subj in ede_exam_list.Where(x => x.std_id == ede.std_id))
                {
                    message = message + ede_subj.subject_name + "=" + ede_subj.subject_obtained + "/" + ede_subj.subject_total + ". ";
                }
                message = message + " Admin " + MainWindow.ins.institute_name + ". " + MainWindow.ins.institute_phone + " " + MainWindow.ins.institute_cell;

                //create sms object
                adm = new admission();
                adm.id = ede.std_id;
                adm.sms_message = message;
                adm.cell_no = ede.cell_no;
                adm.std_name = ede.std_name;
                adm.father_name = ede.father_name;
                adm.sms_type = "Exam Sms";                
                std_nos.Add(adm);
            }
            if (std_nos.Count > 0)
             {
                OptionWindow ow = new OptionWindow();
                ow.ShowDialog();

                if (ow.IsClosed == false)
                {
                    if (isbranded == true)
                    {
                        BrandedSmsEngine bse = new BrandedSmsEngine(std_nos);
                        bse.Show();
                    }
                    else
                    {
                        UploadWindow uw = new UploadWindow(std_nos, false);
                        uw.Show();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Select Minimum One Student");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(exam_entry_grid, itemsSourceChanged);
            }
        }

        private void itemsSourceChanged(object sender, EventArgs e)
        {
            strength_textblock.Text = exam_entry_grid.Items.Count.ToString();
        }
    }
}
