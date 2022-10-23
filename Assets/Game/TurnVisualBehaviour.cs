using System.Collections;
using System.Collections.Generic;
using BrewedInk.CardCheat.Core;
using TMPro;
using UnityEngine;

namespace BrewedInk.CardCheat.Game
{
    public class TurnVisualBehaviour : GameListenerBehaviour
    {
        [Header("references")]
        public List<TextMeshProUGUI> TurnPreviews;


        public override void SetGame(Core.Game game)
        {
            base.SetGame(game);
            // SetTurnPreview();
        }

        public override IEnumerable Handle(TurnNumberChangedGameEvent evt)
        {
            SetTurnPreview();
            yield break;
        }

        public void SetTurnPreview()
        {
            var selected = TurnPreviews[(Game.turnCount-1) % TurnPreviews.Count];

            foreach (var preview in TurnPreviews)
            {
                preview.fontStyle = FontStyles.Normal;
            }

            selected.fontStyle = FontStyles.Bold;

        }
    }
}