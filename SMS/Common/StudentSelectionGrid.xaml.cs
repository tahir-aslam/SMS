using SMS.DAL;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SMS.Common
{
    /// <summary>
    /// Interaction logic for StudentSelectionGrid.xaml
    /// </summary>
    public partial class StudentSelectionGrid : UserControl
    {
        ClassesDAL classDAL;
        MiscDAL miscDAL;
        AdmissionDAL admDAL;

        admission obj;
        List<admission> adm_list;
        List<classes> classes_list;
        List<sections> sections_list;
        
        public StudentSelectionGrid()
        {
            InitializeComponent();

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

        private void click_refresh(object sender, RoutedEventArgs e)
        {
        }

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
        private void itemsSourceChanged(object sender, EventArgs e)
        {
            strength_textblock.Text = adm_grid.Items.Count.ToString();
        }
        private void search_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTextBox.Focus();
        }
    }
}
