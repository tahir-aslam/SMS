﻿#pragma checksum "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "740620E13A9A913BA19454E592EE76CE549DBF02"
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


namespace SMS.AdmissionManagement.FineManagement.Fine {
    
    
    /// <summary>
    /// FineSearch
    /// </summary>
    public partial class FineSearch : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button nameedit;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image img;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button print_btn;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker date_picker;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox month_cmb;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox fine_type_cmb;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock total_amount_tb;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid fine_grid;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/admissionmanagement/finemanagement/fine/finesearch.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
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
            
            #line 20 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_new);
            
            #line default
            #line hidden
            return;
            case 2:
            this.nameedit = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
            this.nameedit.Click += new System.Windows.RoutedEventHandler(this.click_edit);
            
            #line default
            #line hidden
            return;
            case 3:
            this.img = ((System.Windows.Controls.Image)(target));
            return;
            case 4:
            
            #line 28 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_delete);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 32 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_refresh);
            
            #line default
            #line hidden
            return;
            case 6:
            this.print_btn = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
            this.print_btn.Click += new System.Windows.RoutedEventHandler(this.print_btn_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.label = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.date_picker = ((System.Windows.Controls.DatePicker)(target));
            
            #line 67 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
            this.date_picker.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.date_picker_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.month_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 70 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
            this.month_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.month_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.fine_type_cmb = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 11:
            this.total_amount_tb = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 12:
            this.fine_grid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 100 "..\..\..\..\..\..\AdmissionManagement\FineManagement\Fine\FineSearch.xaml"
            this.fine_grid.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.section_grid_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

