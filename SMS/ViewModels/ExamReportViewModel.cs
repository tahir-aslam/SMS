using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMS.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SMS.ViewModels
{
    public class ExamReportViewModel : INotifyPropertyChanged
    {
        List<exam_data_entry> ede_list;
        List<exam> exam_list;
        int i = 0;

      public ExamReportViewModel(List<exam_data_entry> ede_list1, List<exam> exam_list1)
      {
          ede_list = new List<exam_data_entry>();
          exam_list = new List<exam>();

          this.ede_list = ede_list1;
          this.exam_list = exam_list1;
          PopulateStudents();
      }

      public event PropertyChangedEventHandler PropertyChanged;
      private ObservableCollection<exam_data_entry> _attList;
      public ObservableCollection<exam_data_entry> attList
      {
          get
          {
              return _attList;
          }
          set
          {
              if (_attList != value)
              {
                  _attList = value;
                  OnPropertyChanged("attList");
              }
          }
      }

      private List<string> _titleList;
      public List<string> TitleList
      {
          get
          {
              return _titleList;
          }
          set
          {
              if (_titleList != value)
              {
                  _titleList = value;
                  OnPropertyChanged("TitleList");
              }
          }
      }
      private void OnPropertyChanged(string propertyName)
      {
          if (PropertyChanged != null)

              PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }



      //Populate student attendance
      public void PopulateStudents() 
      {
          attList = new ObservableCollection<exam_data_entry>();
          TitleList = new List<string>();
          i = 0;
          foreach (exam_data_entry ede in ede_list)
          {
              attList.Add(ede);
          }

          //Title List
          foreach (exam exa in exam_list.Where(x=>x.Checked==true))
          {                 
              TitleList.Add(exa.exam_name.ToString());
          }
      }
    }
}
