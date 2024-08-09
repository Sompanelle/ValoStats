using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using ValoStats.Models;

namespace ValoStats.ViewModels.DTOs
{
    public class MMRDTO
    {
        public static CurrentMMR MMRResponseToMMR(MMRResponse Root)
        {
            return new CurrentMMR()
            {
                tier = Root.data.current.tier,
                rr = Root.data.current.rr,
                last_change = Root.data.current.last_change,
                elo = Root.data.current.elo,
                games_needed_for_rating = Root.data.current.games_needed_for_rating,

            };
        }

        public static MMRData? MMRResponseToMMRData(MMRResponse Root)
        {
            if(Root.data != null)
            { 
                return new MMRData()
                {
                    current = Root.data.current,
                    player = Root.data.player,
                    peak = Root.data.peak,
                    seasonal = Root.data.seasonal,
                };
            }
            else
            {
                return null;
            }
            
        }
        
    }
}
