﻿#pragma checksum "..\..\..\..\..\ExamManagement\ExamResultList\ResultList.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "752EC0DFE949C691DBFEAA2D4BFD054ABEFEDBD2446CDAE5ED85DE449EFE89A0"
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


namespace SMS.ExamManagement.ExamResultList {
    
    
    /// <summary>
    /// ResultList
    /// </summary>
    public partial class ResultList : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 48 "..\..\..\..\..\ExamManagement\ExamResultList\ResultList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox exam_cmb;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\..\..\ExamManagement\ExamResultList\ResultList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox class_cmb;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\..\..\ExamManagement\ExamResultList\ResultList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox section_cmb;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\..\..\ExamManagement\ExamResultList\ResultList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button print_btn;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\..\..\..\ExamManagement\ExamResultList\ResultList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid exam_entry_grid;
        
        #line default
        #line hidden
        
        
        #line 109 "..\..\..\..\..\ExamManagement\ExamResultList\ResultList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid exam_img_grid;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/exammanagement/examresultlist/resultlist.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\ExamManagement\ExamResultList\ResultList.xaml"
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
            this.exam_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 48 "..\..\..\..\..\ExamManagement\ExamResultList\ResultList.xaml"
            this.exam_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.exam_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.class_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 61 "..\..\..\..\..\ExamManagement\ExamResultList\ResultList.xaml"
            this.class_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.class_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.section_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 78 "..\..\..\..\..\ExamManagement\ExamResultList\ResultList.xaml"
            this.section_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.section_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.print_btn = ((System.Windows.Controls.Button)(target));
            
            #line 94 "..\..\..\..\..\ExamManagement\ExamResultList\ResultList.xaml"
            this.print_btn.Click += new System.Windows.RoutedEventHandler(this.print_btn_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.exam_entry_grid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 102 "..\..\..\..\..\ExamManagement\ExamResultList\ResultList.xaml"
            this.exam_entry_grid.RowEditEnding += new System.EventHandler<System.Windows.Controls.DataGridRowEditEndingEventArgs>(this.exam_entry_grid_RowEditEnding);
            
            #line default
            #line hidden
            return;
            case 6:
            this.exam_img_grid = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

