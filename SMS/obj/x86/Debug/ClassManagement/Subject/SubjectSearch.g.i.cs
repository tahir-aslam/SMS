﻿#pragma checksum "..\..\..\..\..\ClassManagement\Subject\SubjectSearch.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "0B42A586402FAE6194FBFCB44361734E6643216D86B515A88EBF14DA311382E2"
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


namespace SMS.ClassManagement.Subject {
    
    
    /// <summary>
    /// SubjectSearch
    /// </summary>
    public partial class SubjectSearch : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\..\..\..\ClassManagement\Subject\SubjectSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button nameedit;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\..\ClassManagement\Subject\SubjectSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image img;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\..\..\ClassManagement\Subject\SubjectSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\..\..\ClassManagement\Subject\SubjectSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SearchTextBox;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\..\..\ClassManagement\Subject\SubjectSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid subjects_grid;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/classmanagement/subject/subjectsearch.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\ClassManagement\Subject\SubjectSearch.xaml"
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
            
            #line 21 "..\..\..\..\..\ClassManagement\Subject\SubjectSearch.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_new);
            
            #line default
            #line hidden
            return;
            case 2:
            this.nameedit = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\..\..\..\ClassManagement\Subject\SubjectSearch.xaml"
            this.nameedit.Click += new System.Windows.RoutedEventHandler(this.click_edit);
            
            #line default
            #line hidden
            return;
            case 3:
            this.img = ((System.Windows.Controls.Image)(target));
            return;
            case 4:
            
            #line 29 "..\..\..\..\..\ClassManagement\Subject\SubjectSearch.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_delete);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 33 "..\..\..\..\..\ClassManagement\Subject\SubjectSearch.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_refresh);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 37 "..\..\..\..\..\ClassManagement\Subject\SubjectSearch.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.label = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.SearchTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 78 "..\..\..\..\..\ClassManagement\Subject\SubjectSearch.xaml"
            this.SearchTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SearchTextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.subjects_grid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 83 "..\..\..\..\..\ClassManagement\Subject\SubjectSearch.xaml"
            this.subjects_grid.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.subjects_grid_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

