using Blazored.LocalStorage;

namespace TournoiSalade.Data
{
    public class AuthenticationManagement
    {
        public ILocalStorageService _localStorageService { get; }

        public AuthenticationManagement(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<string?> GetCode()
        {            
            return await _localStorageService.GetItemAsync<string>("accessCode");
        }

        public async Task<bool?> GetIsVache()
        {
            try
            {
                var isVache = await _localStorageService.GetItemAsync<bool?>("vache");
                if (isVache == null)
                    return false;
                return isVache.Value;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task StoreCode(string code)
        {
            if(code == await GetCode())
                return;

            await _localStorageService.SetItemAsync("accessCode", code);
        }

        public async Task StoreIsVache(bool isVache)
        {
            await _localStorageService.SetItemAsync("vache", isVache);
        }
    }
}
