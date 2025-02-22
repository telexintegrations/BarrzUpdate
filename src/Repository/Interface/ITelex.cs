using src.Models;
using src.Models.Response;

namespace src.Repository.Interface{
    public interface ITelex
    {
        Task<string> GetToken();
        TelexConfig GetTelexConfiguration();
        Task<TelexResponse> TelexReport();
        Task<string> BingTelex();
        //get playlist
        //post to telex
    }
}