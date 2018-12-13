using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using SMS.Models;
using SMS.ExamManagement.ExamDataEntry;
using MySql.Data.MySqlClient;

namespace SMS.ViewModels
{

    public class ExamReportBySubjectVM : INotifyPropertyChanged
    {
        List<admission> adm_list;
        List<exam_data_entry> ede_exam_list;
        List<exam_data_entry> ede_list;
        exam_data_entry ede_obj;
        List<exam> exam_list_new;
        int i = 0;

        public ExamReportBySubjectVM(List<exam_data_entry> lst, List<exam> exam_list)
        {
            PopulateExam(lst, exam_list);            
        }
     
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<exam_data_entry> _examList;
        public ObservableCollection<exam_data_entry> examList
        {
            get
            {
                return _examList;
            }
            set
            {
                if (_examList != value)
                {
                    _examList = value;
                    OnPropertyChanged("examList");
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
     
        //----------------------------------------
        public void PopulateExam(List<exam_data_entry> lst, List<exam> exam_list)
        {
            //get_all_admissions("3");
            //get_all_exams_entry();
            //set_exam_data_entry_list();

            examList = new ObservableCollection<exam_data_entry>();
            TitleList = new List<string>();
            i = 0;

            foreach (exam_data_entry ede in lst)
            {
                examList.Add(ede);
            }

            //Title List
            foreach (exam ede in exam_list)
            {                
                  TitleList.Add(ede.exam_name.ToString());                
            }
        }
       
    }
}
