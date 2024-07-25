using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using ValorantTrackerApp.Models;

namespace ValorantTrackerApp.DTOs
{
    public class MatchDTO
    {
        public static Match OneMatchResponseToDTO(MatchResponse Data)
        {
            MapData Match = Data.data.First().metadata;
            return new Match()
            {
                map = Match.map,
                game_length = Match.game_length,
                rounds_played = Match.rounds_played,
                mode = Match.mode,
            };
        }

        public static ObservableCollection<Match> MatchesResponseToDTO(MatchResponse Data)
        {
            ObservableCollection<Match> _ = new();
            foreach (MapData mapData in Data.data.Select(x => x.metadata))
            {
                _.Add(new Match()
                {
                    map = mapData.map,
                    game_length = mapData.game_length,
                    rounds_played = mapData.rounds_played,
                    mode = mapData.mode,
                });
            }
            return _;
        }


    }
}
