using PipeBandGames.DataLayer.Entities;
using System.Collections.Generic;

namespace PipeBandGames.BusinessLayer.Interfaces
{
    public interface IStartTimeService
    {
        void SetStartTimes(Contest contest, List<SoloEvent> soloEvents);
    }
}
