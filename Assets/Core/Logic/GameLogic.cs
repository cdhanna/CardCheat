using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using log4net.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BrewedInk.CardCheat.Core
{

    
    public partial class GameLogic : GameListener
    {

        public Game game;


        public override IEnumerable Handle(CellClickedGameEvent evt)
        {
            if (game.turnState != TurnState.Player) yield break;

            game.hasSelectedPosition = true;
            game.selectedPosition = evt.Cell;

            foreach (var p in TryToDoSomething()) yield return p;
        }

        public override IEnumerable Handle(AbilityClickedGameEvent evt)
        {
            if (game.turnState != TurnState.Player) yield break;

            game.hasSelectedAbility = true;
            game.selectedAbility = evt.Ability;
            foreach (var p in TryToDoSomething()) yield return p;

        }

        public override IEnumerable Handle(DiscardSelectionGameEvent evt)
        {
            if (game.turnState != TurnState.Player) yield break;
            if (game.discardState != DiscardState.CanDiscard) yield break;
            
            
            if (!game.hasSelectedAbility) yield break; // nothing to do; no selection...
            
            game.SubmitEvent(new AbilityUsedGameEvent(game.selectedAbility));
            
            game.hasSelectedAbility = false;
            game.selectedAbility = null;
            game.discardCount++;

            if (game.discardCount == 2)
            {
                game.discardState = DiscardState.CanPickup;
                game.SubmitEvent(new CanDiscardGameEvent(false));
                game.SubmitEvent(new CanDrawSingleCardGameEvent(true));
            }
        }

        public override IEnumerable Handle(RequestPlayerDealAbilityGameEvent evt)
        {
            if (game.turnState != TurnState.Player) yield break;
            if (game.discardState != DiscardState.CanPickup) yield break;

            game.discardState = DiscardState.CanDiscard;
            game.discardCount = 0;
            DealOneCard();
            game.SubmitEvent(new CanDiscardGameEvent(true));
            game.SubmitEvent(new CanDrawSingleCardGameEvent(false));

        }

        IEnumerable TryToDoSomething()
        {
            if (!game.hasSelectedAbility)
            {
                // need an ability first... :/ 
                game.SubmitEvent(new AbilitySelectionRequiredGameEvent());
                yield break;
            }

            if (!game.hasSelectedPosition)
            {
                // need a target first
                game.SubmitEvent(new CellSelectionRequiredGameEvent());
                yield break;
            }
            
            // hey, we have two things!
            game.hasSelectedAbility = false;
            game.hasSelectedPosition = false;
            game.SubmitEvent(new AbilityUsageGameEvent(game.selectedPosition, game.selectedAbility));
        }

        public override IEnumerable Handle(PlayerMoveGameEvent evt)
        {
            game.player.position = evt.Position;
            return base.Handle(evt);
        }


        public override IEnumerable Handle(PlayerStartAttackGameEvent evt)
        {
            
            if (!game.player.data.hasCard)
            {
                // the player just gets to own this card
                game.SubmitEvent(new PlayerPickupGameEvent(evt.Target, evt.Enemy));
                yield break;
            }
            
            // we need to check if the player's card can beat the given card...
            // var isHighCard = game.player.data.holdCard.value >= evt.Enemy.card.value;
            // var isSuitMatch = game.player.data.holdCard.suit == evt.Enemy.card.suit;
            // if (isSuitMatch || isHighCard)
            // {
                // can win on high card
            game.SubmitEvent(new PlayerPickupGameEvent(evt.Target, evt.Enemy));
                
            // }
            
        }

        public override IEnumerable Handle(AbilityUsedGameEvent evt)
        {
            game.player.data.abilities.Remove(evt.Ability);

            // maybe we need to draw more cards.
            var curr = game.player.data.abilities;

            if (curr.Count == 0)
            {
                // need to deal the player some events...
                // game.SubmitEvent(new PlayerDealAbilitiesGameEvent());
                game.SubmitEvent(new PlayerEndsTurnGameEvent());
            }

            yield break;
        }


        public override IEnumerable Handle(PlayerDropGameEvent evt)
        {
            game.player.data.hasCard = false;
            return base.Handle(evt);
        }

        public override IEnumerable Handle(EnemyUpgradedGameEvent evt)
        {
            evt.Enemy.card.value++;
            if (evt.Enemy.card.value >= 12)
            {
                evt.Enemy.card.value = 12;
            }
            return base.Handle(evt);
        }

        public override IEnumerable Handle(PlayerPickupGameEvent evt)
        {
            game.enemies.RemoveAll(e => e.position == evt.Cell); // TODO maybe make this a method?
            var enemy = evt.Enemy;
            game.player.data.holdCard = enemy.card;
            game.player.data.hasCard = true;


            game.score += (enemy.card.value + 2) * game.combo;
            game.combo++;
            game.SubmitEvent(new ComboIncreasedGameEvent());
            game.SubmitEvent(new ScoreChangedGameEvent());
            
            
            game.SubmitEvent(new PlayerMoveGameEvent(evt.Cell));

            return base.Handle(evt);
        }

        public override IEnumerable Handle(PlayerStartsTurnGameEvent evt)
        {
            var curr = game.player.data.abilities;

            if (curr.Count < game.player.data.maxHandSize)
            {
                // need to deal the player some events...
                game.SubmitEvent(new PlayerDealAbilitiesGameEvent());
            }

            yield break;
        }

        public override IEnumerable Handle(PlayerDealAbilitiesGameEvent evt)
        {

            var abilities = game.player.data.abilities;
            var diff = game.player.data.maxHandSize - abilities.Count;

            for (var i = 0; i < diff; i++)
            {
                DealOneCard();
            }
            
            yield break;
            
        }

        public void DealOneCard()
        {
            var ability = new Ability(); // TODO how do we actually store the possibily? 
            
            var chance = game.GetRandomDraw();
            ability.type = chance.type;
                
            // draw the card, and add it to the player's inventory...
            game.player.data.abilities.Add(ability);

            game.SubmitEvent(new DrawAbilityGameEvent(ability));
        }

        public override IEnumerable Handle(BeginNewRoundGameEvent evt)
        {

            game.turnState = TurnState.Player;
            game.turnCount = 1;
            game.SubmitEvent(new TurnNumberChangedGameEvent());
            
            // spawn in some bad guys....
            game.enemySpawnsLeftInLevel = 18;
            var requiredEnemies = (int)(game.enemySpawnsLeftInLevel * .3f);

            game.SubmitEvent(new SpawnEnemiesGameEvent(requiredEnemies));
            game.SubmitEvent(new PlayerStartsTurnGameEvent());

            
            return base.Handle(evt);
        }

        public override IEnumerable Handle(GameStartGameEvent evt)
        {
            switch (game.turnState)
            {
                case TurnState.Start:
                    game.turnState = TurnState.Player;
                    game.turnCount = 1;
                    game.SubmitEvent(new TurnNumberChangedGameEvent());

                    // spawn in some bad guys....
                    game.enemySpawnsLeftInLevel = 18;
                    var requiredEnemies = (int)(game.enemySpawnsLeftInLevel * .3f);

                    game.SubmitEvent(new SpawnEnemiesGameEvent(requiredEnemies));
                    game.SubmitEvent(new PlayerStartsTurnGameEvent());
                    
                    break;
                default:
                    Debug.Log("Unknown state transition...");
                    break;
            }
            return base.Handle(evt);
        }
        
        public override IEnumerable Handle(PlayerEndsTurnGameEvent evt)
        {
            switch (game.turnState)
            {
                case TurnState.Player:
                    game.turnState = TurnState.Enemy;
                    game.SubmitEvent(new EnemyStartsTurnGameEvent());
                    
                    break;
                default:
                    Debug.Log("Unknown state transition...");
                    break;
            }
            return base.Handle(evt);
        }


        public override IEnumerable Handle(RoundOverGameEvent evt)
        {
            // TODO: anything interesting here???
            return base.Handle(evt);
        }

        public override IEnumerable Handle(EnemyEndsTurnGameEvent evt)
        {
            switch (game.turnState)
            {
                case TurnState.Enemy:
                    game.turnState = TurnState.Player;

                    
                    game.turnCount++;
                    game.SubmitEvent(new TurnNumberChangedGameEvent());

                    if (game.turnCount == 4)
                    {
                        // ah, the match is over, and we shouldn't do anything else...
                        
                        // need to pick up some new stuff....
                        
                        game.enemies.Clear();
                        game.hasSelectedAbility = false;
                        game.hasSelectedPosition = false;
                        game.player.data.hasCard = false;
                        game.player.data.abilities.Clear();
                        game.SubmitEvent(new RoundOverGameEvent());
                        yield break;
                    }
                    
                    
                    var requiredEnemies = 12;
                    switch (game.turnCount)
                    {
                        case 1:
                            requiredEnemies = (int)(game.enemySpawnsLeftInLevel * .3f);
                            break;
                        case 2:
                            requiredEnemies = (int)(game.enemySpawnsLeftInLevel * .5f);
                            break;
                        default:
                            requiredEnemies = game.enemySpawnsLeftInLevel;
                            break;
                    }
                    // requiredEnemies += (int)(2*Random.value + .5f);


                    
                    game.SubmitEvent(new SpawnEnemiesGameEvent(requiredEnemies));
                    game.SubmitEvent(new PlayerStartsTurnGameEvent());
                    break;
                default:
                    Debug.Log("Unknown state transition...");
                    break;
            }
            
        }

        public override IEnumerable Handle(PlayerDeathGameEvent evt)
        {
            game.turnState = TurnState.GameOver;
            game.SubmitEvent(new GameOverGameEvent());
            return base.Handle(evt);
        }

        public override IEnumerable Handle(EnemyStartsTurnGameEvent evt)
        {
            // lots of stuff will happen on an enemy turn... 
            // first, the enemies next to the player all need to attack.. 

            var neighbors = new HashSet<Vector2Int>(game.player.position.GetNeighbors());

            var movedEnemies = new HashSet<CardEnemy>(); // could make this a dict to have enemies make multiple moves...
            
            // the order in which the bad guys attack doesn't really matter
            foreach (var neighbor in neighbors)
            {
                if (!game.GetEnemyAtCell(neighbor, out var enemy)) continue;

                var playerValue = game.player.data.hasCard ? game.player.data.holdCard.value : -1;
                var isEnemyHigherThanPlayer = enemy.card.value > playerValue;
                if (isEnemyHigherThanPlayer)
                {
                    movedEnemies.Add(enemy);
                    game.player.data.healthPoints--;
                    game.combo = 1;
                    game.SubmitEvent(new ComboBrokenGameEvent());
                    game.SubmitEvent(new EnemyAttackGameEvent(enemy));
                    yield return null;
                }

                if (game.player.data.healthPoints == 0)
                {
                    // play death, and break out of the enemy movement response...
                    game.SubmitEvent(new PlayerDeathGameEvent());
                    yield break;
                }
            }

            // but the order in which the bad guys move _is_ important, because the player should be able to predict it.
            // IssueEnemyMoves(movedEnemies);
            game.SubmitEvent(new IssueEnemyMovementGameEvent(movedEnemies));
            
            // we are done moving the enemies... 
            game.SubmitEvent(new EnemyEndsTurnGameEvent());
            
            yield break;
        }

        public override IEnumerable Handle(IssueEnemyMovementGameEvent evt)
        {
            IssueEnemyMoves();
            yield break;
        }

        void IssueEnemyMoves(HashSet<CardEnemy> movedEnemies = null)
        {
            movedEnemies ??= new HashSet<CardEnemy>(); // could make this a dict to have enemies make multiple moves...

            var leftEdge = -100; // TODO: Get these bounds from somewhere meaningful...
            var rightEdge = 100;
            var topEdge = 100;
            var lowEdge = -100;
            
            for (var x = game.player.position.x - 1; x > leftEdge; x--)
            {
                var enemies = game.enemies.Where(e => e.position.x == x).ToList();
                foreach (var enemy in enemies)
                {
                    if (movedEnemies.Contains(enemy.data)) continue;

                    var nextPos = enemy.position + Vector2Int.right;

                    if (game.IsCellOpen(nextPos))
                    {
                        enemy.position = nextPos;
                        movedEnemies.Add(enemy.data);
                        game.SubmitEvent(new EnemyMoveGameEvent(nextPos, enemy.data));
  
                    }
                }
            }
            
                        
            for (var x = game.player.position.x + 1; x < rightEdge; x++)
            {
                var enemies = game.enemies.Where(e => e.position.x == x).ToList();
                foreach (var enemy in enemies)
                {
                    if (movedEnemies.Contains(enemy.data)) continue;

                    var nextPos = enemy.position + Vector2Int.left;

                    if (game.IsCellOpen(nextPos))
                    {
                        enemy.position = nextPos;
                        movedEnemies.Add(enemy.data);
                        game.SubmitEvent(new EnemyMoveGameEvent(nextPos, enemy.data));
                    }
                }
            }
            
            for (var x = game.player.position.y + 1; x < topEdge; x++)
            {
                var enemies = game.enemies.Where(e => e.position.y == x).ToList();
                foreach (var enemy in enemies)
                {
                    if (movedEnemies.Contains(enemy.data)) continue;

                    var nextPos = enemy.position + Vector2Int.down;

                    if (game.IsCellOpen(nextPos))
                    {
                        enemy.position = nextPos;
                        movedEnemies.Add(enemy.data);
                        game.SubmitEvent(new EnemyMoveGameEvent(nextPos, enemy.data));
                    }
                }
            }

            for (var x = game.player.position.y - 1; x > lowEdge; x--)
            {
                var enemies = game.enemies.Where(e => e.position.y == x).ToList();
                foreach (var enemy in enemies)
                {
                    if (movedEnemies.Contains(enemy.data)) continue;

                    var nextPos = enemy.position + Vector2Int.up;

                    if (game.IsCellOpen(nextPos))
                    {
                        enemy.position = nextPos;
                        movedEnemies.Add(enemy.data);
                        game.SubmitEvent(new EnemyMoveGameEvent(nextPos, enemy.data));
                    }
                }
            }
            
        }
    }
}