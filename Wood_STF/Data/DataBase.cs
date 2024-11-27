using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Wood_STF.Models;

namespace Wood_STF.Data
{
    public class DataBase
    {
        //MODULO MARCACION
        readonly SQLiteAsyncConnection _database;

        public DataBase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<PersonModel>().Wait();
        }

        //TABLA PERSONMODEL
        public Task<List<PersonModel>> GetPeopleAsync()
        {
            return _database.Table<PersonModel>().ToListAsync();
        }
        public Task<int> SavePersonAsync(PersonModel person)
        {
            return _database.InsertAsync(person);
        }
        public Task<int> DeletePersonAsync(PersonModel person)
        {
            return _database.DeleteAsync(person);
        }
        public Task<PersonModel> SearchPersonAsync(int ID)
        {
            return _database.Table<PersonModel>().FirstOrDefaultAsync(Ta => ID == Ta.ID);
        }
        public async Task<T> GetItemAsync<T>(string id) where T : new()
        {
            return await _database.FindAsync<T>(id);
        }
        public async Task ClearPersonAsync()
        {
            await _database.DeleteAllAsync<PersonModel>();
        }
        
    }
}
