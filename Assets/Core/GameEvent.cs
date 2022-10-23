using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrewedInk.CardCheat.Core
{
    public class GameEvent
    {
        
    }
    
    
    public partial class GameHoverGameEvent : GameEvent
    {
        public Vector2Int Data { get; }

        public GameHoverGameEvent(Vector2Int data)
        {
            Data = data;
        }
    }

    public partial class CellClickedGameEvent : GameEvent
    {
        public Vector2Int Cell { get; }

        public CellClickedGameEvent(Vector2Int data)
        {
            Cell = data;
        }
    }

    public partial class AbilityClickedGameEvent : GameEvent
    {
        public Ability Ability { get; }

        public AbilityClickedGameEvent(Ability ability)
        {
            Ability = ability;
        }
    }

    public partial class PlayerMoveGameEvent : GameEvent
    {
        public Vector2Int Position { get; }

        public PlayerMoveGameEvent(Vector2Int data)
        {
            Position = data;
        }
    }

    public partial class PlayerStartAttackGameEvent : GameEvent
    {
        public Vector2Int Target { get; }
        public CardEnemy Enemy { get; }

        public PlayerStartAttackGameEvent(Vector2Int cell, CardEnemy enemy)
        {
            Target = cell;
            Enemy = enemy;
        }
    }

    public partial class PlayerPickupGameEvent : GameEvent
    {
        public Vector2Int Cell { get; }
        public CardEnemy Enemy { get; }

        public PlayerPickupGameEvent(Vector2Int cell, CardEnemy enemy)
        {
            Cell = cell;
            Enemy = enemy;
        }
    }

    public partial class PlayerDropGameEvent : GameEvent
    {
        public Card Card { get; }

        public PlayerDropGameEvent(Card card)
        {
            Card = card;
        }
    }

    public partial class GameStartGameEvent : GameEvent
    {
        
    }

    public partial class PlayerDeathGameEvent : GameEvent
    {
        
    }

    public partial class GameOverGameEvent : GameEvent
    {
        
    }

    public partial class PlayerEndsTurnGameEvent : GameEvent
    {
        
    }
    
    public partial class PlayerStartsTurnGameEvent : GameEvent
    {
        
    }

    public partial class TurnNumberChangedGameEvent : GameEvent
    {
        
    }

    public partial class RoundOverGameEvent : GameEvent
    {
        
    }

    public partial class EnemyStartsTurnGameEvent : GameEvent
    {
        
    }

    public partial class BeginNewRoundGameEvent : GameEvent
    {
        
    }
    

    public partial class EnemyEndsTurnGameEvent : GameEvent
    {
        
    }

    public partial class DiscardSelectionGameEvent : GameEvent
    {
        
    }
    
    
    public partial class CanDiscardGameEvent : GameEvent
    {
        public bool CanDiscard { get; }

        public CanDiscardGameEvent(bool canDiscard)
        {
            CanDiscard = canDiscard;
        }
    }

    public partial class CanDrawSingleCardGameEvent : GameEvent
    {
        public bool CanDraw { get; }

        public CanDrawSingleCardGameEvent(bool canDraw)
        {
            CanDraw = canDraw;
        }
    }

    public partial class EnemyAttackGameEvent : GameEvent
    {
        public CardEnemy Enemy { get; }

        public EnemyAttackGameEvent(CardEnemy enemy, int health=1)
        {
            Enemy = enemy;
        }
    }

    public partial class ScoreChangedGameEvent : GameEvent
    {
        
    }

    public partial class ComboIncreasedGameEvent : GameEvent
    {
        
    }

    public partial class ComboBrokenGameEvent : GameEvent
    {
        
    }

    public partial class SpawnEnemiesGameEvent : GameEvent
    {
        public int Number { get; }

        public SpawnEnemiesGameEvent(int number)
        {
            Number = number;
        }
    }

    public partial class EnemySpawnedGameEvent : GameEvent
    {
        public GameCardEnemy Enemy { get; }

        public EnemySpawnedGameEvent(GameCardEnemy enemy)
        {
            Enemy = enemy;
        }
    }
    

    public partial class IssueEnemyMovementGameEvent : GameEvent
    {
        public HashSet<CardEnemy> MovedEnemies { get; }

        public IssueEnemyMovementGameEvent(HashSet<CardEnemy> movedEnemies = null)
        {
            MovedEnemies = movedEnemies;
        }
    }
    
    
    public partial class EnemyMoveGameEvent : GameEvent
    {
        public Vector2Int Destination { get; }
        public CardEnemy Enemy { get; }

        public EnemyMoveGameEvent(Vector2Int destination, CardEnemy enemy)
        {
            Destination = destination;
            Enemy = enemy;
        }
    }

    public partial class PlayerDealAbilitiesGameEvent : GameEvent
    {
        
    }

    public partial class RequestPlayerDealAbilityGameEvent : GameEvent
    {
        
    }

    public partial class PlayerHealedGameEvent : GameEvent
    {
        
    }

    public partial class ProcessGambleCardGameEvent : GameEvent
    {
        public GambleCard Card { get; }

        public ProcessGambleCardGameEvent(GambleCard card)
        {
            Card = card;
        }
    }

    public partial class DrawAbilityGameEvent : GameEvent
    {
        public Ability Ability { get; }

        public DrawAbilityGameEvent(Ability ability)
        {
            Ability = ability;
        }
    }

    public partial class AbilitySelectionRequiredGameEvent : GameEvent
    {
        
        public AbilitySelectionRequiredGameEvent()
        {
        }
    }

    public partial class CellSelectionRequiredGameEvent : GameEvent
    {
        
    }

    public partial class AbilityUsageGameEvent : GameEvent
    {
        public Vector2Int Target { get; }
        public Ability Ability { get; }


        public AbilityUsageGameEvent(Vector2Int target, Ability ability)
        {
            Target = target;
            Ability = ability;
        }
    }

    public partial class AbilityUsedGameEvent : GameEvent
    {
        public Ability Ability { get; }

        public AbilityUsedGameEvent(Ability ability)
        {
            Ability = ability;
        }
    }

    public partial class EnemyUpgradedGameEvent : GameEvent
    {
        public Vector2Int Target { get; }
        public CardEnemy Enemy { get; }

        public EnemyUpgradedGameEvent(Vector2Int target, CardEnemy enemy)
        {
            Target = target;
            Enemy = enemy;
        }
    }
    
}