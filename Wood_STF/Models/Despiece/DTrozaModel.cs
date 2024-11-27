using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Wood_STF.Models.Despiece
{
    public class DTrozaModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string nombreprop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombreprop));
        }
        
        private string id;
        private string idarbol;
        private string codqr;
        private DateTime fecha;
        private long idusuario;
        private bool cargando;

        [PrimaryKey]
        public string ID { get { return id; } set { id = value; OnPropertyChanged(); } }
        public string IDArbol { get { return idarbol; } set { idarbol = value; OnPropertyChanged(); } }
        public string CodQR { get { return codqr; } set { codqr = value; OnPropertyChanged(); } }
        public DateTime Fecha { get { return fecha; } set { fecha = value; OnPropertyChanged(); } }
        public long IDUsuario { get { return idusuario; } set { idusuario = value; OnPropertyChanged(); } }
        public bool Cargando { get { return cargando; } set { cargando = value; OnPropertyChanged(); } }
    }
}
