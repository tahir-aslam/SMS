﻿#pragma checksum "..\..\..\..\Controls\SmsEngine.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "AAC3E86AB993FA1EF93652280ADDDDFC735C1561"
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


namespace SMS.Controls {
    
    
    /// <summary>
    /// SmsEngine
    /// </summary>
    public partial class SmsEngine : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\..\Controls\SmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Controls\SmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock uploader_content_textblock;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\Controls\SmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock uploader_content_total_textblock;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\Controls\SmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock totalSmsSentTB;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\Controls\SmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid sms_grid;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\..\Controls\SmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar progressbar;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\..\Controls\SmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock progressbar_textblock;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\..\Controls\SmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button start_btn;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\..\Controls\SmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cancel_btn;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\..\Controls\SmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button finsish_btn;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\..\Controls\SmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock status_textblock;
        
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/controls/smsengine.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\SmsEngine.xaml"
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
            
            #line 7 "..\..\..\..\Controls\SmsEngine.xaml"
            ((SMS.Controls.SmsEngine)(target)).Loaded += new System.Windows.RoutedEventHandler(this.loaded_smsEngine);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.uploader_content_textblock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.uploader_content_total_textblock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.totalSmsSentTB = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.sms_grid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 7:
            this.progressbar = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 8:
            this.progressbar_textblock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.start_btn = ((System.Windows.Controls.Button)(target));
            
            #line 60 "..\..\..\..\Controls\SmsEngine.xaml"
            this.start_btn.Click += new System.Windows.RoutedEventHandler(this.buttonStart_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.cancel_btn = ((System.Windows.Controls.Button)(target));
            
            #line 61 "..\..\..\..\Controls\SmsEngine.xaml"
            this.cancel_btn.Click += new System.Windows.RoutedEventHandler(this.buttonCancel_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.finsish_btn = ((System.Windows.Controls.Button)(target));
            
            #line 62 "..\..\..\..\Controls\SmsEngine.xaml"
            this.finsish_btn.Click += new System.Windows.RoutedEventHandler(this.finsish_btn_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.status_textblock = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

