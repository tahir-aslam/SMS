﻿#pragma checksum "..\..\..\..\Messaging\FeePaidSms.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "E97CB276B7F16B80646430FAACA47BBAEB97882769A50D10A10BC3A0F3B1EFD3"
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


namespace SMS.Messaging {
    
    
    /// <summary>
    /// FeePaidSms
    /// </summary>
    public partial class FeePaidSms : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 29 "..\..\..\..\Messaging\FeePaidSms.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock strength_textblock;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\Messaging\FeePaidSms.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker date_picker;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\..\Messaging\FeePaidSms.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox month_cmb;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\..\Messaging\FeePaidSms.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox class_cmb;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\..\Messaging\FeePaidSms.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox section_cmb;
        
        #line default
        #line hidden
        
        
        #line 110 "..\..\..\..\Messaging\FeePaidSms.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid paid_fee_grid;
        
        #line default
        #line hidden
        
        
        #line 168 "..\..\..\..\Messaging\FeePaidSms.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox message_textbox;
        
        #line default
        #line hidden
        
        
        #line 171 "..\..\..\..\Messaging\FeePaidSms.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton Without_btn;
        
        #line default
        #line hidden
        
        
        #line 172 "..\..\..\..\Messaging\FeePaidSms.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton With_btn;
        
        #line default
        #line hidden
        
        
        #line 175 "..\..\..\..\Messaging\FeePaidSms.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button send_btn;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/messaging/feepaidsms.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Messaging\FeePaidSms.xaml"
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
            
            #line 8 "..\..\..\..\Messaging\FeePaidSms.xaml"
            ((SMS.Messaging.FeePaidSms)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.strength_textblock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.date_picker = ((System.Windows.Controls.DatePicker)(target));
            
            #line 52 "..\..\..\..\Messaging\FeePaidSms.xaml"
            this.date_picker.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.date_picker_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.month_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 68 "..\..\..\..\Messaging\FeePaidSms.xaml"
            this.month_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.month_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.class_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 73 "..\..\..\..\Messaging\FeePaidSms.xaml"
            this.class_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.class_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.section_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 90 "..\..\..\..\Messaging\FeePaidSms.xaml"
            this.section_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.section_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.paid_fee_grid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 10:
            this.message_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.Without_btn = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 12:
            this.With_btn = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 13:
            this.send_btn = ((System.Windows.Controls.Button)(target));
            
            #line 175 "..\..\..\..\Messaging\FeePaidSms.xaml"
            this.send_btn.Click += new System.Windows.RoutedEventHandler(this.send_btn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 8:
            
            #line 115 "..\..\..\..\Messaging\FeePaidSms.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked);
            
            #line default
            #line hidden
            
            #line 115 "..\..\..\..\Messaging\FeePaidSms.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked);
            
            #line default
            #line hidden
            break;
            case 9:
            
            #line 121 "..\..\..\..\Messaging\FeePaidSms.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Click += new System.Windows.RoutedEventHandler(this.CheckBox_Checked_sub);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

