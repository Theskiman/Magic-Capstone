using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magic_Capstone.Models
{
    public class CardDeck
    {
        public int CardDeckId { get; set; }
        public int CardDataId { get; set; }
        public CardData CardData { get; set; }
        public int DeckId { get; set; }
        public Deck Deck { get; set; }
    }
}
