using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace SMS.Converter
{
  
  public class BooleanToVisibilityConverter : IValueConverter
   {
       public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
       {
           if (value is Boolean && (bool)value)
           {
               return Visibility.Visible;
           }
           return Visibility.Collapsed;
       }

       public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
       {
           if (value is Visibility && (Visibility)value == Visibility.Visible)
           {
               return true;
           }
           return false;
       }
   }

  public class PositionToVisibilityConverter : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          switch(MainWindow.examAdminPanel.position_visibility)
          {
              case "Y":
                {
                    return Visibility.Visible;
                }
              case "N":
                {
                    return Visibility.Hidden;
                }                
          }
          return Visibility.Hidden;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          if (value is Visibility && (Visibility)value == Visibility.Visible)
          {
              return true;
          }
          return false;
      }   
  }

  public class AttendanceToVisibilityConverter : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          switch (MainWindow.examAdminPanel.attendance_visibility)
          {
              case "Y":
                  {
                      return Visibility.Visible;
                  }
              case "N":
                  {
                      return Visibility.Hidden;
                  }
          }
          return Visibility.Hidden;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          if (value is Visibility && (Visibility)value == Visibility.Visible)
          {
              return true;
          }
          return false;
      }
  }

  public class ImageToVisibilityConverter : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          switch (MainWindow.examAdminPanel.image_visibility)
          {
              case "Y":
                  {
                      return Visibility.Visible;
                  }
              case "N":
                  {
                      return Visibility.Hidden;
                  }
          }
          return Visibility.Hidden;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          if (value is Visibility && (Visibility)value == Visibility.Visible)
          {
              return true;
          }
          return false;
      }
  }

  public class RemarksToVisibilityConverter : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          switch (MainWindow.examAdminPanel.remarks_visibility)
          {
              case "Y":
                  {
                      return Visibility.Visible;
                  }
              case "N":
                  {
                      return Visibility.Hidden;
                  }
          }
          return Visibility.Hidden;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          if (value is Visibility && (Visibility)value == Visibility.Visible)
          {
              return true;
          }
          return false;
      }

  }


  public class TeachersToVisibilityConverter : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          switch (MainWindow.examAdminPanel.teacher_visibility)
          {
              case "Y":
                  {
                      return Visibility.Visible;
                  }
              case "N":
                  {
                      return Visibility.Hidden;
                  }
          }
          return Visibility.Hidden;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          if (value is Visibility && (Visibility)value == Visibility.Visible)
          {
              return true;
          }
          return false;
      }

  }

  public class ParentsToVisibilityConverter : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          switch (MainWindow.examAdminPanel.parents_visibility)
          {
              case "Y":
                  {
                      return Visibility.Visible;
                  }
              case "N":
                  {
                      return Visibility.Hidden;
                  }
          }
          return Visibility.Hidden;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          if (value is Visibility && (Visibility)value == Visibility.Visible)
          {
              return true;
          }
          return false;
      }

  }

  public class PrincipalToVisibilityConverter : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          switch (MainWindow.examAdminPanel.principal_visibility)
          {
              case "Y":
                  {
                      return Visibility.Visible;
                  }
              case "N":
                  {
                      return Visibility.Hidden;
                  }
          }
          return Visibility.Hidden;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          if (value is Visibility && (Visibility)value == Visibility.Visible)
          {
              return true;
          }
          return false;
      }

  }
}
