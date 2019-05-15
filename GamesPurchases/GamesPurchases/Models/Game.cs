using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GamesPurchases.Models
{
    public class Game
    {
        public int GameID { get; set; }
        public string GameTitle { get; set; }
        public string GamePublisher { get; set; }
        public EnumPlatform GamePlatform { get; set; }
        [DataType(DataType.Date)]
        public DateTime GameRelease { get; set; }
        [DataType(DataType.Currency)]
        public double GameRRP { get; set; }

        public enum EnumPlatform
        {
            [Description("PC")] PC,
            [Description("PS4")] PS4,
            [Description("Xbox One")] XboxOne,
            [Description("Switch")] Switch,
            [Description("Mobile")] Mobile,
            [Description("Other")] Other
        }

    }

}
