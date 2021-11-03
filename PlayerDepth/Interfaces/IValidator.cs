using PlayerDepth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerDepth.Interfaces
{
    public interface IValidator
    {
        public (bool, List<string>) IsUserInputValid(Player player, string position, string[] ValidPositions, Dictionary<string, List<Player>> PlayersAtPosition, int? positionDepth);
    }
}
