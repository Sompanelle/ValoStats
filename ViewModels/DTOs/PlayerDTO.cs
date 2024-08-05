using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValoStats.Models;

namespace ValoStats.ViewModels.DTOs
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
                card = Root.data.card,
                player_title = Root.data.title,
                level = Root.data.account_level,
                updated_at = Root.data.updated_at,
            };
        }

    }
}
