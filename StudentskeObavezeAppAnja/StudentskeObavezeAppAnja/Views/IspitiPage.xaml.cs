using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using StudentskeObavezeAppAnja.Models;
using StudentskeObavezeAppAnja.Data;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;




namespace StudentskeObavezeAppAnja.Views
{
    public partial class IspitiPage : ContentPage
    {
        private ObservableCollection<Ispit> listaIspita = new ObservableCollection<Ispit>();
        private Ispit izabraniIspitZaIzmenu = null;

        public IspitiPage()
        {
            InitializeComponent();
            BindingContext = this;
            _ = InicijalizujIspite();
        }

        private async Task InicijalizujIspite()
        {
            var svi = await App.Database.GetIspitiAsync();


            // Dodaj samo tri osnovna ako nema ništa
            if ((await App.Database.GetIspitiAsync()).Count == 0)
            {
                var pocetni = new[]
                {
                    new Ispit { Predmet = "Microsoft tehnologije za pristup podacima", DatumIspita = new DateTime(2025, 5, 4, 17, 30, 0), IspitZavrsen = false },
                    new Ispit { Predmet = "Uvod u softversko inženjerstvo (USI)", DatumIspita = new DateTime(2025, 5, 5, 13, 15, 0), IspitZavrsen = false },
                    new Ispit { Predmet = "Informacione i računarske arhitekture (IRAC)", DatumIspita = new DateTime(2025, 5, 6, 9, 0, 0), IspitZavrsen = false }
                };

                foreach (var ispit in pocetni)
                {
                    await App.Database.SacuvajIspitAsync(ispit);
                }
            }

            await UcitajIspite();
        }

        private async Task UcitajIspite()
        {
            listaIspita.Clear();
            var ispiti = (await App.Database.GetIspitiAsync())
                         .OrderBy(x => x.DatumIspita)
                         .ToList();

            foreach (var ispit in ispiti)
            {
                if (!ispit.AlarmPostavljen)
                {
                    await ZakaziAlarm(ispit);
                    ispit.AlarmPostavljen = true;
                    await App.Database.SacuvajIspitAsync(ispit);
                }
                listaIspita.Add(ispit);
            }

            ispitiListView.ItemsSource = listaIspita;
        }

        private async Task ZakaziAlarm(Ispit ispit)
        {
            var notification = new NotificationRequest
            {
                NotificationId = ispit.Id,  
                Title = "Podsetnik za ispit",
                Description = $"Ispit: {ispit.Predmet} - Datum: {ispit.DatumIspita.ToString("dd.MM.yyyy HH:mm")}",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = ispit.DatumIspita.AddDays(-3), // Alarm 3 dana pre ispita
                    NotifyRepeatInterval = TimeSpan.Zero // Bez ponavljanja
                }
            };

            NotificationCenter.Current.Show(notification);
        }


        private void OnDodajIspitClicked(object sender, EventArgs e)
        {
            izabraniIspitZaIzmenu = null;
            addIspitForm.IsVisible = true;
            btnDodajIspit.IsVisible = false;
        }

        private async void OnSaveIspitClicked(object sender, EventArgs e)
        {
            var datumIVreme = datePickerIspit.Date + timePickerIspit.Time;

            if (izabraniIspitZaIzmenu != null)
            {
                izabraniIspitZaIzmenu.Predmet = entryPredmet.Text;
                izabraniIspitZaIzmenu.DatumIspita = datumIVreme;
                await App.Database.SacuvajIspitAsync(izabraniIspitZaIzmenu);
            }
            else
            {
                var novi = new Ispit
                {
                    Predmet = entryPredmet.Text,
                    DatumIspita = datumIVreme,
                    AlarmPostavljen = false,
                    IspitZavrsen = false
                };
                await App.Database.SacuvajIspitAsync(novi);
            }

            entryPredmet.Text = "";
            addIspitForm.IsVisible = false;
            btnDodajIspit.IsVisible = true;

            await UcitajIspite();
        }

        private async void OnZavrsiIspitChecked(object sender, CheckedChangedEventArgs e)
        {
            var cb = (CheckBox)sender;
            var ispit = (Ispit)cb.BindingContext;
            ispit.IspitZavrsen = e.Value;
            await App.Database.SacuvajIspitAsync(ispit);
        }

        private async void OnObrisiClicked(object sender, EventArgs e)
        {
            var dugme = (Button)sender;
            var ispit = (Ispit)dugme.BindingContext;

            bool potvrda = await DisplayAlert("Potvrda", $"Obrisati ispit: {ispit.Predmet}?", "Da", "Ne");
            if (potvrda)
            {
                await App.Database.ObrisiIspitAsync(ispit);
                listaIspita.Remove(ispit);
            }
        }

        private void OnIzmeniClicked(object sender, EventArgs e)
        {
            var dugme = (Button)sender;
            izabraniIspitZaIzmenu = (Ispit)dugme.BindingContext;

            if (izabraniIspitZaIzmenu != null)
            {
                entryPredmet.Text = izabraniIspitZaIzmenu.Predmet;
                datePickerIspit.Date = izabraniIspitZaIzmenu.DatumIspita;
                timePickerIspit.Time = izabraniIspitZaIzmenu.DatumIspita.TimeOfDay;

                addIspitForm.IsVisible = true;
                btnDodajIspit.IsVisible = false;
            }
        }
        private void OnDarkModeToggled(object sender, ToggledEventArgs e)
        {
            var novaTema = e.Value ? OSAppTheme.Dark : OSAppTheme.Light;
            ((App)Application.Current).PostaviTemu(novaTema);
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            entry.TextColor = Color.HotPink;
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            entry.TextColor = Color.Black;
        }


    }
}