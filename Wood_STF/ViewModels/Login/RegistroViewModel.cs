using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wood_STF.Models;
using Xamarin.Forms;

namespace Wood_STF.ViewModels
{
    public class RegistroViewModel : PersonModel
    {
        public Command RegistrarCommand { get; set; }
        public RegistroViewModel()
        {
            RegistrarCommand = new Command(async () => await Registro(), () => { return !Cargando; });

        }
        public async Task Registro()
        {
            await App.Database.SavePersonAsync(new PersonModel()
            {
                ID = ID,
                Nombre = Nombre,
                Apellido=Apellido,
                FechaRegistro = DateTime.UtcNow
            });
            await Application.Current.MainPage.DisplayAlert("Login", "Registro Exitoso!", "Ok");
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
