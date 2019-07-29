using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Magic_Capstone.Models
{

    public class Rootobject
    {
        public int RootobjectId { get; set; }
        public Card[] cards { get; set; }
    }

    public class Card
    {
        public int CardId { get; set; }
        public int ConditionId { get; set; }
        public Condition Condition { get; set; }
        public string name { get; set; }
        public string manaCost { get; set; }    
        [NotMapped]
        public List<string> colors { get; set; }
        [NotMapped]
        public List<string> colorIdentity { get; set; }
        public string type { get; set; }
        
        public string rarity { get; set; }
        public string set { get; set; }
        public string setName { get; set; }
        public string text { get; set; }
        public string imageUrl { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }

 

}
