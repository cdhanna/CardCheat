using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BrewedInk.CardCheat.Core;
using DG.Tweening;
using UnityEngine;

namespace BrewedInk.CardCheat.Game
{
    public class PlayerHandBehaviour : GameListenerBehaviour
    {
        [Header("Templates")]
        public AbilityCardBehaviour abilityPrefab;

        [Header("State")] 
        public List<AbilityCardBehaviour> cards;


        public float elementWidth = 1.5f;
        
        public override IEnumerable Handle(DrawAbilityGameEvent evt)
        {
            var ability = Instantiate(abilityPrefab, transform);

            ability.Game = Game;
            ability.SetData(evt.Ability, this);
            
            
            
            // TODO play a fancy effect ? 
            cards.Add(ability);
            
            CalculateCardPositions();
            return base.Handle(evt);
        }
        
        public override IEnumerable Handle(AbilityUsedGameEvent evt)
        {
            var found = cards.FirstOrDefault(c => c.ability == evt.Ability);
            if (found != null)
            {
                RemoveCard(found);
            }
            
            return base.Handle(evt);
        }

        public override IEnumerable Handle(RoundOverGameEvent evt)
        {
            foreach (var card in cards)
            {
                Destroy(card.gameObject);
            }
            cards.Clear();
            CalculateCardPositions();
            yield break;
        }

        public void RemoveCard(AbilityCardBehaviour card)
        {
            Destroy(card.gameObject);
            cards.Remove(card);
            CalculateCardPositions();
        }
        
        void CalculateCardPositions()
        {
            var count = cards.Count;

            var totalWidth = count * elementWidth;
            
            for (var i = 0; i < count; i++)
            {
                var r = i / (float)count;

                // cards[i].transform.DOLocalMove(new Vector3(i * elementWidth, 0, 0), .2f);
                cards[i].SetControlIndex();
            }
        }
    }
}