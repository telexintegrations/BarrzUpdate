using src.Models;
using src.Models.Request;
using src.Models.Response;

namespace src.Repository.Interface{
    public interface ITelex
    {
        Task<string> GetToken();
        TelexConfig GetTelexConfiguration();
        Task<TelexResponse> TelexReport(TickRequest request);
        Task<string> BingTelex(TickRequest request);
    }
}