﻿#pragma checksum "..\..\..\..\..\FeesManagement\FeesCollectionByVoucher\FeesCollectionByVoucherPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "D6AA92C5299336789B19C7D7E3C039FBBE98E9A4191203674ED36AC7EE26625B"
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


namespace SMS.FeesManagement.FeesCollectionByVoucher {
    
    
    /// <summary>
    /// FeesCollectionByVoucherPage
    /// </summary>
    public partial class FeesCollectionByVoucherPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 43 "..\..\..\..\..\FeesManagement\FeesCollectionByVoucher\FeesCollectionByVoucherPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\..\FeesManagement\FeesCollectionByVoucher\FeesCollectionByVoucherPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SearchTextBox;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\..\..\FeesManagement\FeesCollectionByVoucher\FeesCollectionByVoucherPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker date_picker;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\..\..\FeesManagement\FeesCollectionByVoucher\FeesCollectionByVoucherPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock count_TB;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\..\..\FeesManagement\FeesCollectionByVoucher\FeesCollectionByVoucherPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock paid_TB;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\..\..\FeesManagement\FeesCollectionByVoucher\FeesCollectionByVoucherPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid fee_voucher_grid;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/feesmanagement/feescollectionbyvoucher/feescollectionbyvoucherpage" +
                    ".xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\FeesManagement\FeesCollectionByVoucher\FeesCollectionByVoucherPage.xaml"
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
            
            #line 10 "..\..\..\..\..\FeesManagement\FeesCollectionByVoucher\FeesCollectionByVoucherPage.xaml"
            ((SMS.FeesManagement.FeesCollectionByVoucher.FeesCollectionByVoucherPage)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Page_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.label = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.SearchTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 56 "..\..\..\..\..\FeesManagement\FeesCollectionByVoucher\FeesCollectionByVoucherPage.xaml"
            this.SearchTextBox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 4:
            this.date_picker = ((System.Windows.Controls.DatePicker)(target));
            
            #line 63 "..\..\..\..\..\FeesManagement\FeesCollectionByVoucher\FeesCollectionByVoucherPage.xaml"
            this.date_picker.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.date_picker_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.count_TB = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.paid_TB = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.fee_voucher_grid = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

