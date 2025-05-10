using SQLite;
using System.Collections.Generic;
using StudentskeObavezeAppAnja.Models;
using System.Threading.Tasks;

namespace StudentskeObavezeAppAnja.Data
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Beleska>().Wait();
            _database.CreateTableAsync<Ispit>().Wait();
        }

        public Task<List<Beleska>> GetBeleskeAsync()
        {
            return _database.Table<Beleska>().OrderByDescending(b => b.Datum).ToListAsync();
        }

        public Task<int> SacuvajBeleskuAsync(Beleska beleska)
        {
            if (beleska.Id != 0)
                return _database.UpdateAsync(beleska);
            else
                return _database.InsertAsync(beleska);
        }

        public Task<int> ObrisiBeleskuAsync(Beleska beleska)
        {
            return _database.DeleteAsync(beleska);
        }

        public Task<List<Ispit>> GetIspitiAsync()
        {
            return _database.Table<Ispit>().OrderByDescending(i => i.DatumIspita).ToListAsync();
        }

        public Task<int> SacuvajIspitAsync(Ispit ispit)
        {
            if (ispit.Id != 0)
                return _database.UpdateAsync(ispit);
            else
                return _database.InsertAsync(ispit);
        }

        public Task<int> ObrisiIspitAsync(Ispit ispit)
        {
            return _database.DeleteAsync(ispit);
        }


    }
}
