using System;
using System.Collections;
using BrewedInk.CardCheat.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace BrewedInk.CardCheat.Game
{
    public class PlayerBehaviour : GameComponent<GamePlayer>
    {
        
        public SpriteRenderer Sprite;
        public TextMeshProUGUI ValueText;
        
        public override void OnCreate()
        {
            // position locally...
            transform.position = root.grid.CellToLocal(data.position);

            UpdateHoldGraphics();
            base.OnCreate();
        }

        void UpdateHoldGraphics()
        {
            if (!data.data.hasCard)
            {
                Sprite.sprite = null;
                ValueText.text = "";
                return;
            }
            
            var art = root.deck.GetArtForSuit(data.data.holdCard.suit);
            Sprite.color = art.color;
            Sprite.sprite = art.symbol;

            ValueText.text = root.deck.GetDisplayValue(data.data.holdCard.value);

        }
        
        

        private void Update()
        {
            Vector2Int delta = Vector2Int.zero;
            
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                delta = Vector2Int.left;    
            }
            
            
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                delta = Vector2Int.up;    
            }
            
            
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                delta = Vector2Int.right;    
            }
            
            
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                delta = Vector2Int.down;    
            }

            if (delta != Vector2Int.zero)
            {
                root.Submit(new CellClickedGameEvent(delta + root.game.player.position));
                

            }
        }

        public override IEnumerable Handle(RoundOverGameEvent evt)
        {
            UpdateHoldGraphics();
            yield break;
        }

        public override IEnumerable Handle(PlayerMoveGameEvent evt)
        {
            var dest = root.grid.CellToLocal(data.position);

            yield return transform.DOMove(dest, .1f).WaitForCompletion();
            
        }

        public override IEnumerable Handle(PlayerPickupGameEvent evt)
        {
            // TODO: do some sort of cool animation? 
            UpdateHoldGraphics();
            yield return new WaitForSecondsRealtime(.25f);
        }

        public override IEnumerable Handle(PlayerDropGameEvent evt)
        {
            UpdateHoldGraphics();
            yield return new WaitForSecondsRealtime(.25f);

            
        }
    }
}