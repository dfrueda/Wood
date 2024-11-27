using Plugin.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Wood_STF.Data;
using Wood_STF.Models;
using Wood_STF.Views;
using Wood_STF.Views.Despiece;
using Xamarin.Essentials;
using Xamarin.Forms;
using Newtonsoft.Json;
using Wood_STF.Models.Login;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace Wood_STF.ViewModels
{
    public class UserViewModel:UserModel
    {
        public UserData DatosUsuario = new UserData();
        public ObservableCollection<UserModel> usuarios;
        //static HttpClient client = new HttpClient();
        public Command IngresarCommand { get; set; }
        public UserViewModel()
        {
            usuarios = UserData.Usuarios;
            IngresarCommand = new Command(async () => await Ingresar(), () => !Cargando);
        }
        private async Task Ingresar()
        {
            await ValidarInternet().ConfigureAwait(false);
            bool valido = Validar();
            bool Ingreso = false;
            PersonModel model;
            Cargando = false;

            if (ID != 0 && Password != null && Rol != null)
            {
                foreach (var u in usuarios)
                {
                    if (u.Password == Password && u.Rol == Rol)
                    {
                        Ingreso = true;
                        if (valido)
                        {
                            model = await App.Database.SearchPersonAsync(ID);
                            if (model != null)
                            {
                                Application.Current.Properties["ID"] = ID; //Crea variable global ID
                                Application.Current.Properties["Nombre"] = model.Nombre; //Crea variable global Nombre
                                Application.Current.Properties["Modulo"] = Rol; //Crea variable global Nombre
                                await Application.Current.SavePropertiesAsync(); //Guardar las propiedades

                                await Application.Current.MainPage.DisplayAlert("Login", "Bienvenido\n" + model.Nombre + " " + model.Apellido, "Ok");
                                Application.Current.MainPage = new NavigationPage(new ListDArbolView())
                                {
                                    BarBackgroundColor = Color.DarkRed
                                };
                            }
                            else
                            {
                                if (await Application.Current.MainPage.DisplayAlert("Login", "Usuario no registrado\n¿Desea crear un nuevo registro?", "Si", "No"))
                                {
                                    await Application.Current.MainPage.Navigation.PushModalAsync(new RegistroPage(ID));
                                }
                            }
                        }
                        else await Application.Current.MainPage.DisplayAlert("Login", "La licencia expiro\nComuniquese con su proveedor", "Ok");
                    }
                }
                if (Ingreso == false) { await Application.Current.MainPage.DisplayAlert("Login", "Credenciales incorrectos", "Ok"); }
            }
            else await Application.Current.MainPage.DisplayAlert("Login", "Datos faltantes", "Ok");
            Cargando = true;           
        }

        //Validar conexion a internet
        private async Task ValidarInternet()
        {
            CelularModel item = null;
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                App.Current.Properties["IDCEL"] = CrossDeviceInfo.Current.Id;
                List<CelularModel> items = App.CelManager.GetTasksAsync().Result;
                if (items != null)
                {
                    if (items.Count > 0)
                    {
                        item = items.Find(ta => ta.IPCell == App.Current.Properties["IDCEL"].ToString());
                    }
                    if (item == null)
                    {
                        //CREAR REGISTRO API(POST)
                        CelularModel Celular = new CelularModel()
                        {
                            IPCell = CrossDeviceInfo.Current.Id
                        };

                        await App.CelManager.SaveTaskAsync(Celular, true).ConfigureAwait(false);
                        items = App.CelManager.GetTasksAsync().Result;
                    }

                    if (item != null)
                    {
                        item = items.Find(ta => ta.IPCell == App.Current.Properties["IDCEL"].ToString());
                        // CONSULTAR INFORMACION EN API
                        if (Application.Current.Properties.ContainsKey("Inicio") == false || string.IsNullOrEmpty(App.Current.Properties["Inicio"].ToString()))
                        {
                            App.Current.Properties["Inicio"] = item.FInicio;
                        }
                        App.Current.Properties["FinalMarc"] = item.FFinalMarc;
                        App.Current.Properties["FinalDesp"] = item.FFinalDesp;
                        App.Current.Properties["FinalCarg"] = item.FFinalCarg;
                    }
                }
            }
        }

        //Validar Licencia Celular
        private bool Validar()
        {
            bool Habilitar = false;
            //Verifica Que el registro exista en la base de datos
            if (Application.Current.Properties.ContainsKey("FinalMarc") == false)
            {
                App.Current.Properties["FinalMarc"] = new DateTime(2000, 01, 01);
                App.Current.Properties["FinalDesp"] = new DateTime(2000, 01, 01);
                App.Current.Properties["FinalCarg"] = new DateTime(2000, 01, 01);
            }

            if (Application.Current.Properties.ContainsKey("Inicio") && Application.Current.Properties.ContainsKey("FinalMarc"))
            {
                if (DateTime.Now.CompareTo(App.Current.Properties["Inicio"]) >= 0 && DateTime.Now.CompareTo(App.Current.Properties["FinalMarc"]) == -1 && Rol == usuarios[0].Rol)
                {
                    Habilitar = true;
                }
                else if (DateTime.Now.CompareTo(App.Current.Properties["Inicio"]) >= 0 && DateTime.Now.CompareTo(App.Current.Properties["FinalDesp"]) == -1 && Rol == usuarios[1].Rol)
                {
                    Habilitar = true;
                }
                else if (DateTime.Now.CompareTo(App.Current.Properties["Inicio"]) >= 0 && DateTime.Now.CompareTo(App.Current.Properties["FinalCarg"]) == -1 && Rol == usuarios[2].Rol)
                {
                    Habilitar = true;
                }
            }
            return Habilitar;
        }
    }
}