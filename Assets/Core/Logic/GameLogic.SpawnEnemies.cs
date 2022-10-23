using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace BrewedInk.CardCheat.Core
{
  public partial class GameLogic
  {
    public override IEnumerable Handle(SpawnEnemiesGameEvent evt)
    {


      var notPerfectThreshold = .02f;
      
      var spawns = GetSpawnOffsets();
      var remaining = evt.Number;
      foreach (var offset in spawns)
      {
        if (remaining <= 0)
        {
          break;
        }
        
        var pos = offset + game.player.position;
        // is there an enemy there?

        var r = UnityEngine.Random.value;
        if (r <= notPerfectThreshold)
        {
          notPerfectThreshold = .05f;
          continue;
        }

        notPerfectThreshold *= 1.5f;
        
        var cellFull = game.GetEnemyAtCell(pos, out _);
        if (cellFull) continue;

        var randomEnemy = new CardEnemy
        {
          card = new Card
          {
            suit = CardExtensions.Random<CardSuit>(),
            value = CardExtensions.RandomValue
          }
        };
        var gameEnemy = new GameCardEnemy
        {
          data = randomEnemy, position = pos
        };
        game.enemies.Add(gameEnemy);
        game.enemySpawnsLeftInLevel--;
        var spawnEvt = new EnemySpawnedGameEvent(gameEnemy);

        game.SubmitEvent(spawnEvt);
        remaining--;
      }

      yield break;
    }

    public IEnumerable<Vector2Int> GetSpawnOffsets(int maxIteration = 9999)
    {
      var spawnPositions = new List<Vector2Int>();
      var currentRank = 1;
      var spawnStrip = GetPositionsForRank(currentRank);
      spawnStrip.Shuffle();
      var spawnStripIndex = 0;

      while (maxIteration-- > 0)
      {
        if (spawnStripIndex >= spawnStrip.Count)
        {
          currentRank += 1;
          spawnStrip = GetPositionsForRank(currentRank);
          spawnStripIndex = 0;
        }

        var spawnPos = spawnStrip[spawnStripIndex];
        spawnStripIndex += 1;

        yield return spawnPos;
      }
    }



    public List<Vector2Int> GetPositionsForRank(int rank)
    {
      var positions = new List<Vector2Int>();
      void AddPos(int x, int y) => positions.Add(new Vector2Int(x, y));

      var p = 1; // similar to a "perfect number"
      var n = 2; // but start counting at 2, for some reason that I don't understand...
      var nearestAxis = p;
      while (p <= rank)
      {
        nearestAxis = p;
        p += n;
        n += 1;
      }

      var distance = n - 2;
      var axisDiff = rank - nearestAxis;

      if (axisDiff == 0)
      {
        // if the axisDiff is 0, we are perfectly aligned!!! There will only be 4 values...
        AddPos(distance, 0);
        AddPos(-distance, 0);
        AddPos(0, -distance);
        AddPos(0, distance);
      }
      else if (axisDiff == distance)
      {
        // we are on a perfect diag
        AddPos(distance, distance);
        AddPos(-distance, distance);
        AddPos(distance, -distance);
        AddPos(-distance, -distance);
      }
      else
      {
        // otherwise, there will be 8 values, offset from the axis by the diff in both negative and positive....
        var offset = axisDiff;

        AddPos(distance, offset);
        AddPos(distance, -offset);

        AddPos(-distance, offset);
        AddPos(-distance, -offset);

        AddPos(offset, distance);
        AddPos(-offset, distance);

        AddPos(offset, -distance);
        AddPos(-offset, -distance);
      }

      return positions;
    }


  }
}