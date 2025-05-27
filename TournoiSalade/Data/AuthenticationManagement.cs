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

        public async Task<int?> GetNbPlayerPerTeam()
        {
            var nbPlayer = await _localStorageService.GetItemAsync<int?>("playerperteam");
            if (nbPlayer == null)
                return 2;
            return nbPlayer.Value;
        }

        public async Task StoreCode(string code)
        {
            if(code == await GetCode())
                return;

            await _localStorageService.SetItemAsync("accessCode", code);
        }

        public async Task StoreNbPlayerPerTeam(int playerPerTeam)
        {
            if(playerPerTeam == await GetNbPlayerPerTeam())
                return;

            await _localStorageService.SetItemAsync("playerperteam", playerPerTeam);
        }
    }
}
