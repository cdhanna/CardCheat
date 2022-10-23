
using System;
using System.Collections;
using BrewedInk.CardCheat.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButtonBehaviour : GameListenerBehaviour
{

    [Header("References")]
    public Button button;

    public TextMeshProUGUI text;
    public ControlHintBehaviour control;
    public CanvasGroup roundCover;


    private void Start()
    {
        roundCover.alpha = 0;
        
        text = button.GetComponentInChildren<TextMeshProUGUI>();
        SetForEndTurn();
    }

    void SetForEndTurn()
    {
        text.text = "End Turn";
        button.onClick.AddListener(SubmitEndTurn);
        control.SetAction(KeyCode.Return, SubmitEndTurn);
        roundCover.alpha = 0;

    }
    
    void SetForNewRound()
    {
        DOTween.To(() => roundCover.alpha, x => roundCover.alpha = x, 1, .2f);
        text.text = "Continue";
        button.onClick.AddListener(SubmitEndTurn);
        control.SetAction(KeyCode.Return, SubmitNewRound);
    }
    

    public void SubmitEndTurn()
    {
        Game.SubmitEvent(new PlayerEndsTurnGameEvent());
    }

    public void SubmitNewRound()
    {
        Game.SubmitEvent(new BeginNewRoundGameEvent());
    }
    
    
    public override IEnumerable Handle(BeginNewRoundGameEvent evt)
    {
        SetForEndTurn();
        yield break;
    }

    public override IEnumerable Handle(RoundOverGameEvent evt)
    {
        SetForNewRound();
        yield break;
    }

}
