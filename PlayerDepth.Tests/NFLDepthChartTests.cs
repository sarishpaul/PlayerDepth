using PlayerDepth.Interfaces;
using PlayerDepth.Models;
using PlayerDepth.Providers;
using PlayerDepth.Validators;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace PlayerDepth.Tests
{
    public class NFLDepthChartTests
    {
        private readonly NFLDepthChart _NFLDepthChart;
        public NFLDepthChartTests()
        {
            _NFLDepthChart = new NFLDepthChart( new Validator());
        }
        [Fact]
        public void AddPlayerToDepthChart_Should_AddCustomerSuccessfully_When_ValidPlayer_And_Position_And_PositionDepthProvided()
        {
            //Arrange
            Player bob = new Player() { Player_Id =1, Name= "Bob" };
            string position = "WR";
            int positionDepth = 0;
            PlayersWithPosition player1WithPosition = new PlayersWithPosition() { Player = bob, PlayerPosition = position, PositionDepth = positionDepth };            

            //Act

            _NFLDepthChart.AddPlayerToDepthChart(player1WithPosition.Player, player1WithPosition.PlayerPosition, player1WithPosition.PositionDepth);

            //Assert
            Assert.Equal(bob, NFLDepthChart.PlayersAtPosition[position].Find(p=> p.Player_Id == bob.Player_Id && p.Name == bob.Name));
            Assert.Equal(positionDepth, NFLDepthChart.PlayersAtPosition[position].FindIndex(p => p.Player_Id == bob.Player_Id && p.Name == bob.Name));
        }
        [Fact]
        public void AddPlayerToDepthChart_Should_AddCustomerSuccessfully_When_ValidPlayer_And_Position_And_NoPositionDepthProvided()
        {
            //Arrange
            Player bob = new Player() { Player_Id = 1, Name = "Bob" };
            string position = "WR";
            int positionDepth = 0;
            PlayersWithPosition player1WithPosition = new PlayersWithPosition() { Player = bob, PlayerPosition = position, PositionDepth = null };

            //Act

            _NFLDepthChart.AddPlayerToDepthChart(player1WithPosition.Player, player1WithPosition.PlayerPosition, player1WithPosition.PositionDepth);

            //Assert
            Assert.Equal(bob, NFLDepthChart.PlayersAtPosition[position].Find(p => p.Player_Id == bob.Player_Id && p.Name == bob.Name));
            Assert.Equal(positionDepth, NFLDepthChart.PlayersAtPosition[position].FindIndex(p => p.Player_Id == bob.Player_Id && p.Name == bob.Name));
        }
        [Fact]
        public void AddPlayerToDepthChart_Should_AddCustomerSuccessfully_When_ValidPlayer_And_Position_And_PositionDepthProvided_To_CorrectDepth()
        {
            //Arrange
            Player bob = new Player() { Player_Id = 1, Name = "Bob" };
            Player alice = new Player() { Player_Id = 2, Name = "Alice" };
            string position = "WR";
            int positionDepth = 0;
            int BobsNewPositionDepth = 1;
            PlayersWithPosition player1WithPosition = new PlayersWithPosition() { Player = bob, PlayerPosition = position, PositionDepth = positionDepth };
            PlayersWithPosition player2WithPosition = new PlayersWithPosition() { Player = alice, PlayerPosition = position, PositionDepth = positionDepth };

            //Act

            _NFLDepthChart.AddPlayerToDepthChart(player1WithPosition.Player, player1WithPosition.PlayerPosition, player1WithPosition.PositionDepth);
            _NFLDepthChart.AddPlayerToDepthChart(player2WithPosition.Player, player2WithPosition.PlayerPosition, player2WithPosition.PositionDepth);

            //Assert
            Assert.Equal(bob, NFLDepthChart.PlayersAtPosition[position].Find(p => p.Player_Id == bob.Player_Id && p.Name == bob.Name));
            Assert.Equal(alice, NFLDepthChart.PlayersAtPosition[position].Find(p => p.Player_Id == alice.Player_Id && p.Name == alice.Name));

            Assert.Equal(BobsNewPositionDepth, NFLDepthChart.PlayersAtPosition[position].FindIndex(p => p.Player_Id == bob.Player_Id && p.Name == bob.Name));
            Assert.Equal(positionDepth, NFLDepthChart.PlayersAtPosition[position].FindIndex(p => p.Player_Id == alice.Player_Id && p.Name == alice.Name));
        }

        [Fact]
        public void AddPlayerToDepthChart_ShouldNot_AddCustomerSuccessfully_When_Duplicate_PlayerId_Provided()
        {
            //Arrange
            Player bob = new Player() { Player_Id = 1, Name = "Bob" };
            Player alice = new Player() { Player_Id = 1, Name = "Alice" };
            string position = "WR";
            int positionDepth = 0;
            PlayersWithPosition player1WithPosition = new PlayersWithPosition() { Player = bob, PlayerPosition = position, PositionDepth = positionDepth };
            PlayersWithPosition player2WithPosition = new PlayersWithPosition() { Player = alice, PlayerPosition = position, PositionDepth = positionDepth };

            //Act

            _NFLDepthChart.AddPlayerToDepthChart(player1WithPosition.Player, player1WithPosition.PlayerPosition, player1WithPosition.PositionDepth);
            var ex = Assert.Throws<Exception>(()=>_NFLDepthChart.AddPlayerToDepthChart(player2WithPosition.Player, player2WithPosition.PlayerPosition, player2WithPosition.PositionDepth));

            //Assert
            Assert.StartsWith($"Invalid input. Player with Id: {alice.Player_Id} already exists in the position list {position}", ex.Message);
        }

        [Theory]
        [InlineData(-1, null, "WR", 0)]
        [InlineData(-1,"Bob","WR",0)]
        [InlineData(1, "", "WR", 0)]
        [InlineData(1, "Bob", "", 0)]
        [InlineData(1, "Bob", "WR", -1)]
        [InlineData(1, "Bob", "KK", 0)]
        public void AddPlayerToDepthChart_ShouldNot_AddCustomerSuccessfully_When_InvalidInput_Is_Provided(int playerId, string playerName, string position, int? positionDepth)
        {
            //Arrange
            Player player = (playerId < 0 && playerName is null) ? null : new Player() { Player_Id = playerId, Name = playerName };

            PlayersWithPosition playerWithPosition = new PlayersWithPosition() { Player = player, PlayerPosition = position, PositionDepth = positionDepth };

            //Act            
            var ex = Assert.Throws<Exception>(() => _NFLDepthChart.AddPlayerToDepthChart(playerWithPosition.Player, playerWithPosition.PlayerPosition, playerWithPosition.PositionDepth));

            //Assert
            Assert.StartsWith($"Invalid input", ex.Message);
        }

        [Fact]
        public void AddPlayerToDepthChart_Should_AddCustomerSuccessfully_When_ValidPlayer_And_Position_And_PositionDepthProvided_To_CorrectDepth_When_Multiple_Players()
        {
            //Arrange
            Player bob = new Player() { Player_Id = 1, Name = "Bob" };
            Player alice = new Player() { Player_Id = 2, Name = "Alice" };
            Player charlie = new Player() { Player_Id = 3, Name = "Charlie" };
            string position = "WR";
            int positionDepth = 0;
            int charliesPositionDepth = 2;
            int BobsNewPositionDepth = 1;
            PlayersWithPosition player1WithPosition = new PlayersWithPosition() { Player = bob, PlayerPosition = position, PositionDepth = positionDepth };
            PlayersWithPosition player2WithPosition = new PlayersWithPosition() { Player = alice, PlayerPosition = position, PositionDepth = positionDepth };
            PlayersWithPosition player3WithPosition = new PlayersWithPosition() { Player = charlie, PlayerPosition = position, PositionDepth = charliesPositionDepth };

            //Act
            _NFLDepthChart.AddPlayerToDepthChart(player1WithPosition.Player, player1WithPosition.PlayerPosition, player1WithPosition.PositionDepth);
            _NFLDepthChart.AddPlayerToDepthChart(player2WithPosition.Player, player2WithPosition.PlayerPosition, player2WithPosition.PositionDepth);
            _NFLDepthChart.AddPlayerToDepthChart(player3WithPosition.Player, player3WithPosition.PlayerPosition, player3WithPosition.PositionDepth);

            //Assert
            Assert.Equal(bob, NFLDepthChart.PlayersAtPosition[position].Find(p => p.Player_Id == bob.Player_Id && p.Name == bob.Name));
            Assert.Equal(alice, NFLDepthChart.PlayersAtPosition[position].Find(p => p.Player_Id == alice.Player_Id && p.Name == alice.Name));
            Assert.Equal(charlie, NFLDepthChart.PlayersAtPosition[position].Find(p => p.Player_Id == charlie.Player_Id && p.Name == charlie.Name));

            Assert.Equal(BobsNewPositionDepth, NFLDepthChart.PlayersAtPosition[position].FindIndex(p => p.Player_Id == bob.Player_Id && p.Name == bob.Name));
            Assert.Equal(positionDepth, NFLDepthChart.PlayersAtPosition[position].FindIndex(p => p.Player_Id == alice.Player_Id && p.Name == alice.Name));
            Assert.Equal(charliesPositionDepth, NFLDepthChart.PlayersAtPosition[position].FindIndex(p => p.Player_Id == charlie.Player_Id && p.Name == charlie.Name));
        }

        [Fact]
        public void GetPlayersUnderPlayerInDepthChart_Should_Return_All_Players_Under_Given_Player()
        {
            //Arrange
            Player bob = new Player() { Player_Id = 1, Name = "Bob" };
            Player alice = new Player() { Player_Id = 2, Name = "Alice" };
            Player charlie = new Player() { Player_Id = 3, Name = "Charlie" };
            string position = "WR";
            int positionDepth = 0;
            int charliesPositionDepth = 2;
            List<int> lstExpected = new List<int>() { 1, 3 };
            
            PlayersWithPosition player1WithPosition = new PlayersWithPosition() { Player = bob, PlayerPosition = position, PositionDepth = positionDepth };
            PlayersWithPosition player2WithPosition = new PlayersWithPosition() { Player = alice, PlayerPosition = position, PositionDepth = positionDepth };
            PlayersWithPosition player3WithPosition = new PlayersWithPosition() { Player = charlie, PlayerPosition = position, PositionDepth = charliesPositionDepth };
            _NFLDepthChart.AddPlayerToDepthChart(player1WithPosition.Player, player1WithPosition.PlayerPosition, player1WithPosition.PositionDepth);
            _NFLDepthChart.AddPlayerToDepthChart(player2WithPosition.Player, player2WithPosition.PlayerPosition, player2WithPosition.PositionDepth);
            _NFLDepthChart.AddPlayerToDepthChart(player3WithPosition.Player, player3WithPosition.PlayerPosition, player3WithPosition.PositionDepth);

            //Act
            (string position, Array players) result = _NFLDepthChart.GetPlayersUnderPlayerInDepthChart(alice, position);

            //Assert
            Assert.Collection(lstExpected, itm=> Assert.Equal(lstExpected[0], itm), itm => Assert.Equal(lstExpected[1], itm));
        }
        [Fact]
        public void RemovePlayerFromDepthChart_Should_Remove_Player_Provided_From_The_List()
        {
            //Arrange
            Player bob = new Player() { Player_Id = 1, Name = "Bob" };
            Player alice = new Player() { Player_Id = 2, Name = "Alice" };
            Player charlie = new Player() { Player_Id = 3, Name = "Charlie" };
            string position = "WR";
            const int NotFoundIndex = -1;
          

            PlayersWithPosition player1WithPosition = new PlayersWithPosition() { Player = bob, PlayerPosition = position };
            PlayersWithPosition player2WithPosition = new PlayersWithPosition() { Player = alice, PlayerPosition = position };
            PlayersWithPosition player3WithPosition = new PlayersWithPosition() { Player = charlie, PlayerPosition = position};
            _NFLDepthChart.AddPlayerToDepthChart(player1WithPosition.Player, player1WithPosition.PlayerPosition, player1WithPosition.PositionDepth);
            _NFLDepthChart.AddPlayerToDepthChart(player2WithPosition.Player, player2WithPosition.PlayerPosition, player2WithPosition.PositionDepth);
            _NFLDepthChart.AddPlayerToDepthChart(player3WithPosition.Player, player3WithPosition.PlayerPosition, player3WithPosition.PositionDepth);

            //Act

            bool result = _NFLDepthChart.RemovePlayerFromDepthChart(alice, position);
            const int ExpectedCountAfterRemoval = 2;

            //Assert
            Assert.Equal(NotFoundIndex, NFLDepthChart.PlayersAtPosition[position].FindIndex(p => p.Player_Id == alice.Player_Id && p.Name == alice.Name));
            Assert.Equal(ExpectedCountAfterRemoval, NFLDepthChart.PlayersAtPosition[position].Count);
        }

        [Fact]
        public void GetFullDepthChart_Should_Return_All_Players_Under_All_Positions()
        {
            //Arrange
            Player bob = new Player() { Player_Id = 1, Name = "Bob" };
            Player alice = new Player() { Player_Id = 2, Name = "Alice" };
            Player charlie = new Player() { Player_Id = 3, Name = "Charlie" };

            Dictionary<string, List<Player>> lstExpected = new Dictionary<string, List<Player>>();
            lstExpected.Add("WR", new List<Player>() { bob, alice, charlie });
            lstExpected.Add("KR", new List<Player>() { bob });        

            PlayersWithPosition player1WithPosition = new PlayersWithPosition() { Player = bob, PlayerPosition = "WR" };
            PlayersWithPosition player2WithPosition = new PlayersWithPosition() { Player = alice, PlayerPosition = "WR"};
            PlayersWithPosition player3WithPosition = new PlayersWithPosition() { Player = charlie, PlayerPosition = "WR"};
            PlayersWithPosition player4WithPosition = new PlayersWithPosition() { Player = bob, PlayerPosition = "KR"};
            _NFLDepthChart.AddPlayerToDepthChart(player1WithPosition.Player, player1WithPosition.PlayerPosition);
            _NFLDepthChart.AddPlayerToDepthChart(player2WithPosition.Player, player2WithPosition.PlayerPosition);
            _NFLDepthChart.AddPlayerToDepthChart(player3WithPosition.Player, player3WithPosition.PlayerPosition);
            _NFLDepthChart.AddPlayerToDepthChart(player4WithPosition.Player, player4WithPosition.PlayerPosition);

            //Act
            Dictionary<string, List<Player>> result = _NFLDepthChart.GetFullDepthChart();

            //Assert
            Assert.Collection(lstExpected, itm => Assert.Equal(lstExpected["WR"], itm.Value), itm => Assert.Equal(lstExpected["KR"], itm.Value));
        }
    }
}
