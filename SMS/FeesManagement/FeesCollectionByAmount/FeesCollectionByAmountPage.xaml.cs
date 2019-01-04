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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using SMS.FeeManagement.FeePaidByAmount;
using System.ComponentModel;
using SMS.Models;
using SMS.DAL;

namespace SMS.FeesManagement.FeesCollectionByAmount
{
    /// <summary>
    /// Interaction logic for FeesCollectionByAmountPage.xaml
    /// </summary>
    public partial class FeesCollectionByAmountPage : Page
    {
        FeesDAL feesDAL;
        ClassesDAL classDAL;
        MiscDAL miscDAL;
        AdmissionDAL admDAL;

        admission obj;
        List<admission> adm_list;
        List<classes> classes_list;
        List<sections> sections_list;          
        FeesCollectionByAmountWindow window;

        public FeesCollectionByAmountPage()
        {
            InitializeComponent();

            feesDAL = new FeesDAL();
            classDAL = new ClassesDAL();
            miscDAL = new MiscDAL();
            admDAL = new AdmissionDAL();

            adm_list = new List<admission>();
            load_grid();
        }

        public void load_grid()
        {
            try
            {
                adm_list = admDAL.get_all_admissions();
                classes_list = classDAL.get_all_classes();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            adm_grid.ItemsSource = adm_list;

            classes_list.Insert(0, new classes() { id = "-1", class_name = "--All Class--" });
            class_cmb.ItemsSource = classes_list;
            class_cmb.SelectedIndex = 0;

            strength_textblock.Text = adm_grid.Items.Count.ToString();            
        }       

        //-------------     Editing          ---------------------------
        private void click_edit(object sender, RoutedEventArgs e)
        {
            editing();
        }
        private void fee_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editing();
        }

        public void editing()
        {
            obj = (admission)adm_grid.SelectedItem;
            if (obj == null)
            {
                // MessageBox.Show("plz select a row");
            }
            else
            {
                window = new FeesCollectionByAmountWindow(obj);
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
            }
        }


        private void click_delete(object sender, RoutedEventArgs e)
        {

        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {
        }
        //============      Classes Selection Change       ===============================

        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            classes c = (classes)class_cmb.SelectedItem;
            string id = c.id;

            if (class_cmb.SelectedIndex != 0)
            {
                sections_list = new List<sections>();
                sections_list = classDAL.get_all_sections(c.id);
                sections_list.Insert(0, new sections() { id = "-1", section_name = "--All Sections--" });
                section_cmb.ItemsSource = sections_list;
                section_cmb.SelectedIndex = 0;

                section_cmb.IsEnabled = true;
               
            }
            else
            {
                section_cmb.IsEnabled = false;
                section_cmb.SelectedIndex = 0;
            }
        }
               
        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sections sec = (sections)section_cmb.SelectedItem;
            if (sec != null)
            {
                string id = sec.id;
                if (section_cmb.SelectedIndex != 0)
                {
                    adm_grid.ItemsSource = adm_list.Where(x => x.section_id == id);
                }
                else
                {
                    adm_grid.ItemsSource = null;
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }

        public void search_box()
        {
            string v_search = SearchTextBox.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(adm_grid.ItemsSource);
            if (v_search == null)
            {
                cv.Filter = null;
            }
            else
            {
                cv.Filter = o =>
                {
                    admission adm = o as admission;

                    if (search_cmb.SelectedIndex == 0)
                    {
                        return (adm.std_name.ToUpper().StartsWith(v_search.ToUpper()) || adm.std_name.ToUpper().Contains(v_search.ToUpper()));
                    }
                    else if (search_cmb.SelectedIndex == 1)
                    {
                        return (adm.father_name.ToUpper().StartsWith(v_search.ToUpper()) || adm.father_name.ToUpper().Contains(v_search.ToUpper()));
                    }
                    if (search_cmb.SelectedIndex == 2)
                    {
                        return (adm.adm_no.ToUpper().StartsWith(v_search.ToUpper()) || adm.adm_no.ToUpper().Contains(v_search.ToUpper()));
                    }                 
                    else if (search_cmb.SelectedIndex == 3)
                    {
                        return (adm.cell_no.ToUpper().StartsWith(v_search.ToUpper()) || adm.cell_no.ToUpper().Contains(v_search.ToUpper()));
                    }
                    else
                    {
                        return true;
                    }
                };                
            }
            SearchTextBox.Focus();
        }        

        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(adm_grid, itemsSourceChanged);
            }
        }

        private void itemsSourceChanged(object sender, EventArgs e)
        {
            strength_textblock.Text = adm_grid.Items.Count.ToString();
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SearchTextBox.IsFocused == true)
                {
                    adm_grid.SelectedIndex = 0;
                    editing();
                }
            }
        }

        private void search_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTextBox.Focus();
        }
    }
}
