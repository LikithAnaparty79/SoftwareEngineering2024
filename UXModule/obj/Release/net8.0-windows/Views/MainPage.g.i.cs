﻿#pragma checksum "..\..\..\..\Views\MainPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "D8C1373ABA4B7D8D61443AE215F8BBED03D58FA6"
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
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
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
using UXModule.Views;


namespace UXModule.Views {
    
    
    /// <summary>
    /// MainPage
    /// </summary>
    public partial class MainPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 121 "..\..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Dashboard;
        
        #line default
        #line hidden
        
        
        #line 133 "..\..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Whiteboard;
        
        #line default
        #line hidden
        
        
        #line 145 "..\..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button FileCloner;
        
        #line default
        #line hidden
        
        
        #line 157 "..\..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Updater;
        
        #line default
        #line hidden
        
        
        #line 169 "..\..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Analyser;
        
        #line default
        #line hidden
        
        
        #line 181 "..\..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Screenshare;
        
        #line default
        #line hidden
        
        
        #line 193 "..\..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Chat;
        
        #line default
        #line hidden
        
        
        #line 205 "..\..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Cloud;
        
        #line default
        #line hidden
        
        
        #line 279 "..\..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame Main;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.8.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/UXModule;component/views/mainpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\MainPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.8.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Dashboard = ((System.Windows.Controls.Button)(target));
            
            #line 121 "..\..\..\..\Views\MainPage.xaml"
            this.Dashboard.Click += new System.Windows.RoutedEventHandler(this.DashboardClick);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Whiteboard = ((System.Windows.Controls.Button)(target));
            
            #line 133 "..\..\..\..\Views\MainPage.xaml"
            this.Whiteboard.Click += new System.Windows.RoutedEventHandler(this.WhiteboardClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.FileCloner = ((System.Windows.Controls.Button)(target));
            
            #line 145 "..\..\..\..\Views\MainPage.xaml"
            this.FileCloner.Click += new System.Windows.RoutedEventHandler(this.FileClonerClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Updater = ((System.Windows.Controls.Button)(target));
            
            #line 157 "..\..\..\..\Views\MainPage.xaml"
            this.Updater.Click += new System.Windows.RoutedEventHandler(this.UpdaterClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Analyser = ((System.Windows.Controls.Button)(target));
            
            #line 169 "..\..\..\..\Views\MainPage.xaml"
            this.Analyser.Click += new System.Windows.RoutedEventHandler(this.AnalyserClick);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Screenshare = ((System.Windows.Controls.Button)(target));
            
            #line 181 "..\..\..\..\Views\MainPage.xaml"
            this.Screenshare.Click += new System.Windows.RoutedEventHandler(this.ScreenShareClick);
            
            #line default
            #line hidden
            return;
            case 7:
            this.Chat = ((System.Windows.Controls.Button)(target));
            
            #line 193 "..\..\..\..\Views\MainPage.xaml"
            this.Chat.Click += new System.Windows.RoutedEventHandler(this.ChatButtonClick);
            
            #line default
            #line hidden
            return;
            case 8:
            this.Cloud = ((System.Windows.Controls.Button)(target));
            
            #line 205 "..\..\..\..\Views\MainPage.xaml"
            this.Cloud.Click += new System.Windows.RoutedEventHandler(this.UploadClick);
            
            #line default
            #line hidden
            return;
            case 9:
            this.Main = ((System.Windows.Controls.Frame)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

