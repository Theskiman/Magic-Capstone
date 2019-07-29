using Magic_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Magic_Capstone
{
    
    public class CardProccessor
    {
        public async Task<Card> LoadCardByType(string cardType)
        {
            string url = $"https://api.magicthegathering.io/v1/cards?type={cardType}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    Card card = await response.Content.ReadAsAsync<Card>();
                    return card;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

            
        }
    }
}
