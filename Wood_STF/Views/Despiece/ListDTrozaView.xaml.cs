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
    public partial class ListDTrozaView : ContentPage
    {
        DTrozaViewModel context = new DTrozaViewModel();
        public ListDTrozaView()
        {
            this.BindingContext = context;
            InitializeComponent();
            ToolbarItem item = new ToolbarItem
            {
                Text = "Adicionar",
                IconImageSource = "add.png",
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };
            item.Clicked += OnItemClicked;
            this.ToolbarItems.Add(item);
        }

        protected override void OnAppearing()
        {
            LVTroza.ItemsSource = App.DBDespiece.QueryArbolAsync(App.IDGArbol).Result;
            base.OnAppearing();
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
                        DisplayAlert("Trozas", "El codigo " + result.Text + " ya fue asignado", "OK");
                    }
                });
            };
            await Navigation.PushModalAsync(scannerPage);
        }

        private void LVTroza_Refreshing(object sender, EventArgs e)
        {
            LVTroza.ItemsSource = App.DBDespiece.QueryArbolAsync(App.IDGArbol).Result;
            LVTroza.IsRefreshing = false;
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item != null)
            {
                context.CodQR = ((DTrozaModel)item.BindingContext).CodQR;
                context.Eliminar();
            }
        }
    }
}