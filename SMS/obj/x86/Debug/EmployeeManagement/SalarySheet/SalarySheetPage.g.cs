﻿#pragma checksum "..\..\..\..\..\EmployeeManagement\SalarySheet\SalarySheetPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "321D82C00CE9F6A45C1E6D798F28A8C7546C2866"
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


namespace SMS.EmployeeManagement.SalarySheet {
    
    
    /// <summary>
    /// SalarySheetPage
    /// </summary>
    public partial class SalarySheetPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 54 "..\..\..\..\..\EmployeeManagement\SalarySheet\SalarySheetPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox month_cmb;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\..\..\EmployeeManagement\SalarySheet\SalarySheetPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox year_cmb;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\..\..\EmployeeManagement\SalarySheet\SalarySheetPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox emp_types_cmb;
        
        #line default
        #line hidden
        
        
        #line 114 "..\..\..\..\..\EmployeeManagement\SalarySheet\SalarySheetPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Forms.Integration.WindowsFormsHost windowsFormsHost3;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\..\..\..\EmployeeManagement\SalarySheet\SalarySheetPage.xaml"
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/employeemanagement/salarysheet/salarysheetpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\EmployeeManagement\SalarySheet\SalarySheetPage.xaml"
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
            
            #line 22 "..\..\..\..\..\EmployeeManagement\SalarySheet\SalarySheetPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_refresh);
            
            #line default
            #line hidden
            return;
            case 2:
            this.month_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 54 "..\..\..\..\..\EmployeeManagement\SalarySheet\SalarySheetPage.xaml"
            this.month_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.month_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.year_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 65 "..\..\..\..\..\EmployeeManagement\SalarySheet\SalarySheetPage.xaml"
            this.year_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.year_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.emp_types_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 85 "..\..\..\..\..\EmployeeManagement\SalarySheet\SalarySheetPage.xaml"
            this.emp_types_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.emp_types_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.windowsFormsHost3 = ((System.Windows.Forms.Integration.WindowsFormsHost)(target));
            return;
            case 6:
            this._reportViewer3 = ((Microsoft.Reporting.WinForms.ReportViewer)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

