using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValorantTrackerApp.Models;

namespace ValorantTrackerApp.DTOs
{
    public class PlayerDTO
    {
        public static Player PlayerResponseToPlayer(PlayerResponse Root)
        {
            return new Player()
            { 
                puuid = Root.data.puuid,
                name = Root.data.name,
                tag = Root.data.tag,
                level = Root.data.account_level,
            };
        }

    }
}
