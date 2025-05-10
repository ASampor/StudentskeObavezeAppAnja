using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StudentskeObavezeAppAnja.Data;
using System.IO;


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

            MainPage = new MainPage();
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
