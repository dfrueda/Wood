using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using Wood_STF.Models;

namespace Wood_STF.Data
{
    public class UserData
    {
        public static ObservableCollection<UserModel> Usuarios
        {
            private set { }
            get
            {
                return new ObservableCollection<UserModel>()
                {
                    new UserModel()
                    {
                        ID=1,
                        Password="Marc",
                        Rol="Marcacion"
                    },
                    new UserModel()
                    {
                        ID=2,
                        Password="Desp",
                        Rol="Despiece"
                    },
                    new UserModel()
                    {
                        ID=3,
                        Password="Carga",
                        Rol="Carga"
                    },
                };
            }
        }
        public List<String> GetModulos()
        {
            List<String> Lmodulos = new List<string>();
            foreach (var u in Usuarios)
            {
                Lmodulos.Add(u.Rol);
            }
            return Lmodulos;
        }
    }
}