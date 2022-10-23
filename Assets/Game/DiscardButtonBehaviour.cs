
using System.Collections;
using BrewedInk.CardCheat.Core;
using BrewedInk.CardCheat.Game;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiscardButtonBehaviour : GameListenerBehaviour
{

    [Header("References")]
    public Button button;
    public TextMeshProUGUI displayText;
    public ControlHintBehaviour control;
    public Image[] discardReqImages;
    
    
    [Header("Sprites")] public Sprite Empty, Full;
    

    void Start()
    {
        button.onClick.AddListener(SubmitDiscard);
        control.SetAction(KeyCode.Tab, SubmitDiscard);
        UpdateDiscardReqs();
    }
    
    

    public void SubmitDiscard()
    {
        if (Game.discardState == DiscardState.CanDiscard)
        {
            Game.SubmitEvent(new DiscardSelectionGameEvent());
        }
        else
        {
            Game.SubmitEvent(new RequestPlayerDealAbilityGameEvent());
        }
    }

    public override IEnumerable Handle(DiscardSelectionGameEvent evt)
    {
        UpdateDiscardReqs();
        yield break;
    }

    public override IEnumerable Handle(CanDiscardGameEvent evt)
    {
        displayText.text = evt.CanDiscard
            ? "Discard"
            : "Draw Card";

        UpdateDiscardReqs();
        yield break;
    }

    void UpdateDiscardReqs()
    {
        for (var i = 0; i < discardReqImages.Length; i++)
        {
            var isMet = Game.discardCount <= i;

            var oldSprite = discardReqImages[i].sprite;
            
            var sprite = isMet
                ? Empty
                : Full;

            var isDiff = oldSprite != sprite;
            if (isDiff)
            {
                discardReqImages[i].transform.DOPunchScale(Vector3.one, .2f);
            }
            discardReqImages[i].sprite = sprite;
        }
    }
    
}
