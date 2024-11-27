using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wood_STF.Data;
using Wood_STF.Models;
using Wood_STF.Models.Despiece;
using Wood_STF.Views.Login;
using Xamarin.Forms;

namespace Wood_STF.ViewModels.Despiece
{
    public class DArbolViewModel : DArbolModel
    {
        public DArbolModel model = new DArbolModel();
        string result;
        public DArbolViewModel()
        {
            //AddCommand = new Command(async () => await Adicionar(), () => !Cargando);
            //GuardarCommand = new Command(async () => await Guardar(), () => !Cargando);
            ModificarCommand = new Command(async () => await Modificar(), () => !Cargando);
            EliminarCommand = new Command(async () => await Eliminar(), () => !Cargando);
            CerrarCommand = new Command(async () => await CerrarSesion(), () => !Cargando);
            LimpiarCommand = new Command(async () => await LimpiarDB(), () => !Cargando);
            AcercadeCommand = new Command(async () => await Acercade(), () => !Cargando);
            ExportToExcelCommand = new Command(async () => await ExportToExcel(), () => !Cargando);
            excelService = new ExcelService();
        }

        //public Command AddCommand { get; set; }
        public Command GuardarCommand { get; set; }
        public Command ModificarCommand { get; set; }
        public Command EliminarCommand { get; set; }
        public Command CerrarCommand { get; set; }
        public Command LimpiarCommand { get; set; }
        public Command AcercadeCommand { get; set; }
        public ICommand ExportToExcelCommand { private set; get; }

        private ExcelService excelService;

        public async void Guardar()
        {
            Cargando = false;
            if (App.DBDespiece.SearchArbolQRAsync(CodQR).Result == null)
            {
                await App.DBDespiece.SaveArbolAsync(new DArbolModel()
                {
                    ID = Guid.NewGuid().ToString(),
                    CodQR = CodQR,
                    NumTrozas = 0,
                    Observaciones = Observaciones,
                    Fecha = DateTime.Now,
                    IDUsuario = int.Parse(Application.Current.Properties["ID"].ToString())
                });
                await App.Current.MainPage.DisplayAlert("Arbol", "Exito", "Ok");
            }
            else { await App.Current.MainPage.DisplayAlert("Arbol", "El CodigoQR ya fue asigando a otro Arbol", "Ok"); }
            Cargando = true;
        }

        public async Task Modificar()
        {
            Cargando = false;
            await App.DBDespiece.UpdateArbolAsync(new DArbolModel()
            {
                ID = ID,
                CodQR = CodQR,
                NumTrozas = NumTrozas,
                Observaciones = Observaciones,
                IDUsuario = IDUsuario,
                Fecha = Fecha
            });
            Cargando = true;
        }

        private async Task Eliminar()
        {
            Cargando = false;
            if (await App.Current.MainPage.DisplayAlert("Arbol", "¿Desea Eliminar el arbol Actual?\nTodos los datos asociados seran eliminados", "Eliminar", "Cancelar"))
            {
                model = await App.DBDespiece.SearchArbolAsync(ID);
                await EliminarTrozas();
                await App.DBDespiece.DeleteArbolAsync(model);
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            Cargando = true;
        }

        private async Task EliminarTrozas()
        {
            List<DTrozaModel> LTrozas = await App.DBDespiece.QueryArbolAsync(App.IDGArbol);
            if (LTrozas != null)
            {
                foreach (var item in LTrozas)
                {
                    await App.DBDespiece.DeleteTrozaAsync(item);
                }
            }
        }

        public void Search(string id)
        {
            model=App.DBDespiece.SearchArbolAsync(id).Result;
            ID = model.ID;
            CodQR = model.CodQR;
            NumTrozas = model.NumTrozas;
            Observaciones = model.Observaciones;
            IDUsuario = model.IDUsuario;
            Fecha = model.Fecha;
        }

        private async Task CerrarSesion()
        {
            if (await Application.Current.MainPage.DisplayAlert("Sesion", "¿Confirme cierre de sesion?", "Si", "No"))
            {
                await MetodoCerrar();
            }
        }

        private async Task MetodoCerrar()
        {
            Application.Current.Properties["ID"] = ""; //Crea variable global ID
            Application.Current.Properties["Nombre"] = ""; //Crea variable global Nombre
            Application.Current.Properties["Modulo"] = ""; //Crea variable global Nombre
            await Application.Current.SavePropertiesAsync(); //Guardar las propiedades
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
        private async Task LimpiarDB()
        {
            Cargando = false;
            result = await App.Current.MainPage.DisplayPromptAsync("Pasword", "Ingrese la contraseña", "OK", "Cancelar");
            if (string.IsNullOrEmpty(result) == false && result == "CLEAR")
            {
                if (await App.Current.MainPage.DisplayAlert("Base de datos", "¿Desea eliminar toda la informacion de la base de datos?", "Limpiar", "Cancelar"))
                {
                    if (await App.Current.MainPage.DisplayAlert("Base de datos", "La informacion eliminada no se podra recuperar\n¿Desea Limpiar de todas maneras?", "Limpiar", "Cancelar"))
                    {
                        await App.DBDespiece.ClearTrozaAsync();
                        await App.DBDespiece.ClearArbolAsync();
                        await MetodoCerrar();
                    }
                }
            }
            else { await App.Current.MainPage.DisplayAlert("Base de datos", "Acceso denegado", "OK"); }
            Cargando = true;
        }
        private async Task Acercade()
        {
            Cargando = false;
            await Application.Current.MainPage.Navigation.PushAsync(new AcercadeView());
            Cargando = true;
        }

        private async Task ExportToExcel()
        {
            //Actividad = true;
            Cargando = false;
            var fileName = $"{Guid.NewGuid()}.xlsx";
            string filePath = excelService.GenerateExcel(fileName);

            var header = new List<string>()
            {
                "IDArbol","Numero Trozas", "Observaciones", "Fecha Arbol","Usuario Arbol",
                "CodQR Troza", "Fecha Troza", "Usuario Troza"
            };

            var data = new ExcelData();
            data.Headers = header;

            List<DTrozaModel> ListaTrozas = App.DBDespiece.GetTrozaAsync().Result;

            if (ListaTrozas.Count > 0)
            {
                foreach (var Item in ListaTrozas)
                {
                    DArbolModel Arbol = App.DBDespiece.SearchArbolAsync(Item.IDArbol).Result;

                    var row = new List<string>()
                    {
                        //Informacion Arbol
                        Arbol.CodQR,
                        Arbol.NumTrozas.ToString(),
                        Arbol.Observaciones,
                        Arbol.Fecha.ToString(),
                        Arbol.IDUsuario.ToString(),

                        //Informacion Troza
                        Item.CodQR,
                        Item.Fecha.ToString(),
                        Item.IDUsuario.ToString(),
                    };

                    data.Values.Add(row);
                }

                excelService.InsertDataIntoSheet(filePath, "Despiece", data);

                //await Launcher.OpenAsync(new OpenFileRequest()
                //{
                //    File = new ReadOnlyFile(filePath)
                //});
                await App.Current.MainPage.DisplayAlert("Exportacion", "Exportacion Exitosa", "Ok");
            }
            else await App.Current.MainPage.DisplayAlert("Exportacion", "No hay registros en la Base de Datos", "Ok");
            Cargando = true;
            //Actividad = false;
        }

        public bool Escanear(string codigo)
        {
            if (App.DBDespiece.SearchArbolQRAsync(codigo).Result == null)
            {
                return true;
            }
            else { return false; }
        }
    }
}
