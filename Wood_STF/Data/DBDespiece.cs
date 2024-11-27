using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wood_STF.Models.Despiece;

namespace Wood_STF.Data
{
    public class DBDespiece
    {
        //MODULO DESPIECE
        readonly SQLiteAsyncConnection _database;

        public DBDespiece(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<DArbolModel>().Wait();
            _database.CreateTableAsync<DTrozaModel>().Wait();
        }

        //TABLA ARBOL
        public Task<List<DArbolModel>> GetArbolAsync()
        {
            return _database.Table<DArbolModel>().ToListAsync();
        }
        public Task<int> SaveArbolAsync(DArbolModel arbol)
        {
            return _database.InsertAsync(arbol);
        }
        public Task<int> UpdateArbolAsync(DArbolModel arbol)
        {
            return _database.UpdateAsync(arbol);
        }
        public Task<int> DeleteArbolAsync(DArbolModel arbol)
        {
            return _database.DeleteAsync(arbol);
        }
        public Task<DArbolModel> SearchArbolQRAsync(string codqr)
        {
            return _database.Table<DArbolModel>().FirstOrDefaultAsync(Ta => codqr == Ta.CodQR);
        }
        public Task<DArbolModel> SearchArbolAsync(string id)
        {
            return _database.Table<DArbolModel>().FirstOrDefaultAsync(Ta => id == Ta.ID);
        }
        public async Task ClearArbolAsync()
        {
            await _database.DeleteAllAsync<DArbolModel>();
        }

        //TABLA TROZA
        public Task<List<DTrozaModel>> GetTrozaAsync()
        {
            return _database.Table<DTrozaModel>().ToListAsync();
        }
        public Task<int> SaveTrozaAsync(DTrozaModel troza)
        {
            return _database.InsertAsync(troza);
        }
        public Task<int> UpdateTrozaAsync(DTrozaModel troza)
        {
            return _database.UpdateAsync(troza);
        }
        public Task<int> DeleteTrozaAsync(DTrozaModel troza)
        {
            return _database.DeleteAsync(troza);
        }
        public Task<DTrozaModel> SearchTrozaQRAsync(string codqr)
        {
            return _database.Table<DTrozaModel>().FirstOrDefaultAsync(Ta => codqr == Ta.CodQR);
        }
        public Task<List<DTrozaModel>> QueryTrozaAsync(string arbol, string codqr)
        {
            return _database.QueryAsync<DTrozaModel>("SELECT * from DTrozaModel WHERE IDArbol= '" + arbol + "' AND CodQR= '" + codqr + "'");
        }
        public Task<List<DTrozaModel>> QueryArbolAsync(string arbol)
        {
            return _database.QueryAsync<DTrozaModel>("SELECT * from DTrozaModel WHERE IDArbol= '" + arbol + "'");
        }
        public async Task ClearTrozaAsync()
        {
            await _database.DeleteAllAsync<DTrozaModel>();
        }
    }
}
