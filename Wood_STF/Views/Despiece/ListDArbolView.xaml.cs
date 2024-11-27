using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wood_STF.Models.Despiece;
using Wood_STF.ViewModels.Despiece;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace Wood_STF.Views.Despiece
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListDArbolView : ContentPage
    {
        DArbolViewModel context = new DArbolViewModel();
        public ListDArbolView()
        {
            this.BindingContext = context;
            InitializeComponent();
            ToolbarItem item = new ToolbarItem
            {
                Text = Application.Current.Properties["Nombre"].ToString(),
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };
            this.ToolbarItems.Add(item);
            item = new ToolbarItem
            {
                Text = "Adicionar",
                IconImageSource = "add.png",
                Order = ToolbarItemOrder.Primary,
                Priority = 1
            };
            item.Clicked += OnItemClicked;
            this.ToolbarItems.Add(item);
            item = new ToolbarItem
            {
                Text = "Cerrar Sesion",
                Command = context.CerrarCommand,
                Order = ToolbarItemOrder.Secondary,
                Priority = 0
            };
            this.ToolbarItems.Add(item);
            item = new ToolbarItem
            {
                Text = "Limpiar Base de datos",
                Command = context.LimpiarCommand,
                Order = ToolbarItemOrder.Secondary,
                Priority = 1
            };
            this.ToolbarItems.Add(item);
            item = new ToolbarItem
            {
                Text = "Exportar datos",
                Command = context.ExportToExcelCommand,
                Order = ToolbarItemOrder.Secondary,
                Priority = 2
            };
            this.ToolbarItems.Add(item);
            item = new ToolbarItem
            {
                Text = "Acerca de",
                Command = context.AcercadeCommand,
                Order = ToolbarItemOrder.Secondary,
                Priority = 3
            };
            this.ToolbarItems.Add(item);
        }

        private async void OnItemClicked(object sender, EventArgs e)
        {
            string codigo;
            var scannerPage = new ZXingScannerPage();

            scannerPage.Title = "Lector de QR";
            scannerPage.OnScanResult += (result) =>
            {
                scannerPage.IsScanning = false;

                Device.BeginInvokeOnMainThread(() =>
                {
                    //Vibracion
                    {
                        // Or use specified time
                        var duration = TimeSpan.FromSeconds(1);
                        Vibration.Vibrate(duration);
                    }
                    Navigation.PopModalAsync();
                    codigo = result.Text;
                    if (context.Escanear(codigo))
                    {
                        context.CodQR = codigo;
                        context.Guardar();
                        //DisplayAlert("Codigo", result.Text, "OK");
                    }
                    else
                    {
                        DisplayAlert("Arbol", "El Arbol " + result.Text + " ya fue ingresado", "OK");
                    }
                });
            };
            await Navigation.PushModalAsync(scannerPage);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LVArbol.ItemsSource = App.DBDespiece.GetArbolAsync().Result;
        }

        private void LVArbol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.IDGArbol = ((DArbolModel)LVArbol.SelectedItem).ID;
            Navigation.PushAsync(new PassDespiece());
        }
    }
}