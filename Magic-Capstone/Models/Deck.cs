using Magic_Capstone.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Magic_Capstone.Models
{
    public class Deck
    {
        public int DeckId { get; set; }
        public string DeckName { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public ICollection<CardDeck> cardDecks { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [NotMapped]
        public ICollection<CardData> cardDatas { get; set; }
    }
}
