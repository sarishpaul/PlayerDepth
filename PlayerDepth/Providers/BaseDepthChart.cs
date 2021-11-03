using PlayerDepth.Interfaces;
using PlayerDepth.Models;
using PlayerDepth.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerDepth.Providers
{
    public abstract class BaseDepthChart : IDepthChart
    {
        public Dictionary<string, List<Player>> PlayersAtPosition { get; set; }
        private readonly string[] _validPositions;
        private readonly IValidator _validator;

        public string GameType => "Base";

        public BaseDepthChart(Dictionary<string, List<Player>> playersAtPosition, string[] validPositions, IValidator validator)
        {
            PlayersAtPosition = playersAtPosition;
            _validPositions = validPositions;
            _validator = validator;
        }
        public virtual void AddPlayerToDepthChart(Player player, string position, int? positionDepth = null)
        {
            (bool isValid, List<string> errorList) = _validator.IsUserInputValid(player, position, _validPositions, PlayersAtPosition, positionDepth);
            if (!isValid)
            {
                throw new ArgumentException($"Invalid input. {string.Join("\n", errorList.ToArray())}");
            }
            if (PlayersAtPosition.ContainsKey(position))
            {
                PlayersAtPosition.TryGetValue(position, out List<Player> lstPlayers);
                if (lstPlayers is null)
                {
                    throw new ArgumentNullException("Could not find the player position");
                }
                HandlePositionList(player, position, positionDepth, lstPlayers);
            }
            else
            {
                HandlePositionList(player, position, positionDepth);
            }

        }
      

        public virtual Dictionary<string, List<Player>> GetFullDepthChart()
        {
            return PlayersAtPosition;
        }

        public virtual (string, Array) GetPlayersUnderPlayerInDepthChart(Player player, string position)
        {
            if (PlayersAtPosition.TryGetValue(position, out List<Player> lstPlayers))
            {
                int playerIndex = lstPlayers.FindIndex(p => p.Player_Id == player.Player_Id);
                if (playerIndex == -1)
                {
                    return (position, Array.Empty<int>());
                }
                var lstPlayerAfterSkip = lstPlayers.Skip(playerIndex + 1);
                var lstPlayersUnderPlayer = lstPlayerAfterSkip?.Take(lstPlayers.Count - playerIndex);
                var lstPlayerIdsUnderPlayer = lstPlayersUnderPlayer?.Select(p => p.Player_Id).ToArray();
                return (position, lstPlayerIdsUnderPlayer);
            }
            return (position, Array.Empty<int>());
        }
        public virtual bool RemovePlayerFromDepthChart(Player player, string position)
        {
            try
            {
                if (PlayersAtPosition.TryGetValue(position, out List<Player> lstPlayers))
                {
                    if (!(lstPlayers is null))
                    {
                        int playerIndex = lstPlayers.FindIndex(p => p.Player_Id == player.Player_Id);
                        if (playerIndex != -1)
                            lstPlayers.RemoveAt(playerIndex);
                        PlayersAtPosition[position] = lstPlayers;
                    }
                }
                return true; 
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public virtual bool IsPositionDepthValid(int CountOfItems, int? positionDepth)
        {
            return !positionDepth.HasValue || positionDepth <= CountOfItems;
        }       

        private void HandlePositionList(Player player, string position, int? positionDepth, List<Player> lstPlayers = null)
        {
            try
            {
                if (lstPlayers is null)
                {
                    if (!IsPositionDepthValid(0, positionDepth))
                    {
                        throw new ArgumentException($"Invalid Position depth. Cannot add to a depth more than the maximum available position which is 0");
                    }
                    lstPlayers = new List<Player>();
                    lstPlayers.Add(player);
                }
                else
                {
                    int currentListLength = lstPlayers.Count();
                    if (!IsPositionDepthValid(currentListLength, positionDepth))
                    {
                        throw new ArgumentException($"Invalid Position depth. Cannot add to a depth more than the maximum available position which is {currentListLength}");
                    }
                    if (!positionDepth.HasValue || (positionDepth == currentListLength))
                        lstPlayers.Add(player);
                    else
                        lstPlayers.Insert(positionDepth.Value, player);                    
                }
                PlayersAtPosition[position] = lstPlayers;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occured while adding player {player?.Name} to position {positionDepth}, Error : {ex.Message}");
            }
        }
    }
}

