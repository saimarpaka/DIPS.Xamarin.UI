﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DIPS.Xamarin.UI.Samples.Converters
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConvertersPage : ContentPage
    {
        public ConvertersPage()
        {
            BindingContext = new ConvertersViewModel(Application.Current.MainPage.Navigation);
            InitializeComponent();
        }
    }
}