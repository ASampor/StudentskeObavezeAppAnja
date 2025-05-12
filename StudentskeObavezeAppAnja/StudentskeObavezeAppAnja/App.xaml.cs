using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StudentskeObavezeAppAnja.Data;
using System.IO;
using StudentskeObavezeAppAnja.Styles;


namespace StudentskeObavezeAppAnja
{
    public partial class App : Application
    {
        private static Database _database;

        public static Database Database
        {
            get
            {
                if (_database == null)
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "studentskeobaveze.db3");
                    _database = new Database(dbPath);
                }
                return _database;
            }
        }
        public App()
        {
            InitializeComponent();

            PostaviTemu(OSAppTheme.Light);

            MainPage = new MainPage();
        }

        public void PostaviTemu(OSAppTheme tema)
        {
            ResourceDictionary novaTema;

            if (tema == OSAppTheme.Dark)
                novaTema = new DarkTheme(); // ovo traži da si u DarkTheme.xaml stavila `x:Class="StudentskeObavezeAppAnja.Styles.DarkTheme"`
            else
                novaTema = new LightTheme();

            Application.Current.Resources = novaTema;

            Application.Current.UserAppTheme = tema;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
