﻿#pragma checksum "..\..\..\..\..\Messaging\BrandedSms\BrandedSmsEngine.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "C7CDDA7DB72E55620F8803961E63120BC6A14EC43BC18777F93B132F20ECAAA7"
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


namespace SMS.Messaging.BrandedSms {
    
    
    /// <summary>
    /// BrandedSmsEngine
    /// </summary>
    public partial class BrandedSmsEngine : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 5 "..\..\..\..\..\Messaging\BrandedSms\BrandedSmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\..\Messaging\BrandedSms\BrandedSmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock uploader_content_textblock;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\..\Messaging\BrandedSms\BrandedSmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock uploader_content_total_textblock;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\..\Messaging\BrandedSms\BrandedSmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid sms_grid;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\..\Messaging\BrandedSms\BrandedSmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar progressbar;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\..\Messaging\BrandedSms\BrandedSmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock progressbar_textblock;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\..\Messaging\BrandedSms\BrandedSmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button uploader_btn;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\..\Messaging\BrandedSms\BrandedSmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cancel_btn;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\..\..\Messaging\BrandedSms\BrandedSmsEngine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button finsish_btn;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\..\..\Messaging\BrandedSms\BrandedSmsEngine.xaml"
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
            System.Uri resourceLocater = new System.Uri("/SMS;component/messaging/brandedsms/brandedsmsengine.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Messaging\BrandedSms\BrandedSmsEngine.xaml"
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
            
            #line 4 "..\..\..\..\..\Messaging\BrandedSms\BrandedSmsEngine.xaml"
            ((SMS.Messaging.BrandedSms.BrandedSmsEngine)(target)).Loaded += new System.Windows.RoutedEventHandler(this.loaded_smsEngine);
            
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
            this.sms_grid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 6:
            this.progressbar = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 7:
            this.progressbar_textblock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.uploader_btn = ((System.Windows.Controls.Button)(target));
            
            #line 51 "..\..\..\..\..\Messaging\BrandedSms\BrandedSmsEngine.xaml"
            this.uploader_btn.Click += new System.Windows.RoutedEventHandler(this.buttonStart_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.cancel_btn = ((System.Windows.Controls.Button)(target));
            
            #line 52 "..\..\..\..\..\Messaging\BrandedSms\BrandedSmsEngine.xaml"
            this.cancel_btn.Click += new System.Windows.RoutedEventHandler(this.buttonCancel_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.finsish_btn = ((System.Windows.Controls.Button)(target));
            
            #line 53 "..\..\..\..\..\Messaging\BrandedSms\BrandedSmsEngine.xaml"
            this.finsish_btn.Click += new System.Windows.RoutedEventHandler(this.finsish_btn_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.status_textblock = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

