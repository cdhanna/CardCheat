using System.Collections;
using BrewedInk.CardCheat.Core;
using UnityEngine;

namespace BrewedInk.CardCheat.Game
{

    public abstract class GameComponent : GameListenerBehaviour
    {
        public GameBehaviour root;

        public void Create(GameBehaviour game)
        {
            root = game;
        }
    }
    
    public abstract class GameComponent<T> : GameComponent
    {
        public T data;

        public void Create(GameBehaviour game, T customData)
        {
            data = customData;
            base.Create(game);
            OnCreate();
        }

        public virtual void OnCreate()
        {
            
        }
    }
}