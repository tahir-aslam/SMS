﻿#pragma checksum "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "41EEB081371CB4145628803F9C2FDF1E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SMS.Converter;
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


namespace SMS.FeesManagement.FeesCollection {
    
    
    /// <summary>
    /// FeesCollectionPage
    /// </summary>
    public partial class FeesCollectionPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button nameedit;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image img;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button print_btn;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SearchTextBox;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox search_cmb;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox class_cmb;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox section_cmb;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock strength_textblock;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid fee_grid;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/feesmanagement/feescollection/feescollectionpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
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
            
            #line 8 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
            ((SMS.FeesManagement.FeesCollection.FeesCollectionPage)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            
            #line 8 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
            ((SMS.FeesManagement.FeesCollection.FeesCollectionPage)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Page_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.nameedit = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
            this.nameedit.Click += new System.Windows.RoutedEventHandler(this.click_edit);
            
            #line default
            #line hidden
            return;
            case 3:
            this.img = ((System.Windows.Controls.Image)(target));
            return;
            case 4:
            
            #line 31 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.click_refresh);
            
            #line default
            #line hidden
            return;
            case 5:
            this.print_btn = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
            this.print_btn.Click += new System.Windows.RoutedEventHandler(this.print_btn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.SearchTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 61 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
            this.SearchTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SearchTextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.search_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 62 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
            this.search_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.search_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.class_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 72 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
            this.class_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.class_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.section_cmb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 82 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
            this.section_cmb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.section_cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.strength_textblock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 11:
            this.fee_grid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 100 "..\..\..\..\..\FeesManagement\FeesCollection\FeesCollectionPage.xaml"
            this.fee_grid.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.fee_grid_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

