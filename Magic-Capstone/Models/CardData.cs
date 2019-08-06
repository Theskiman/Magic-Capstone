using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Magic_Capstone.Models
{
    public class CardData
    {
        [Key]
        public int CardDataId { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public string manaCost { get; set; }
        [NotMapped]
        public string[] colors { get; set; }
        public string type { get; set; }
        public string rarity { get; set; }
        public string imageUrl { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public virtual ICollection<CardDeck> cardDecks { get; set; }
    }
}
