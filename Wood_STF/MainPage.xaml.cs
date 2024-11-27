using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wood_STF.Models;
using Wood_STF.ViewModels;
using Wood_STF.Views;
using Xamarin.Forms;

namespace Wood_STF
{
    public partial class MainPage : ContentPage
    {
        public UserViewModel context = new UserViewModel();
        public MainPage()
        {
            InitializeComponent();
            BindingContext = context;
            PModulos.ItemsSource = context.DatosUsuario.GetModulos();
            BLog.Command = context.IngresarCommand;
        }
    }
}