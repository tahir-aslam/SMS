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
using System.Collections.ObjectModel;

namespace SMS.EmployeeManagement.UnitTest
{
    /// <summary>
    /// Interaction logic for UnitTestPage.xaml
    /// </summary>
    public partial class UnitTestPage : Page
    {
        ObservableCollection<CustomItem> OC { get; set; }
        ObservableCollection<CustomItem> ChildOC { get; set; }

        public UnitTestPage()
        {
            OC = new ObservableCollection<CustomItem>()
                {
                      new CustomItem(){Name="Item1", Checked=false,
                      Children = new ObservableCollection<CustomItem>()
                      {            
                        new CustomItem(){Name="SubItem11", Checked=false},
                        new CustomItem(){Name="SubItem12", Checked=false},
                        new CustomItem(){Name="SubItem13", Checked=false}
                      }},
                      new CustomItem(){Name="Item2", Checked=false,
                      Children = new ObservableCollection<CustomItem>()
                      {
                        new CustomItem(){Name="SubItem21", Checked=false},            
                        new CustomItem(){Name="SubItem22", Checked=false},
                        new CustomItem(){Name="SubItem23", Checked=false}
                      }},
                     new CustomItem(){Name="Item3", Checked=false,
                      Children = new ObservableCollection<CustomItem>()
                      {
                        new CustomItem(){Name="SubItem31", Checked=false},
                        new CustomItem(){Name="SubItem32", Checked=false},            
                        new CustomItem(){Name="SubItem33", Checked=false}
                      }},
                    new CustomItem(){Name="Item4", Checked=false,
                      Children = new ObservableCollection<CustomItem>()
                      {
                        new CustomItem(){Name="SubItem41", Checked=false},
                        new CustomItem(){Name="SubItem42", Checked=false},
                        new CustomItem(){Name="SubItem43", Checked=false}
                      }
                      }};
      
            InitializeComponent();
            this.DataContext = OC;

            
        }

        // On Check

        public void OnCheck()
        {
            ChildOC = new ObservableCollection<CustomItem>() { };
            foreach (CustomItem item in OC)
            {
                if (item.Checked == true)
                {
                    ChildOC.Add(item);
                    foreach (CustomItem subitem in item.Children)
                    {
                        subitem.Checked = true;

                        if (subitem.Checked == true)
                        {
                            ChildOC.Add(subitem);
                        }
                    }
                }                
            }
            
            //listbox.ItemsSource = ChildOC;
        }
            private void CheckBox_Click(object sender, RoutedEventArgs e)
            {
                OnCheck();
                treeView.Items.Refresh();               
                //var checkbox = sender as CheckBox;
                //if (null == checkbox) return;
                //foreach( CustomItem ci in OC)
                //{
                //    ci.Checked = checkbox.IsChecked.Value;
                //}

                
                
               
            }
            private void CheckBox_Loaded(object sender, RoutedEventArgs e) 
            {
                OnCheck();
            }
       }

    public class CustomItem
    {
        public string Name { get; set; }
        public bool Checked { get; set; }
        public ObservableCollection<CustomItem> Children { get; set; }
    }
}
