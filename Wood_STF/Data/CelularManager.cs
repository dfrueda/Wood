using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wood_STF.Models.Login;

namespace Wood_STF.Data
{
    public class CelularManager
    {
        IRestService restService;

        public CelularManager(IRestService service)
        {
            restService = service;
        }

        public Task<List<CelularModel>> GetTasksAsync()
        {
            return restService.RefreshDataAsync();
        }

        public Task SaveTaskAsync(CelularModel item, bool isNewItem = false)
        {
            return restService.SaveCelularAsync(item, isNewItem);
        }

        public Task DeleteTaskAsync(CelularModel item)
        {
            return restService.DeleteCelularAsync(item.IPCell);
        }
    }
}
