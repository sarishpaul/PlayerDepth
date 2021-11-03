using PlayerDepth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerDepth.Interfaces
{
    public interface IDepthChart
    {
        public string GameType { get; }
        public void AddPlayerToDepthChart(Player player, string position, int? positionDepth);
        public bool RemovePlayerFromDepthChart(Player player, string position);
        public Dictionary<string, List<Player>> GetFullDepthChart();
        public  (string, Array) GetPlayersUnderPlayerInDepthChart(Player player, string position);
    }
}
