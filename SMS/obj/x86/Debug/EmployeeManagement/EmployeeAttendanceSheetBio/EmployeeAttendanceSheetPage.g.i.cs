﻿#pragma checksum "..\..\..\..\..\EmployeeManagement\EmployeeAttendanceSheetBio\EmployeeAttendanceSheetPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "432EAFE07A03D29D036F268BA1E6000D49D59FE542900A18158620536B8E7C1C"
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


namespace SMS.EmployeeManagement.EmployeeAttendanceSheet {
    
    
    /// <summary>
    /// EmployeeAttendanceSheetPage
    /// </summary>
    public partial class EmployeeAttendanceSheetPage : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 59 "..\..\..\..\..\EmployeeManagement\EmployeeAttendanceSheetBio\EmployeeAttendanceSheetPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox month_cmb;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\..\..\EmployeeManagement\EmployeeAttendanceSheetBio\EmployeeAttendanceSheetPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox year_cmb;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\..\..\..\EmployeeManagement\EmployeeAttendanceSheetBio\EmployeeAttendanceSheetPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Forms.Integration.WindowsFormsHost windowsFormsHost3;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\..\..\..\EmployeeManagement\EmployeeAttendanceSheetBio\EmployeeAttendanceSheetPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Reporting.WinForms.ReportViewer _reportViewer3;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/employeemanagement/employeeattendancesheetbio/employeeattendancesh" +
                    "eetpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\EmployeeManagement\EmployeeAttendanceSheetBio\EmployeeAttendanceSheetPage.xaml"
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
            this.month_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 61 "..\..\..\..\..\EmployeeManagement\EmployeeAttendanceSheetBio\EmployeeAttendanceSheetPage.xaml"
            this.month_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.month_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.year_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 78 "..\..\..\..\..\EmployeeManagement\EmployeeAttendanceSheetBio\EmployeeAttendanceSheetPage.xaml"
            this.year_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.year_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.windowsFormsHost3 = ((System.Windows.Forms.Integration.WindowsFormsHost)(target));
            return;
            case 4:
            this._reportViewer3 = ((Microsoft.Reporting.WinForms.ReportViewer)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

