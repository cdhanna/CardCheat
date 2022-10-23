using System.Collections;
using BrewedInk.CardCheat.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace BrewedInk.CardCheat.Game
{
    public class AbilityCardBehaviour : GameListenerBehaviour
    {
        [Header("references")]
        public TextMeshProUGUI textName;
        public ControlHintBehaviour hintBehaviour;
        
        
        [Header("runtime")]
        public Ability ability;

        private PlayerHandBehaviour _playerHandBehaviour;

        /*
         * An ability card lives in the draw pile...
         * Then it gets dealt.
         */

        public void SetData(Ability evtAbility, PlayerHandBehaviour playerHandBehaviour)
        {
            _playerHandBehaviour = playerHandBehaviour;
            ability = evtAbility;
            textName.text = ability.type.ToString();
            transform.DOScale(Vector3.one * .8f, .2f);
            SetControlIndex();


        }

        public void SetControlIndex()
        {
            var index = Game.player.data.abilities.IndexOf(ability);

            var kc = KeyCode.Alpha0;
            switch (index)
            {
                case 0:
                    kc = KeyCode.Alpha1;
                    break;
                
                case 1:
                    kc = KeyCode.Alpha2;
                    break;
                
                case 2:
                    kc = KeyCode.Alpha3;
                    break;
                
                case 3:
                    kc = KeyCode.Alpha4;
                    break;
                
                case 4:
                    kc = KeyCode.Alpha5;
                    break;
            }
            
            hintBehaviour.SetAction(kc, OnMouseUpAsButton);
        }
        

        private void OnMouseEnter()
        {
            if (Game.turnState == TurnState.GameOver) return;
            
            //root.Submit(new GameHoverGameEvent(data));
            transform.DOScale(Vector3.one, .2f);

        }

        private void OnMouseExit()
        {
            if (Game.turnState == TurnState.GameOver) return;

            transform.DOScale(Vector3.one * .8f, .2f);
            
        }

        private void OnMouseUpAsButton()
        {
            if (Game.turnState == TurnState.GameOver) return;

            Game.SubmitEvent(new AbilityClickedGameEvent(ability));
        }
        
    }
}