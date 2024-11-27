using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wood_STF.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wood_STF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistroPage : ContentPage
    {
        RegistroViewModel context = new RegistroViewModel();
        public int usuario;
        public RegistroPage(int Usuario)
        {
            InitializeComponent();
            BindingContext = context;
            usuario = Usuario;
            BRegistro.Command = context.RegistrarCommand;
            ENombre.TextChanged += FTextChanged;
            EId.TextChanged += FTextChanged;
            EApellido.TextChanged += FTextChanged;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            context.Nombre = "";
            context.Apellido = "";
            context.Cargando = false;
            LAdvertencia.IsVisible = false;
        }

        private void FTextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(context.Nombre)==false && string.IsNullOrEmpty(context.Apellido) == false && context.ID !=0 && context.ID==usuario)
            {
                context.Cargando = true;
            }
            else context.Cargando = false;

            if (context.ID == usuario)
            {
                EId.BackgroundColor = Color.LightGreen;
                LAdvertencia.IsVisible = false;
            }
            else if(context.ID != 0)
            {
                EId.BackgroundColor = Color.LightPink;
                LAdvertencia.IsVisible = true;
            }
        }
    }
}