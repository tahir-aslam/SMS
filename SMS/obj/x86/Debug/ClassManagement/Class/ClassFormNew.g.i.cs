﻿#pragma checksum "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "973442DAF1F897211DDA5BE2A47C55F650CC5AC8BE4A707159EC28B98AEC5B42"
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
    /// ClassFormNew
    /// </summary>
    public partial class ClassFormNew : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 37 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox class_name_textbox;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox reg_fee_textbox;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox adm_fee_textbox;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tutuion_fee_textbox;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox transport_chrgs_textbox;
        
        #line default
        #line hidden
        
        
        #line 129 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox exam_fee_textbox;
        
        #line default
        #line hidden
        
        
        #line 151 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox other_exp_textbox;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/classmanagement/class/classformnew.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
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
            
            #line 8 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
            ((SMS.ClassManagement.Class.ClassFormNew)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.class_name_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.reg_fee_textbox = ((System.Windows.Controls.TextBox)(target));
            
            #line 50 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
            this.reg_fee_textbox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 4:
            this.adm_fee_textbox = ((System.Windows.Controls.TextBox)(target));
            
            #line 71 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
            this.adm_fee_textbox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 5:
            this.tutuion_fee_textbox = ((System.Windows.Controls.TextBox)(target));
            
            #line 93 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
            this.tutuion_fee_textbox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 6:
            this.transport_chrgs_textbox = ((System.Windows.Controls.TextBox)(target));
            
            #line 114 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
            this.transport_chrgs_textbox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 7:
            this.exam_fee_textbox = ((System.Windows.Controls.TextBox)(target));
            
            #line 137 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
            this.exam_fee_textbox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 8:
            this.other_exp_textbox = ((System.Windows.Controls.TextBox)(target));
            
            #line 159 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
            this.other_exp_textbox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 165 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_save);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 167 "..\..\..\..\..\ClassManagement\Class\ClassFormNew.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_cancel);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

