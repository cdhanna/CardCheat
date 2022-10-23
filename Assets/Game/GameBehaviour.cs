using System;
using System.Collections;
using System.Collections.Generic;
using BrewedInk.CardCheat.Core;
using BrewedInk.CardCheat.Data;
using BrewedInk.CardCheat.Game;
using UnityEngine;

public class GameBehaviour : GameListenerBehaviour
{
    [Header("Data")]
    public GameData data;
    public DeckData deck;

    [Header("References")] 
    public Grid grid;
    

    public List<GameListenerBehaviour> customComponents;

    [Header("Templates")]
    public PlayerBehaviour playerPrefab;
    public EnemyCardBehaviour enemyPrefab;

    private List<IGameListener> _components = new List<IGameListener>();
    // private Queue<GameEvent> _pendingEvents = new Queue<GameEvent>();

    private GameLogic _logic;

    [Header("Runtime State")] 
    public Game game;
    
    // Start is called before the first frame update
    void Start()
    {
        _logic = new GameLogic();
        game = data.Game;
        _logic.game = data.Game;
        _components.Add(_logic);
        _components.Add(this);
        foreach (var custom in customComponents)
        {
            custom.SetGame(game);
            _components.Add(custom);
        }
        StartCoroutine(ProcessEventJob());
        Create();
        
        game.SubmitEvent(new GameStartGameEvent());
        
    }

    public Bounds ComputeWorldBounds()
    {
        var b = game.ComputeBounds();
        var position = grid.CellToLocal(b.position);
        var otherSide = grid.CellToLocal(b.position + b.size);

        var diff = (otherSide - position);
        
        return new Bounds(position + diff * .5f, diff);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EventHandlerWrapper(IEnumerable handler, WaitForHandlers flag)
    {
        flag.expected++; // increase the expected count...
        try
        {
            foreach (var progress in handler)
            {
                yield return progress;
            }
        }
        finally
        {
            flag.actual++; // increase the completion count...
        }
    }
    
    IEnumerator ProcessEventJob()
    {
        var waiter = new WaitForHandlers();

        while (isActiveAndEnabled) // loop forever...
        {
            if (data.Game.HasEvents)
            {
                var handlers = new List<IEnumerable>();

                // var evts = data.Game.FlushEvents();
                var evts = new List<GameEvent> { data.Game.GetNextEvent() }; // TODO how do we decide to handle async?
                try
                {
                    foreach (var evt in evts)
                    {
                        foreach (var component in _components)
                        {
                            if (component == null) continue;
                            
                            var handler = component.Handle(evt);
                            handlers.Add(handler);
                        }
                    }
                }
                finally
                {
                    waiter.expected = 0;
                    waiter.actual = 0;
                }
                
                foreach (var handler in handlers)
                {
                    // kick off all handlers at the same time-ish... Let them run in parallel...
                    StartCoroutine(EventHandlerWrapper(handler, waiter));
                }
                
                // but before we process more events, all of the handlers need to complete.
                yield return waiter;
            }
            
            yield return null; // next frame...
        }
    }
    

    void Create()
    {
        // foreach (var cell in data.Game.board.cells)
        // {
        //     InstantiateComponent(tilePrefab, cell);
        // }

        foreach (var enemy in data.Game.enemies)
        {
            InstantiateComponent(enemyPrefab, enemy);
        }

        InstantiateComponent(playerPrefab, data.Game.player);
    }

    public TComponent InstantiateComponent<TComponent, TData>(TComponent prefab, TData elem) 
        where TComponent : GameComponent<TData>, IGameListener
    {
        var instance = Instantiate(prefab, transform);
        instance.Create(this, elem);
        _components.Add(instance);
        return instance;
    }

    public override IEnumerable Handle(EnemySpawnedGameEvent evt)
    {
        var enemy = InstantiateComponent(enemyPrefab, evt.Enemy); // TODO animation?
        enemy.Appear();
        yield return new WaitForSecondsRealtime(.05f);
        yield break;
    }


    public void Submit(GameEvent evt) => data.Game.SubmitEvent(evt);
    
    public class WaitForHandlers : CustomYieldInstruction
    {
        public int expected;
        public int actual;
        public WaitForHandlers()
        {
            
        }

        public override bool keepWaiting => actual < expected;
    }
    
}
