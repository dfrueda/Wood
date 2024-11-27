using System;
using System.IO;
using Wood_STF.Data;
using Wood_STF.Views.Despiece;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wood_STF
{
    public partial class App : Application
    {

        //Database usuario
        static DataBase database;
        public static DataBase Database
        {
            get
            {
                if (database == null)
                {
                    database = new DataBase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WoodDB.db3"));
                }
                return database;
            }
        }

        //Modulo de despiece
        static DBDespiece dbdespiece;
        public static DBDespiece DBDespiece
        {
            get
            {
                if (dbdespiece == null)
                {
                    dbdespiece = new DBDespiece(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WoodDB.db3"));
                }
                return dbdespiece;
            }
        }

        //VARIABLES GLOBALES

        //Despiece
        public static string IDGArbol;

        //RestService
        public static CelularManager CelManager { get; private set; }

        public App()
        {
            InitializeComponent();
            CelManager = new CelularManager(new RestService());

            if (Application.Current.Properties.ContainsKey("ID"))
            {
                if (string.IsNullOrEmpty(Properties["ID"].ToString()))
                {
                    MainPage = new NavigationPage(new MainPage());
                }
                else
                {
                    MainPage = new NavigationPage(new ListDArbolView())
                    {
                        BarBackgroundColor = Color.DarkRed
                    };                    
                }
            } else MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            if (Application.Current.Properties.ContainsKey("Inicio") && DateTime.Now.CompareTo(App.Current.Properties["Inicio"])>=0)
            {
                App.Current.Properties["Inicio"] = DateTime.Now;
            }
        }

        protected override void OnResume()
        {
        }
    }
}
