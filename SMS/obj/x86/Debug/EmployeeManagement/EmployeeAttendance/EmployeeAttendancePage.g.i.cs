﻿#pragma checksum "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "FB27F462BA2CFF8D902F3BC837F582DAEA53C71CAD792A7E2120102409A4C04E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace SMS.EmployeeManagement.EmployeeAttendance {
    
    
    /// <summary>
    /// EmployeeAttendancePage
    /// </summary>
    public partial class EmployeeAttendancePage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 112 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox emp_types_cmb;
        
        #line default
        #line hidden
        
        
        #line 124 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker attendnce_date;
        
        #line default
        #line hidden
        
        
        #line 126 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button att_button;
        
        #line default
        #line hidden
        
        
        #line 127 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button edit_button;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button delete_button;
        
        #line default
        #line hidden
        
        
        #line 137 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid attendence_grid;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SMS;component/employeemanagement/employeeattendance/employeeattendancepage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            ((SMS.EmployeeManagement.EmployeeAttendance.EmployeeAttendancePage)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.emp_types_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 112 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            this.emp_types_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.emp_types_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.attendnce_date = ((System.Windows.Controls.DatePicker)(target));
            
            #line 124 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            this.attendnce_date.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.attendnce_date_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.att_button = ((System.Windows.Controls.Button)(target));
            
            #line 126 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            this.att_button.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.edit_button = ((System.Windows.Controls.Button)(target));
            
            #line 127 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            this.edit_button.Click += new System.Windows.RoutedEventHandler(this.edit_button_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.delete_button = ((System.Windows.Controls.Button)(target));
            
            #line 128 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            this.delete_button.Click += new System.Windows.RoutedEventHandler(this.delete_button_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.attendence_grid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 137 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            this.attendence_grid.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.att_grid_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 8:
            
            #line 143 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked);
            
            #line default
            #line hidden
            
            #line 143 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked);
            
            #line default
            #line hidden
            break;
            case 9:
            
            #line 149 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Click += new System.Windows.RoutedEventHandler(this.CheckBox_Checked_sub);
            
            #line default
            #line hidden
            break;
            case 10:
            
            #line 157 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked_isAbsent);
            
            #line default
            #line hidden
            
            #line 157 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked_isAbsent);
            
            #line default
            #line hidden
            break;
            case 11:
            
            #line 163 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Click += new System.Windows.RoutedEventHandler(this.CheckBox_Checked_sub_isAbsent);
            
            #line default
            #line hidden
            break;
            case 12:
            
            #line 171 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked_isLeave);
            
            #line default
            #line hidden
            
            #line 171 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked_isLeave);
            
            #line default
            #line hidden
            break;
            case 13:
            
            #line 177 "..\..\..\..\..\EmployeeManagement\EmployeeAttendance\EmployeeAttendancePage.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Click += new System.Windows.RoutedEventHandler(this.CheckBox_Checked_sub_isLeave);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

