﻿#pragma checksum "..\..\..\..\..\ClassManagement\Subject\SubjectForm.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "63860B02ABBDD4272775D46DCF6F6D6346876DB0E2FD49D9AA996D4BA848A6AD"
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
    /// SubjectForm
    /// </summary>
    public partial class SubjectForm : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 30 "..\..\..\..\..\ClassManagement\Subject\SubjectForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox class_cmb;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\..\ClassManagement\Subject\SubjectForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox teacher_cmb;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\..\..\ClassManagement\Subject\SubjectForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox subj_name_textbox;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\..\..\ClassManagement\Subject\SubjectForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox total_marks_textbox;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\..\..\ClassManagement\Subject\SubjectForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox remarks_textbox;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\..\..\ClassManagement\Subject\SubjectForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox is_active_chekbox;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/classmanagement/subject/subjectform.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\ClassManagement\Subject\SubjectForm.xaml"
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
            
            #line 4 "..\..\..\..\..\ClassManagement\Subject\SubjectForm.xaml"
            ((SMS.ClassManagement.Subject.SubjectForm)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.class_cmb = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.teacher_cmb = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.subj_name_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.total_marks_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.remarks_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.is_active_chekbox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 8:
            
            #line 80 "..\..\..\..\..\ClassManagement\Subject\SubjectForm.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_save);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 82 "..\..\..\..\..\ClassManagement\Subject\SubjectForm.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_cancel);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

