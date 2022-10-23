using BrewedInk.CardCheat.Core;
using UnityEngine;

namespace BrewedInk.CardCheat.Data
{
    [CreateAssetMenu(menuName = "Game/Deck", order = -1000)]
    public class DeckData : ScriptableObject
    {
        public CardSuitArt clubsArt;
        public CardSuitArt heartsArt;
        public CardSuitArt diamondsArt;
        public CardSuitArt spadeArt;

        public string[] indexToDisplay = new string[]{"2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A"};
        
        public CardSuitArt GetArtForSuit(CardSuit suit)
        {
            switch (suit)
            {
                case CardSuit.Clubs: return clubsArt;
                case CardSuit.Diamonds: return diamondsArt;
                case CardSuit.Hearts: return heartsArt;
                case CardSuit.Spades: return spadeArt;
            }

            return clubsArt;
        }

        public string GetDisplayValue(int value)
        {
            if (indexToDisplay.Length > value)
            {
                return indexToDisplay[value];
            }
            else return "?"+value;
        }
        
        
    }
}