using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Wood_STF.Models
{
    public class UserModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string nombreprop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombreprop));
        }
        private int id;
        public int ID 
        {
            get { return id; }
            set 
            {
                id = value;
                OnPropertyChanged();
            } 
        }
        private string rol;
        public string Rol {
            get { return rol; }
            set 
            {
                rol = value;
                OnPropertyChanged();
            }
        }
        private string password;
        public string Password {
            get { return password; }
            set 
            {
                password = value;
                OnPropertyChanged();
            } 
        }
        private bool cargando;
        public bool Cargando {
            get { return cargando; }
            set
            {
                cargando = value;
                OnPropertyChanged();
            }
        }
    }
}