﻿#pragma checksum "..\..\..\..\Views\Cashier\CashierMainPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "0F91C7C5725CA76C6A9B648C9634224AFD255192AE348AFA11D3CC002AF263A0"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using CoffeeShop.Views.Cashier;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace CoffeeShop.Views.Cashier {
    
    
    /// <summary>
    /// CashierMainPage
    /// </summary>
    public partial class CashierMainPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 29 "..\..\..\..\Views\Cashier\CashierMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock FIO;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\Views\Cashier\CashierMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ShiftStatus;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\Views\Cashier\CashierMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ShiftKassa;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\Views\Cashier\CashierMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock FIOKassa;
        
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
            System.Uri resourceLocater = new System.Uri("/CoffeeShop;component/views/cashier/cashiermainpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Cashier\CashierMainPage.xaml"
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
            this.FIO = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.ShiftStatus = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            
            #line 32 "..\..\..\..\Views\Cashier\CashierMainPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ClosePersonalShift_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 33 "..\..\..\..\Views\Cashier\CashierMainPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.PersonalPage_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ShiftKassa = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.FIOKassa = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            
            #line 42 "..\..\..\..\Views\Cashier\CashierMainPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenShift_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 43 "..\..\..\..\Views\Cashier\CashierMainPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CloseShift_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 44 "..\..\..\..\Views\Cashier\CashierMainPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SearchReceipt_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

