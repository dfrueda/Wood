using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wood_STF.ViewModels.Despiece;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wood_STF.Views.Despiece
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EstadoArbol : ContentPage
    {
        DArbolViewModel context = new DArbolViewModel();
        public EstadoArbol(string idarbol)
        {
            InitializeComponent();
            BindingContext = context;
            context.ID = idarbol;
            this.ToolbarItems.Add(new ToolbarItem()
            {
                Text = "Eliminar",
                IconImageSource = ImageSource.FromFile("borrar.png"),
                Order = ToolbarItemOrder.Primary,
                Priority = 1,
                Command = context.EliminarCommand
            });
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            context.Search(context.ID);
        }

        private async void Editor_Completed(object sender, EventArgs e)
        {
            //context.Observaciones=e.ToString();
            await context.Modificar();
        }
    }
}