
using System;
using System.Collections;
using BrewedInk.CardCheat.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreBehaviour : GameListenerBehaviour
{
    [Header("references")]
    public TextMeshProUGUI ComboText;
    public TextMeshProUGUI ScoreText;

    public override void SetGame(Game game)
    {
        base.SetGame(game);
        ComboText.text = $"combo {Game.combo}x";
        ScoreText.text = Game.score.ToString();
    }

    private void Update()
    {
        
        
        
        
    }

    public override IEnumerable Handle(ComboIncreasedGameEvent evt)
    {
        ComboText.text = $"combo {Game.combo}x";
        ComboText.transform.DOPunchScale(Vector3.one * .2f, .2f);
        return base.Handle(evt);
    }
    
    
    public override IEnumerable Handle(ComboBrokenGameEvent evt)
    {
        ComboText.text = $"combo {Game.combo}x";
        ComboText.transform.DOPunchScale(Vector3.one * .5f, .2f);
        
        return base.Handle(evt);
    }

    public override IEnumerable Handle(ScoreChangedGameEvent evt)
    {
        ScoreText.text = Game.score.ToString();
        return base.Handle(evt);
    }
}
