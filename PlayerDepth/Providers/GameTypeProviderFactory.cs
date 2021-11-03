using PlayerDepth.Interfaces;
using PlayerDepth.Validators;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerDepth.Providers
{
    public class GameTypeProviderFactory
    {
        
        private readonly IReadOnlyDictionary<string, IDepthChart> _gameTypeProviders;
        public GameTypeProviderFactory()
        {
            var depthChartProvider = typeof(IDepthChart);

            _gameTypeProviders = new Dictionary<string, IDepthChart>() {
                {"NFL", new NFLDepthChart( new Validator()) },
                {"MLB", new MLBDepthChart(new Validator()) }
            };
        }
        public IDepthChart GetGameTypeProviderByName(string GameType)
        {
            var provider = _gameTypeProviders.GetValueOrDefault(GameType);
            return provider ?? DefaultGameType();
        }
        private IDepthChart DefaultGameType()
        {
            return _gameTypeProviders[string.Empty];
        }
    }
}
