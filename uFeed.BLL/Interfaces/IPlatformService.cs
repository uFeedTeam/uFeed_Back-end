using System.Collections.Generic;
using GameStore.BLL.DTO;

namespace GameStore.BLL.Interfaces
{
    public interface IPlatformService
    {
        IEnumerable<PlatformDto> GetAll();
    }
}
