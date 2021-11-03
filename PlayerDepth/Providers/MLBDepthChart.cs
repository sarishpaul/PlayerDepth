using PlayerDepth.Interfaces;
using PlayerDepth.Models;
using PlayerDepth.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerDepth.Providers
{
    public class MLBDepthChart : BaseDepthChart
    {
        public static new Dictionary<string, List<Player>> PlayersAtPosition { get; set; } = new Dictionary<string, List<Player>>();
        private static readonly string[] _validPositions = new string[] { "SP", "RP", "C", "1B", "2B", "3B", "SS", "LF", "RF", "CF", "DH" };       
        public new string GameType => "MLB";
        public MLBDepthChart(IValidator validator) : base(PlayersAtPosition, _validPositions, validator)
        {
        
        }       
    }
}
