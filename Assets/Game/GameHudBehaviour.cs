using System.Collections;
using System.Collections.Generic;
using BrewedInk.CardCheat.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHudBehaviour : GameListenerBehaviour
{
    public GameBehaviour root;
    public TextMeshProUGUI hitPointText;

    public CanvasGroup gameOverGroup;

    public Button RestartButton;

    private int _oldHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        RestartButton.onClick.AddListener(() => RestartGame());
    }

    private void RestartGame()
    {
        // need to clear the game's data... 
        root.data.ClearRuntime();
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetHealthText(int health)
    {
        hitPointText.text = $"Hit Points {health}";
    }

    public override IEnumerable Handle(PlayerStartsTurnGameEvent evt)
    {
        _oldHealth = root.game.player.data.healthPoints;
        SetHealthText(_oldHealth);
        return base.Handle(evt);
    }

    public override IEnumerable Handle(PlayerHealedGameEvent evt)
    {
        _oldHealth = root.game.player.data.healthPoints;
        SetHealthText(_oldHealth);
        return base.Handle(evt);
    }

    public override IEnumerable Handle(EnemyAttackGameEvent evt)
    {
        _oldHealth--;
        SetHealthText(_oldHealth);
        yield return new WaitForSecondsRealtime(.1f);
        hitPointText.transform.DOPunchScale(Vector3.one * .1f, .2f);
        
    }

    public override IEnumerable Handle(GameOverGameEvent evt)
    {
        gameOverGroup.alpha = 0;
        DOTween.To(() => hitPointText.alpha, v => hitPointText.alpha = v, 0, .1f);
        gameOverGroup.gameObject.SetActive(true);
        yield return DOTween.To(() => gameOverGroup.alpha, v => gameOverGroup.alpha = v, 1, .3f).WaitForCompletion();
        
    }
}
