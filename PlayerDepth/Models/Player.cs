using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerDepth.Models
{
    public class Player
    {
        [Required]
        public int Player_Id { get; set; }
        [Required]
        public string Name { get; set; }       
    }
}
