﻿#pragma checksum "..\..\..\..\..\FeeManagement\FeePaidByVoucher\FeePaidByVoucherWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "66B65D36A0421CF2C697A13CD8456E24"
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


namespace SMS.FeeManagement.FeePaidByVoucher {
    
    
    /// <summary>
    /// FeePaidByVoucherWindow
    /// </summary>
    public partial class FeePaidByVoucherWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\..\..\FeeManagement\FeePaidByVoucher\FeePaidByVoucherWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label reciept_no_lbl;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\..\FeeManagement\FeePaidByVoucher\FeePaidByVoucherWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label std_name_lbl;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\..\FeeManagement\FeePaidByVoucher\FeePaidByVoucherWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label adm_no_lbl;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\..\FeeManagement\FeePaidByVoucher\FeePaidByVoucherWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label months_lbl;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\..\FeeManagement\FeePaidByVoucher\FeePaidByVoucherWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker date;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\..\FeeManagement\FeePaidByVoucher\FeePaidByVoucherWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox total_tb;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/feemanagement/feepaidbyvoucher/feepaidbyvoucherwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\FeeManagement\FeePaidByVoucher\FeePaidByVoucherWindow.xaml"
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
            
            #line 4 "..\..\..\..\..\FeeManagement\FeePaidByVoucher\FeePaidByVoucherWindow.xaml"
            ((SMS.FeeManagement.FeePaidByVoucher.FeePaidByVoucherWindow)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            
            #line 4 "..\..\..\..\..\FeeManagement\FeePaidByVoucher\FeePaidByVoucherWindow.xaml"
            ((SMS.FeeManagement.FeePaidByVoucher.FeePaidByVoucherWindow)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.reciept_no_lbl = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.std_name_lbl = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.adm_no_lbl = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.months_lbl = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.date = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 7:
            this.total_tb = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

