﻿#pragma checksum "..\..\..\..\..\EmployeeManagement\EmpLogin\EmployeeLoginForm.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0A78D9DDFC9CF1346B9ADE70CBDE79AD0BCA20A9"
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


namespace SMS.EmployeeManagement.EmpLogin {
    
    
    /// <summary>
    /// EmployeeLoginForm
    /// </summary>
    public partial class EmployeeLoginForm : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\..\..\..\EmployeeManagement\EmpLogin\EmployeeLoginForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox emp_types_cmb;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\..\EmployeeManagement\EmpLogin\EmployeeLoginForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox emp_cmb;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\..\..\EmployeeManagement\EmpLogin\EmployeeLoginForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox user_name_textbox;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\..\..\EmployeeManagement\EmpLogin\EmployeeLoginForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox pwd_textbox;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\..\..\EmployeeManagement\EmpLogin\EmployeeLoginForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox re_pwd_textbox;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/employeemanagement/emplogin/employeeloginform.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\EmployeeManagement\EmpLogin\EmployeeLoginForm.xaml"
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
            
            #line 4 "..\..\..\..\..\EmployeeManagement\EmpLogin\EmployeeLoginForm.xaml"
            ((SMS.EmployeeManagement.EmpLogin.EmployeeLoginForm)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.emp_types_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 32 "..\..\..\..\..\EmployeeManagement\EmpLogin\EmployeeLoginForm.xaml"
            this.emp_types_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.emp_types_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.emp_cmb = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.user_name_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.pwd_textbox = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 6:
            this.re_pwd_textbox = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 7:
            
            #line 79 "..\..\..\..\..\EmployeeManagement\EmpLogin\EmployeeLoginForm.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_save);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 81 "..\..\..\..\..\EmployeeManagement\EmpLogin\EmployeeLoginForm.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_cancel);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

