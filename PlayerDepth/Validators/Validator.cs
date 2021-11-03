using PlayerDepth.Interfaces;
using PlayerDepth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerDepth.Validators
{
    public class Validator : IValidator
    {
        public (bool, List<string>) IsUserInputValid(Player player, string position, string[] ValidPositions, Dictionary<string,List<Player>> PlayersAtPosition, int? positionDepth)
        {           
            List<string> errorList = new List<string>();
            if (player is null)
            {
                errorList.Add("Player cannot be null");
            }
            if (player?.Player_Id <= 0)
            {
                errorList.Add("Player ID should be greater than 0.");
            }
            if (string.IsNullOrEmpty(player?.Name))
            {
                errorList.Add("Player Name required.");
            }
            if (string.IsNullOrEmpty(position))
            {
                errorList.Add("Player poistion required.");
            }
            if (positionDepth.HasValue && positionDepth < 0)
            {
                errorList.Add("Invalid position depth. Should be greater than 0.");
            }
            if (PlayersAtPosition.TryGetValue(position, out List<Player> lstPerson) && lstPerson.Exists(p => p.Player_Id == player.Player_Id))
            {
                errorList.Add($"Player with Id: {player?.Player_Id} already exists in the position list {position}.");
            }
            if (!ValidPositions.Contains(position))
            {
                errorList.Add($"{position} is not a valid NFL position.");
            }
            return (errorList.Count() == 0, errorList);
        }
    }
}
