using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wood_STF.Models.Despiece;
using Xamarin.Forms;

namespace Wood_STF.ViewModels.Despiece
{
    public class DTrozaViewModel : DTrozaModel
    {
        DTrozaModel model = new DTrozaModel();
        DArbolModel arbolmodel = new DArbolModel();
        public DTrozaViewModel() 
        {
            //GuardarCommand = new Command(async () => await Guardar(), () => !Cargando);
            EliminarCommand = new Command(async () => await Eliminar(), () => !Cargando);
        }

        public Command GuardarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }

        public async void Guardar()
        {
            Cargando = false;
            if (string.IsNullOrEmpty(CodQR) == false && App.DBDespiece.SearchTrozaQRAsync(CodQR).Result == null)
            {
                await App.DBDespiece.SaveTrozaAsync(new DTrozaModel
                {
                    ID = Guid.NewGuid().ToString(),
                    IDArbol = App.IDGArbol,
                    CodQR = CodQR,
                    Fecha = DateTime.Now,
                    IDUsuario = int.Parse(Application.Current.Properties["ID"].ToString()),
                });
                await NumeroTrozas();
            }
            else await App.Current.MainPage.DisplayAlert("Troza", "Falta", "Ok");
            Cargando = true;
        }
        public async Task Eliminar()
        {
            Cargando = false;
            if (await App.Current.MainPage.DisplayAlert("Troza", "¿Desea Eliminar la Troza " + CodQR + "?", "Eliminar", "Cancelar"))
            {
                model = await App.DBDespiece.SearchTrozaQRAsync(CodQR);
                await App.DBDespiece.DeleteTrozaAsync(model);
                await NumeroTrozas();
            }
            Cargando = true;
        }
        private async Task NumeroTrozas()
        {
            arbolmodel = await App.DBDespiece.SearchArbolAsync(App.IDGArbol);
            int n = App.DBDespiece.QueryArbolAsync(App.IDGArbol).Result.Count;
            await App.DBDespiece.UpdateArbolAsync(new DArbolModel()
            {
                ID = arbolmodel.ID,
                CodQR = arbolmodel.CodQR,
                NumTrozas = n,
                Observaciones = arbolmodel.Observaciones,
                IDUsuario = arbolmodel.IDUsuario,
                Fecha = arbolmodel.Fecha
            });
        }
        public bool Escanear(string codigo)
        {
            if (App.DBDespiece.SearchTrozaQRAsync(codigo).Result == null)
            {
                return true;
            }
            else { return false; }
        }
    }
}
