﻿#pragma checksum "..\..\..\..\..\Messaging\EmpAttendanceSms\EmpAttendanceSmsPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E781721FDF940216F3BF4B717F07C3CAE85EFB41"
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


namespace SMS.Messaging.EmpAttendanceSms {
    
    
    /// <summary>
    /// EmpAttendanceSmsPage
    /// </summary>
    public partial class EmpAttendanceSmsPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 33 "..\..\..\..\..\Messaging\EmpAttendanceSms\EmpAttendanceSmsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock strength_textblock;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\..\Messaging\EmpAttendanceSms\EmpAttendanceSmsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker attendnce_date;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\..\..\Messaging\EmpAttendanceSms\EmpAttendanceSmsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid attendence_grid;
        
        #line default
        #line hidden
        
        
        #line 154 "..\..\..\..\..\Messaging\EmpAttendanceSms\EmpAttendanceSmsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox message_textbox;
        
        #line default
        #line hidden
        
        
        #line 157 "..\..\..\..\..\Messaging\EmpAttendanceSms\EmpAttendanceSmsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button send_btn;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/messaging/empattendancesms/empattendancesmspage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Messaging\EmpAttendanceSms\EmpAttendanceSmsPage.xaml"
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
            
            #line 9 "..\..\..\..\..\Messaging\EmpAttendanceSms\EmpAttendanceSmsPage.xaml"
            ((SMS.Messaging.EmpAttendanceSms.EmpAttendanceSmsPage)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.strength_textblock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.attendnce_date = ((System.Windows.Controls.DatePicker)(target));
            
            #line 51 "..\..\..\..\..\Messaging\EmpAttendanceSms\EmpAttendanceSmsPage.xaml"
            this.attendnce_date.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.date_picker_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.attendence_grid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 7:
            this.message_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.send_btn = ((System.Windows.Controls.Button)(target));
            
            #line 157 "..\..\..\..\..\Messaging\EmpAttendanceSms\EmpAttendanceSmsPage.xaml"
            this.send_btn.Click += new System.Windows.RoutedEventHandler(this.send_btn_Click);
            
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
            case 5:
            
            #line 62 "..\..\..\..\..\Messaging\EmpAttendanceSms\EmpAttendanceSmsPage.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked);
            
            #line default
            #line hidden
            
            #line 62 "..\..\..\..\..\Messaging\EmpAttendanceSms\EmpAttendanceSmsPage.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked);
            
            #line default
            #line hidden
            break;
            case 6:
            
            #line 68 "..\..\..\..\..\Messaging\EmpAttendanceSms\EmpAttendanceSmsPage.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Click += new System.Windows.RoutedEventHandler(this.CheckBox_Checked_sub);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

