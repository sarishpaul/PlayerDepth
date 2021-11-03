using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlayerDepth.Models;
using PlayerDepth.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayerDepth.Validators;

namespace PlayerDepth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerDepthController : ControllerBase
    {
        private readonly ILogger<PlayerDepthController> _logger;
        private readonly GameTypeProviderFactory _providerFactory;
        public PlayerDepthController(ILogger<PlayerDepthController> logger)
        {
            _providerFactory = new GameTypeProviderFactory();
            _logger = logger;
        }

        [Route("{gametype}/add")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Post(PlayersWithPosition playersWithPosition)
        {
            try
            {
                var gameType = HttpContext.Request?.RouteValues["gametype"].ToString();
                if (string.IsNullOrEmpty(gameType))
                {
                    return NotImplemetedGameType();
                }
                var depthChartGameType = _providerFactory.GetGameTypeProviderByName(gameType);                
                depthChartGameType.AddPlayerToDepthChart(playersWithPosition.Player, playersWithPosition.PlayerPosition, playersWithPosition.PositionDepth);               
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Route("{gametype}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Get()
        {
            try
            {
                var gameType = HttpContext.Request?.RouteValues["gametype"].ToString();
                if (string.IsNullOrEmpty(gameType))
                {
                    return NotImplemetedGameType();
                }
                var depthChartGameType = _providerFactory.GetGameTypeProviderByName(gameType);
                var fullDepthChart = depthChartGameType.GetFullDepthChart();

                return Ok(string.Join("\n", fullDepthChart
                    .Select(d => d.Key + ": [" + string.Join(",", d.Value.Select(k => k.Player_Id)) + "],"))
                    .TrimEnd(','));              
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Route("{gametype}/getplayersunderplayer")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult GetPlayersUnderAnother(Player player, string position)
        {
            try
            {
                var gameType = HttpContext.Request?.RouteValues["gametype"].ToString();
                if (string.IsNullOrEmpty(gameType))
                {
                    return NotImplemetedGameType();
                }
                var depthChartGameType = _providerFactory.GetGameTypeProviderByName(gameType);
                (string playerPosition, Array arrPlayers) = depthChartGameType.GetPlayersUnderPlayerInDepthChart(player, position);
                return Ok(string.Concat("[", ConvertToString(arrPlayers), "]"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Route("{gametype}/removeplayer")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult RemovePlayer(Player player, string position)
        {
            try
            {
                var gameType = HttpContext.Request?.RouteValues["gametype"].ToString();
                if (string.IsNullOrEmpty(gameType))
                {
                    return NotImplemetedGameType();
                }
                var depthChartGameType = _providerFactory.GetGameTypeProviderByName(gameType);
                depthChartGameType.RemovePlayerFromDepthChart(player, position);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private string ConvertToString(Array arr)
        {
            string result = string.Empty;
            foreach(var itm in arr)
            {
                result += string.Concat(itm.ToString(), ",");
            }
            return result.TrimEnd(',');
        }
        private ActionResult NotImplemetedGameType()
        {
            return NotFound($"This game type is not implemented yet.");
        }
    }
}
