using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SMS.ViewModels
{
   public class HeaderViewModel 
    {
       public string name = "Tahir";
       

       public event PropertyChangedEventHandler PropertyChanged;
       private void OnPropertyChanged(string propertyName)
       {
           if (PropertyChanged != null)

               PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
       }

    }
}
