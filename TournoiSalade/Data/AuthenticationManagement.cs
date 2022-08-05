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

        public async Task StoreCode(string code)
        {
            if(code == await GetCode())
                return;

            await _localStorageService.SetItemAsync("accessCode", code);
        }
    }
}
