﻿#pragma checksum "..\..\..\..\..\AdvancedAccountManagement\DetailedAccounts\DetailedAccountsWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "B5C8E0462BF9731B32EF68DFCB04A61ABA64AB8D348F519B78A9FC13D71C9701"
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


namespace SMS.AdvancedAccountManagement.DetailedAccounts {
    
    
    /// <summary>
    /// DetailedAccountsWindow
    /// </summary>
    public partial class DetailedAccountsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\..\..\AdvancedAccountManagement\DetailedAccounts\DetailedAccountsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox account_type_cmb;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\..\AdvancedAccountManagement\DetailedAccounts\DetailedAccountsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox category_account_cmb;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\..\..\AdvancedAccountManagement\DetailedAccounts\DetailedAccountsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox account_code_textbox;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\..\..\AdvancedAccountManagement\DetailedAccounts\DetailedAccountsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel amount_sp;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\..\..\AdvancedAccountManagement\DetailedAccounts\DetailedAccountsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox account_textbox;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\..\..\AdvancedAccountManagement\DetailedAccounts\DetailedAccountsWindow.xaml"
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/advancedaccountmanagement/detailedaccounts/detailedaccountswindow." +
                    "xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\AdvancedAccountManagement\DetailedAccounts\DetailedAccountsWindow.xaml"
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
            
            #line 4 "..\..\..\..\..\AdvancedAccountManagement\DetailedAccounts\DetailedAccountsWindow.xaml"
            ((SMS.AdvancedAccountManagement.DetailedAccounts.DetailedAccountsWindow)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.account_type_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 24 "..\..\..\..\..\AdvancedAccountManagement\DetailedAccounts\DetailedAccountsWindow.xaml"
            this.account_type_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.account_type_cmb_SelectionChanged_1);
            
            #line default
            #line hidden
            return;
            case 3:
            this.category_account_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 39 "..\..\..\..\..\AdvancedAccountManagement\DetailedAccounts\DetailedAccountsWindow.xaml"
            this.category_account_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.account_category_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.account_code_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.amount_sp = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 6:
            this.account_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.submit_btn = ((System.Windows.Controls.Button)(target));
            
            #line 65 "..\..\..\..\..\AdvancedAccountManagement\DetailedAccounts\DetailedAccountsWindow.xaml"
            this.submit_btn.Click += new System.Windows.RoutedEventHandler(this.submit_btn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

