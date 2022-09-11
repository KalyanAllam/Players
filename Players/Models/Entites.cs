using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Players.Models
{
    public class players
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string position { get; set; }
        public List<playerSkills> playerSkills { get; set; }
    }

    public class playerSkills
    {
        [Key]
        public int Id { get; set; }
        public string skill { get; set; }
        public int value { get; set; }
        public int playerId { get; set; }
    }

}