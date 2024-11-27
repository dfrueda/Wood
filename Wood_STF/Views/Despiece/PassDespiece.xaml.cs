using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wood_STF.Views.Despiece
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PassDespiece : CarouselPage
    {
        public PassDespiece()
        {
            InitializeComponent();
            Children.Add(new EstadoArbol(App.IDGArbol));
            Children.Add(new ListDTrozaView());
        }
        protected override void OnCurrentPageChanged()
        {
            base.OnAppearing();
            if (this.Children.IndexOf(CurrentPage) == 0) { this.Title = "Arbol"; }
            else if (this.Children.IndexOf(CurrentPage) == 1) { this.Title = "Trozas"; }
        }
    }
}