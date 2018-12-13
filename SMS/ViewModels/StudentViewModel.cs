using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMS.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SMS.ViewModels
{
  public class StudentViewModel:INotifyPropertyChanged
    {
      ObservableCollection<student_attendence> std_att;
      List<DateTime> group_by_att_dates;
      int i = 0;
      public StudentViewModel(ObservableCollection<student_attendence> lst, List<DateTime> group_by_lst)
      {
          std_att = new ObservableCollection<student_attendence>();
          group_by_att_dates = new List<DateTime>();
          this.group_by_att_dates = group_by_lst;
          this.std_att = lst;
          PopulateStudents();
      }

      public event PropertyChangedEventHandler PropertyChanged;
      private ObservableCollection<student_attendence> _attList;
      public ObservableCollection<student_attendence> attList
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
          attList = new ObservableCollection<student_attendence>();
          TitleList = new List<string>();
          i = 0;

          foreach (student_attendence std in std_att)
          {
              attList.Add(std);
          }

          //Title List
          foreach (DateTime dt in group_by_att_dates)
          {                 
              TitleList.Add(dt.ToString("dd MMM"));                               
          }
      }
    }
}
