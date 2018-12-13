using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using SMS.Models;
using MySql.Data.MySqlClient;

namespace SMS.ViewModels
{
  public  class EmpViewModel:INotifyPropertyChanged
    {

     public List<char> employee_attendence_list;
     public static ObservableCollection<employees> emp_vm_list;   
     public static List<employees> emp_list;
     public List<employees> emp_attendence_list;
     public List<DateTime> emp_attendence_date_list;
     public List<DateTime> group_by_att_dates;
     public static ObservableCollection<employees> final_list;
     
     bool check = false;

      public EmpViewModel()
      {
          PopulateEmployees();
      }

        public event PropertyChangedEventHandler PropertyChanged;
        

        private ObservableCollection<employees> _empList;
        public ObservableCollection<employees> empList
        {
            get
            {
                return _empList;
            }
            set
            {
                if (_empList != value)
                {
                    _empList = value;
                    OnPropertyChanged("empList");
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

        public void PopulateEmployees()
        {       
            get_all_emp();
            get_all_emp_attendence();            
            populate_emp_vm_list();

            var varEmpList = new ObservableCollection<employees>();
            foreach (employees emp in emp_vm_list)
            {                
                varEmpList.Add(emp);
            }
            empList = new ObservableCollection<employees>();
            foreach(employees emp1 in varEmpList)
            {
                //emp1.Active = false;
                empList.Add(emp1);
            }
            
           // empList = varEmpList;
           // empList = emp_list;

            //final_list = empList;

            TitleList = new List<string>();
            foreach(DateTime d in group_by_att_dates)
            {
                TitleList.Add(d.Date.ToString("dd MMM"));
            }            
        }

      

      //-----------------           Populate employee view model list----------------------
        public void populate_emp_vm_list()
        {
            emp_vm_list = new ObservableCollection<employees>();

            foreach(employees emp in emp_list)
            {
                employees emp_vm = new employees() 
                {
                    id = emp.id,
                    emp_name = emp.emp_name,
                    emp_type = emp.emp_type,
                    Active= false
                };

                employee_attendence_list = new List<char>();
                emp_attendence_date_list = new List<DateTime>();

                foreach(employees emp_att in emp_attendence_list.Where(x=>x.id == emp.id))
                {
                    for (int i = 0; i < group_by_att_dates.Count ; i++ ) 
                    {
                        if (emp_att.attendence_date == group_by_att_dates[i])
                        {
                            employee_attendence_list.Insert(i,emp_att.attendence); 
                            emp_attendence_date_list.Insert(i,emp_att.attendence_date);

                            break;

                        }
                        else 
                        {
                            if (emp_attendence_date_list.Count <= i)
                            {
                                employee_attendence_list.Insert(i, '-');
                                emp_attendence_date_list.Insert(i, emp_att.attendence_date);                                
                            }
                        }
                    }
                }
                emp_vm.att_lst = employee_attendence_list;
                emp_vm.att_date_lst = emp_attendence_date_list;
                emp_vm_list.Add(emp_vm);                
            }
        }

        //---------------           Get All Employees    ----------------------------------
        public void get_all_emp()
        {
            try
            {
                emp_list = new List<employees>();

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_emp where is_active='Y'";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            employees emp = new employees()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                emp_type = Convert.ToString(reader["emp_type"].ToString()),
                                emp_type_id = Convert.ToString(reader["emp_type_id"].ToString()),
                                Active = false
                            };
                            emp_list.Add(emp);

                        }


                    }
                }
            }
            catch(Exception ex)
            {
                
            }
        }

        //==============      Get All Employee attendence        ==============================
        public void get_all_emp_attendence()
        {
            try
            {
                emp_attendence_list = new List<employees>();
                group_by_att_dates = new List<DateTime>();
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_emp_attendence ORDER BY attendence_date DESC ";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            employees emp = new employees()
                            {
                                id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                emp_type = Convert.ToString(reader["emp_type"].ToString()),
                                attendence = Convert.ToChar(reader["attendence"]),
                                attendence_date = Convert.ToDateTime(reader["attendence_date"]),
                            };
                            emp_attendence_list.Add(emp);

                            if (group_by_att_dates.Exists(x => x.Date == emp.attendence_date))
                            {
                            }
                            else 
                            {
                                group_by_att_dates.Add(emp.attendence_date);
                            }
                        }


                    }
                }
            }
            catch(Exception ex)
            {
                
            }
        }


    }

}
