﻿#pragma checksum "..\..\..\..\..\AdmissionManagement\BulkFeeUpdate\FeeUpdateForm.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "30649EC8B3319F2E472A96EBD0D6BE865E0A2DB14890F4F7F413C2D3491F302C"
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


namespace SMS.AdmissionManagement.BulkFeeUpdate {
    
    
    /// <summary>
    /// FeeUpdateForm
    /// </summary>
    public partial class FeeUpdateForm : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\..\..\AdmissionManagement\BulkFeeUpdate\FeeUpdateForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label std_lbl;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\..\AdmissionManagement\BulkFeeUpdate\FeeUpdateForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox fee_cmb;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\..\AdmissionManagement\BulkFeeUpdate\FeeUpdateForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox other_cmb;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\..\..\AdmissionManagement\BulkFeeUpdate\FeeUpdateForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox month_cmb;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\..\..\AdmissionManagement\BulkFeeUpdate\FeeUpdateForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker date_picker;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\..\..\AdmissionManagement\BulkFeeUpdate\FeeUpdateForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox amount_option;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\..\..\AdmissionManagement\BulkFeeUpdate\FeeUpdateForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox fee_textbox;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/admissionmanagement/bulkfeeupdate/feeupdateform.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\AdmissionManagement\BulkFeeUpdate\FeeUpdateForm.xaml"
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
            this.std_lbl = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.fee_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 34 "..\..\..\..\..\AdmissionManagement\BulkFeeUpdate\FeeUpdateForm.xaml"
            this.fee_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.fee_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.other_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 44 "..\..\..\..\..\AdmissionManagement\BulkFeeUpdate\FeeUpdateForm.xaml"
            this.other_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.other_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.month_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 56 "..\..\..\..\..\AdmissionManagement\BulkFeeUpdate\FeeUpdateForm.xaml"
            this.month_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.month_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.date_picker = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 6:
            this.amount_option = ((System.Windows.Controls.ComboBox)(target));
            
            #line 71 "..\..\..\..\..\AdmissionManagement\BulkFeeUpdate\FeeUpdateForm.xaml"
            this.amount_option.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.amount_option_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.fee_textbox = ((System.Windows.Controls.TextBox)(target));
            
            #line 78 "..\..\..\..\..\AdmissionManagement\BulkFeeUpdate\FeeUpdateForm.xaml"
            this.fee_textbox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 81 "..\..\..\..\..\AdmissionManagement\BulkFeeUpdate\FeeUpdateForm.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_save);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 82 "..\..\..\..\..\AdmissionManagement\BulkFeeUpdate\FeeUpdateForm.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_cancel);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

