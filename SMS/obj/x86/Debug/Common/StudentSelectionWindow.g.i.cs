﻿#pragma checksum "..\..\..\..\Common\StudentSelectionWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "6EAAF8497BC9DF3D197BC7E2DF1233CFD98FE9EA7A88395BD65FEFFFF8DE3BCC"
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


namespace SMS.Common {
    
    
    /// <summary>
    /// StudentSelectionWindow
    /// </summary>
    public partial class StudentSelectionWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 33 "..\..\..\..\Common\StudentSelectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SearchTextBox;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\Common\StudentSelectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox search_cmb;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\Common\StudentSelectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox class_cmb;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\Common\StudentSelectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox section_cmb;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\..\Common\StudentSelectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox month_cmb;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\..\Common\StudentSelectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker date_picker;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\..\Common\StudentSelectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid adm_grid;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\..\..\Common\StudentSelectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button proceed_btn;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/common/studentselectionwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Common\StudentSelectionWindow.xaml"
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
            
            #line 4 "..\..\..\..\Common\StudentSelectionWindow.xaml"
            ((SMS.Common.StudentSelectionWindow)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SearchTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 33 "..\..\..\..\Common\StudentSelectionWindow.xaml"
            this.SearchTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SearchTextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.search_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 34 "..\..\..\..\Common\StudentSelectionWindow.xaml"
            this.search_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.search_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.class_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 41 "..\..\..\..\Common\StudentSelectionWindow.xaml"
            this.class_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.class_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.section_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 51 "..\..\..\..\Common\StudentSelectionWindow.xaml"
            this.section_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.section_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.month_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 63 "..\..\..\..\Common\StudentSelectionWindow.xaml"
            this.month_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.month_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.date_picker = ((System.Windows.Controls.DatePicker)(target));
            
            #line 83 "..\..\..\..\Common\StudentSelectionWindow.xaml"
            this.date_picker.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.date_picker_to_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.adm_grid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 11:
            this.proceed_btn = ((System.Windows.Controls.Button)(target));
            
            #line 113 "..\..\..\..\Common\StudentSelectionWindow.xaml"
            this.proceed_btn.Click += new System.Windows.RoutedEventHandler(this.proceed_btn_Click);
            
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
            case 9:
            
            #line 93 "..\..\..\..\Common\StudentSelectionWindow.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked);
            
            #line default
            #line hidden
            
            #line 93 "..\..\..\..\Common\StudentSelectionWindow.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked);
            
            #line default
            #line hidden
            break;
            case 10:
            
            #line 99 "..\..\..\..\Common\StudentSelectionWindow.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Click += new System.Windows.RoutedEventHandler(this.CheckBox_Checked_sub);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

