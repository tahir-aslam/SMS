﻿#pragma checksum "..\..\..\..\..\AccountManagement\Account\AccountForm.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "643C4FF234DB1C9ED7C326646A613F93CBF3E873FB68CD51303AB0A2C0ED5A35"
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


namespace SMS.AccountManagement.Account {
    
    
    /// <summary>
    /// AccountForm
    /// </summary>
    public partial class AccountForm : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 33 "..\..\..\..\..\AccountManagement\Account\AccountForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox acc_name_textbox;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\..\AccountManagement\Account\AccountForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox acc_desc_textbox;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\..\AccountManagement\Account\AccountForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox acc_holder_name_textbox;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\..\AccountManagement\Account\AccountForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox acc_cell_textbox;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\..\AccountManagement\Account\AccountForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox acc_phone_textbox;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/accountmanagement/account/accountform.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\AccountManagement\Account\AccountForm.xaml"
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
            
            #line 4 "..\..\..\..\..\AccountManagement\Account\AccountForm.xaml"
            ((SMS.AccountManagement.Account.AccountForm)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.acc_name_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.acc_desc_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.acc_holder_name_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.acc_cell_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.acc_phone_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            
            #line 51 "..\..\..\..\..\AccountManagement\Account\AccountForm.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_save);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 53 "..\..\..\..\..\AccountManagement\Account\AccountForm.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_cancel);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

