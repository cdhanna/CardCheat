using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrewedInk.CardCheat.Core
{
    public partial class GameLogic
    {
        
        public override IEnumerable Handle(AbilityUsageGameEvent evt)
        {
            var ability = game.selectedAbility;
            var target = evt.Target;

            var hasEnemy = game.GetEnemyAtCell(target, out var enemy);
            var diff = target - game.player.position;
            var isNeighbor = diff.magnitude <= 1.01f;
            
            
            switch (ability.type)
            {
                case (AbilityType _) when isNeighbor && !game.player.data.hasCard:
                case AbilityType.SUIT_MATCHER when isNeighbor && hasEnemy && game.player.data.holdCard.suit == enemy.card.suit:
                case AbilityType.HIGH_MATCHER when isNeighbor && hasEnemy && game.player.data.holdCard.value >= enemy.card.value:
                case AbilityType.LOW_MATCHER when isNeighbor && hasEnemy && game.player.data.holdCard.value <= enemy.card.value:
                    game.SubmitEvent(new PlayerStartAttackGameEvent(target, enemy));
                    game.SubmitEvent(new AbilityUsedGameEvent(ability));

                    break;
                case AbilityType.DRAW:
                    game.SubmitEvent(new PlayerDealAbilitiesGameEvent());
                    game.SubmitEvent(new AbilityUsedGameEvent(ability));

                    break;
                // case AbilityType.ENEMY_MOVE:
                //     game.SubmitEvent(new AbilityUsedGameEvent(ability));
                //     IssueEnemyMoves();
                //     break;
                case AbilityType.HEAL:
                    game.SubmitEvent(new AbilityUsedGameEvent(ability));

                    game.player.data.healthPoints++;
                    game.SubmitEvent(new PlayerHealedGameEvent());
                    
                    break;
                case AbilityType.SWAPPER when hasEnemy:

                    game.GetEnemyDataAtCell(target, out var teleEnemy);
                    teleEnemy.position = game.player.position;
                    game.player.position = target;
                    
                    game.SubmitEvent(new EnemyMoveGameEvent(teleEnemy.position, teleEnemy.data));
                    game.SubmitEvent(new PlayerMoveGameEvent(target));
                    game.SubmitEvent(new AbilityUsedGameEvent(ability));

                    break;
                case AbilityType.KNOCK_BACK when isNeighbor && hasEnemy:
                    
                    // move the enemy back a slot
                    var kickedToPos = target + diff;
                    var kickAnotherEnemy = game.GetEnemyDataAtCell(target, out var kickedEnemy);
                    var maxIter = 100;
                    while (kickAnotherEnemy && maxIter-- > 0)
                    {

                        game.SubmitEvent(new EnemyMoveGameEvent(kickedToPos, kickedEnemy.data));
                        kickAnotherEnemy = game.GetEnemyDataAtCell(kickedToPos, out var nextKickedEnemy);
                        kickedEnemy.position = kickedToPos;
                        kickedEnemy = nextKickedEnemy;

                        kickedToPos += diff;
                    }
                    
                    game.player.position = target;
                    game.SubmitEvent(new PlayerMoveGameEvent(target));
                    game.SubmitEvent(new AbilityUsedGameEvent(ability));

                    break;
                case AbilityType.ENEMY_REINFORCEMENTS:
                    game.SubmitEvent(new AbilityUsedGameEvent(ability));
                    
                    
                    
                    // TODO: how to spawn in more bad guys?
                    break;
                case AbilityType.ENEMY_UPGRADE:
                    game.SubmitEvent(new AbilityUsedGameEvent(ability));
                    // identify the enemies in the 3x3 grid around the target, and issue upgrade events...
                    for (var x = target.x - 1; x <= target.x + 1; x++)
                    {
                        for (var y = target.y - 1; y <= target.y + 1; y++)
                        {
                            var pos = new Vector2Int(x, y);
                            if (game.GetEnemyAtCell(pos, out var enemyAtPos))
                            {
                                game.SubmitEvent(new EnemyUpgradedGameEvent(pos, enemyAtPos));
                            }
                        }                        
                    }
                    
                    break;
                
                case (AbilityType _) when isNeighbor && !hasEnemy:
                    game.SubmitEvent(new PlayerMoveGameEvent(target));
                    game.SubmitEvent(new PlayerDropGameEvent(game.player.data.holdCard));
                    game.SubmitEvent(new AbilityUsedGameEvent(ability));

                    break;
                default:
                    Debug.LogError("Unknown ability usage... :( ");                    
                    
                    break;
            }
            
            game.SubmitEvent(new IssueEnemyMovementGameEvent());
            
            yield break;
        }
    }
}