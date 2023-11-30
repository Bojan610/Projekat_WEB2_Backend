using System.Collections.Generic;
using UserAdminAPI.DTO;

namespace UserAdminAPI.Interfaces
{
    public interface IAdminService
    {
        List<DisplayDelivererDto> GetProcessing();
        List<DisplayDelivererDto> GetDenied();
        List<DisplayDelivererDto> GetAccepted();

        bool AcceptDeliverer(string email);
        bool DeclineDeliverer(string email);
    }
}
