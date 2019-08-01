using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magic_Capstone.Models.DeckViewModels
{
    public class CardDeckViewModel
    {
        public CardData CardData { get; set; }
        public List<CardData> cardDatas { get; set; }
        public Deck Deck { get; set; }
        public List<Deck> AvailableDecks { get; set; }
        public List<SelectListItem> DeckOptions =>
            AvailableDecks.Select(a => new SelectListItem(a.DeckName, a.DeckId.ToString())).ToList();
    }
}
