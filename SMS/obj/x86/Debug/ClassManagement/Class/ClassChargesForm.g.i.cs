﻿#pragma checksum "..\..\..\..\..\ClassManagement\Class\ClassChargesForm.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "8F80AC826891FA1115656774CBD8E42E89019B44B8FCD929C8318C78933CDC06"
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


namespace SMS.ClassManagement.Class {
    
    
    /// <summary>
    /// ClassChargesForm
    /// </summary>
    public partial class ClassChargesForm : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\..\..\..\ClassManagement\Class\ClassChargesForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox class_cmb;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\..\ClassManagement\Class\ClassChargesForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox fees_category_cmb;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\..\..\ClassManagement\Class\ClassChargesForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel amount_sp;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\..\ClassManagement\Class\ClassChargesForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox amount_textbox;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\..\..\ClassManagement\Class\ClassChargesForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button submit_btn;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/classmanagement/class/classchargesform.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\ClassManagement\Class\ClassChargesForm.xaml"
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
            
            #line 4 "..\..\..\..\..\ClassManagement\Class\ClassChargesForm.xaml"
            ((SMS.ClassManagement.Class.ClassChargesForm)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.class_cmb = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.fees_category_cmb = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.amount_sp = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 5:
            this.amount_textbox = ((System.Windows.Controls.TextBox)(target));
            
            #line 50 "..\..\..\..\..\ClassManagement\Class\ClassChargesForm.xaml"
            this.amount_textbox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 6:
            this.submit_btn = ((System.Windows.Controls.Button)(target));
            
            #line 55 "..\..\..\..\..\ClassManagement\Class\ClassChargesForm.xaml"
            this.submit_btn.Click += new System.Windows.RoutedEventHandler(this.submit_btn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

