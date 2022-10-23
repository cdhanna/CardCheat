using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace BrewedInk.CardCheat.Core {
    
    [Serializable]
    public class Game
    {
        public GamePlayer player;
        public List<GameCardEnemy> enemies = new List<GameCardEnemy>();


        public bool hasSelectedPosition;
        public bool hasSelectedAbility;
        public Vector2Int selectedPosition;
        public Ability selectedAbility;
        public AbilityDeck deck;
        public TurnState turnState;
        public int randomSeed = -1;
        public int combo;
        public long score;
        public int enemySpawnsLeftInLevel;
        public int turnCount;
        public bool HasEvents => _pendingEvents.Count > 0;
        
        private Queue<GameEvent> _pendingEvents = new Queue<GameEvent>();

        private Random _random;
        public int discardCount;
        public DiscardState discardState;
        
        public Random Random => _random ??= randomSeed > 0 ? new Random(randomSeed) : new Random();
        
        public void SubmitEvent(GameEvent evt)
        {
            _pendingEvents.Enqueue(evt);
        }

        public GameEvent GetNextEvent()
        {
            return _pendingEvents.Dequeue();
        }

        public AbilityDrawChance GetRandomDraw()
        {
            // random weights...
            var r = Random.NextDouble();

            var weightSum = 0f;
            foreach (var chance in deck.drawChances)
            {
                weightSum += chance.weight;
            }

            var chanceStart = 0f;
            foreach (var chance in deck.drawChances)
            {
                var chanceDistance = chance.weight / weightSum;
                var chanceEnd = chanceStart + chanceDistance;
                if (r >= chanceStart & r < chanceEnd)
                {
                    return chance;
                }
                chanceStart = chanceEnd;

            }

            throw new Exception("Invalid chance random");
        }

        public bool IsCellOpen(Vector2Int position) => !GetEnemyAtCell(position, out _) && player.position != position;


        public BoundsInt ComputeBounds()
        {
            var b = new Bounds
            {
                size = Vector3.one,
                center = new Vector3(player.position.x, player.position.y)
            };
            foreach (var enemy in enemies)
            {
                b.Encapsulate(new Vector3(enemy.position.x, enemy.position.y));
            }
            return new BoundsInt((int)b.min.x, (int)b.min.y, (int)0, (int)b.size.x, (int)b.size.y, 0);
        }
        
        public bool GetEnemyAtCell(Vector2Int position, out CardEnemy enemy)
        {
            enemy = enemies.FirstOrDefault(e => e.position == position)?.data;
            return enemy != null;
        }
        
        public bool GetEnemyDataAtCell(Vector2Int position, out GameCardEnemy enemy)
        {
            enemy = enemies.FirstOrDefault(e => e.position == position);
            return enemy != null;
        }
        
    }

    public enum TurnState
    {
        Start, 
        Player,
        Enemy,
        GameOver
    }


    public enum DiscardState
    {
        CanDiscard,
        CanPickup
    }
    
    [Serializable]
    public class Card
    {
        public int value;
        public CardSuit suit;
    }

    public enum CardSuit
    {
        Clubs, Hearts, Diamonds, Spades
    }

    public static class CardExtensions
    {
        public static TEnum Random<TEnum>() where TEnum : Enum
        {
            var values = Enum.GetValues(typeof(TEnum));
            var value = (int)(UnityEngine.Random.value * values.Length);
            return (TEnum)values.GetValue(value % values.Length);
        }

        public static CardSuit RandomSuit => Random<CardSuit>();
        public static int RandomValue => UnityEngine.Random.Range(0, 13);
    }

    [Serializable]
    public class CardSuitArt
    {
        public Sprite symbol;
        public Color color;
    }

    [Serializable]
    public class GamePiece<T> where T : Piece
    {
        public T data;
        public Vector2Int position;
    }
    
    public abstract class Piece
    {
    }

    [Serializable]
    public class GamePlayer : GamePiece<Player>
    {
        
    }
    
    [Serializable]
    public class GameCardEnemy : GamePiece<CardEnemy>
    {
    }

    [Serializable]
    public class Player : Piece
    {
        public List<Ability> abilities = new List<Ability>();
        public int maxHandSize = 3;
        public Card holdCard;
        public bool hasCard;
        public int healthPoints;
    }

    [Serializable]
    public class CardEnemy : Piece
    {
        public Card card;
    }
    
    [Serializable]
    public class Ability
    {
        public AbilityType type;

    }

    [Serializable]
    public class AbilityDeck
    {
        public List<AbilityDrawChance> drawChances = new List<AbilityDrawChance>();
    }

    [Serializable]
    public class AbilityDrawChance
    {
        public AbilityType type;
        public int weight;
    }

    public enum AbilityType
    {
        HIGH_MATCHER,
        SUIT_MATCHER,
        LOW_MATCHER,
        ENEMY_MOVE, // all enemies move closer...
        ENEMY_UPGRADE, // all enemies near the player upgrade their value,
        ENEMY_REINFORCEMENTS, // a wave of new enemies appear
        SHUFFLER, // all cards re-arrange
        SWAPPER, // swap positions with the given card
        DRAW, // draw a new hand
        DISCARD, // discard a chosen card,
        DOUBLE_USE, // deal a duplicate of a card in your hand
        KNOCK_BACK, // kick a card back, 
        HEAL, // re-gain a hit point
    }




}
