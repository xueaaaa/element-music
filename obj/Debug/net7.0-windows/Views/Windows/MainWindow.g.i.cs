﻿#pragma checksum "..\..\..\..\..\Views\Windows\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7197FDE724DDD87BA8BC47125E2563208DF40873"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using ElementMusic.Helpers;
using ElementMusic.Views.Pages;
using ElementMusic.Views.Windows;
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
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Converters;
using Wpf.Ui.Markup;


namespace ElementMusic.Views.Windows {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.FluentWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 38 "..\..\..\..\..\Views\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Wpf.Ui.Controls.NavigationView RootNavigation;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\..\..\Views\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Wpf.Ui.Controls.BreadcrumbBar BreadcrumbBar;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\..\..\Views\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Wpf.Ui.Controls.SnackbarPresenter SnackbarPresenter;
        
        #line default
        #line hidden
        
        
        #line 366 "..\..\..\..\..\Views\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContentPresenter RootContentDialog;
        
        #line default
        #line hidden
        
        
        #line 370 "..\..\..\..\..\Views\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Wpf.Ui.Controls.TitleBar TitleBar;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ElementMusic;component/views/windows/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\Windows\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.RootNavigation = ((Wpf.Ui.Controls.NavigationView)(target));
            return;
            case 2:
            this.BreadcrumbBar = ((Wpf.Ui.Controls.BreadcrumbBar)(target));
            return;
            case 3:
            this.SnackbarPresenter = ((Wpf.Ui.Controls.SnackbarPresenter)(target));
            return;
            case 4:
            
            #line 110 "..\..\..\..\..\Views\Windows\MainWindow.xaml"
            ((Wpf.Ui.Controls.PasswordBox)(target)).PasswordChanged += new System.Windows.RoutedEventHandler(this.PasswordBox_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 265 "..\..\..\..\..\Views\Windows\MainWindow.xaml"
            ((System.Windows.Controls.Slider)(target)).ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.Slider_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 306 "..\..\..\..\..\Views\Windows\MainWindow.xaml"
            ((System.Windows.Controls.Slider)(target)).ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.Slider_ValueChanged_1);
            
            #line default
            #line hidden
            return;
            case 7:
            this.RootContentDialog = ((System.Windows.Controls.ContentPresenter)(target));
            return;
            case 8:
            this.TitleBar = ((Wpf.Ui.Controls.TitleBar)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

