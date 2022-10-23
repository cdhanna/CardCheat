using System.Collections;
using BrewedInk.CardCheat.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;


namespace BrewedInk.CardCheat.Game
{
    public class EnemyCardBehaviour : GameComponent<GameCardEnemy>
    {

        public SpriteRenderer Sprite;
        public TextMeshProUGUI ValueText;
        
        public override void OnCreate()
        {
            // position locally...
            
            transform.position = root.grid.CellToLocal(data.position);
            // transform.localPosition = transform.TransformPoint(root.grid.CellToLocal(data.position));
            // transform.localPosition = transform.InverseTransformPoint(root.grid.CellToWorld(data.position));


            var art = root.deck.GetArtForSuit(data.data.card.suit);
            Sprite.color = art.color;
            Sprite.sprite = art.symbol;

            ValueText.text = root.deck.GetDisplayValue(data.data.card.value);
            
            
            base.OnCreate();
        }


        public override IEnumerable Handle(RoundOverGameEvent evt)
        {
            if (this && gameObject)
            {
                Destroy(gameObject);
            }
            yield break;
        }

        public override IEnumerable Handle(PlayerPickupGameEvent evt)
        {
            if (evt.Enemy != data.data) yield break;
            
            Destroy(gameObject);
        }

        public override IEnumerable Handle(EnemyUpgradedGameEvent evt)
        {
            if (evt.Enemy != data.data) yield break;
            ValueText.text = root.deck.GetDisplayValue(data.data.card.value);

        }

        public override IEnumerable Handle(EnemyAttackGameEvent evt)
        {
            if (evt.Enemy != data.data) yield break;

            // do a little lunge at the player to indicate the attack... replace with a cool animation
            var dir = root.data.Game.player.position - data.position;
            yield return new WaitForSecondsRealtime(.05f);
            yield return transform.DOPunchPosition(new Vector3(dir.x, dir.y) * .7f, .5f, 5, .5f).WaitForCompletion();
            yield return new WaitForSecondsRealtime(.1f);

        }


        public override IEnumerable Handle(EnemyMoveGameEvent evt)
        {
            if (evt.Enemy != data.data) yield break;

            var move = transform.DOMove(root.grid.CellToLocal(data.position), .2f);

            yield return new WaitForSecondsRealtime(.02f);
            // transform.position = root.grid.CellToLocal(data.position);
        }

        public void Appear()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one * .9f, .2f);
        }
    }
}