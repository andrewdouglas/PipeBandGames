using PipeBandGames.DataLayer.Entities;

namespace PipeBandGames.BusinessLayer.Interfaces
{
    public interface IJudgeService
    {
        Judge GetMatchingJudge(Contest contest, SoloEvent soloEvent);
    }
}
