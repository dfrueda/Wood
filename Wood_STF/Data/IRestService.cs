using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wood_STF.Models.Login;

namespace Wood_STF.Data
{
    public interface IRestService
    {
        Task<List<CelularModel>> RefreshDataAsync();
        Task SaveCelularAsync(CelularModel item, bool isNewItem);
        Task DeleteCelularAsync(string id);
    }
}
