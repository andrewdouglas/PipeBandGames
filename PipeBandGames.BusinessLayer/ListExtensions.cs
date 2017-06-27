using System.Collections.Generic;
using System.Linq;

namespace PipeBandGames.BusinessLayer
{
    public static class ListExtensions
    {
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            if (list == null || !list.Any())
            {
                return true;
            }

            return false;
        }
    }
}
