﻿#pragma checksum "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "ACCA6425AC54333E2F5BFCB614E6E63447F20855E0C68302039B73F2BF2875BE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Reporting.WinForms;
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


namespace SMS.StudentManagement.AttendanceReportClassWise {
    
    
    /// <summary>
    /// AttendanceReportClassWisePage
    /// </summary>
    public partial class AttendanceReportClassWisePage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 59 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker date_picker_to;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker date_picker_from;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox class_cmb;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox section_cmb;
        
        #line default
        #line hidden
        
        
        #line 132 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button generate_report_btn;
        
        #line default
        #line hidden
        
        
        #line 144 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Forms.Integration.WindowsFormsHost windowsFormsHost1;
        
        #line default
        #line hidden
        
        
        #line 146 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Reporting.WinForms.ReportViewer _reportViewer1;
        
        #line default
        #line hidden
        
        
        #line 151 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid adm_grid;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/studentmanagement/attendancereportclasswise/attendancereportclassw" +
                    "isepage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
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
            this.date_picker_to = ((System.Windows.Controls.DatePicker)(target));
            
            #line 61 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
            this.date_picker_to.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.date_picker_to_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.date_picker_from = ((System.Windows.Controls.DatePicker)(target));
            
            #line 66 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
            this.date_picker_from.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.date_picker_from_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.class_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 84 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
            this.class_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.class_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.section_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 117 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
            this.section_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.section_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.generate_report_btn = ((System.Windows.Controls.Button)(target));
            
            #line 133 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
            this.generate_report_btn.Click += new System.Windows.RoutedEventHandler(this.generate_report_btn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.windowsFormsHost1 = ((System.Windows.Forms.Integration.WindowsFormsHost)(target));
            return;
            case 7:
            this._reportViewer1 = ((Microsoft.Reporting.WinForms.ReportViewer)(target));
            return;
            case 8:
            this.adm_grid = ((System.Windows.Controls.DataGrid)(target));
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
            case 9:
            
            #line 167 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked);
            
            #line default
            #line hidden
            
            #line 168 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked);
            
            #line default
            #line hidden
            break;
            case 10:
            
            #line 176 "..\..\..\..\..\StudentManagement\AttendanceReportClassWise\AttendanceReportClassWisePage.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Click += new System.Windows.RoutedEventHandler(this.CheckBox_Checked_sub);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

