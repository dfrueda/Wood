using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Wood_STF.Models
{
    public class PersonModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string nombreprop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombreprop));
        }
        private long id;
        [PrimaryKey]
        public long ID
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        private string nombre;
        public string Nombre {
            get { return nombre; } 
            set 
            {
                nombre = value;
                OnPropertyChanged();
            } 
        }
        private string apellido;
        public string Apellido
        {
            get { return apellido; }
            set
            {
                apellido = value;
                OnPropertyChanged();
            }
        }
        public DateTime FechaRegistro { get; set; }

        private bool cargando;
        public bool Cargando
        {
            get { return cargando; }
            set
            {
                cargando = value;
                OnPropertyChanged();
            }
        }
    }
}
