using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace SMS.Converter
{
    public class RowNumber : IMultiValueConverter
    {
        #region " Convert Function "
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Object item = values[0];
            DataGrid grdPassedGrid = values[1] as DataGrid;
            int intRowNumber = grdPassedGrid.Items.IndexOf(item) + 1;
            return intRowNumber.ToString().PadLeft(2, '0');
        }
        #endregion

        #region " ConvertBack Function "
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
