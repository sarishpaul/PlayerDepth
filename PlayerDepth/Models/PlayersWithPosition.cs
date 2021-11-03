using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerDepth.Models
{
    public class PlayersWithPosition
    {
        [Required]
        public Player Player { get; set; }
        [Required]
        public string PlayerPosition { get; set; }
        public int? PositionDepth { get; set; } = null;
    }
}
