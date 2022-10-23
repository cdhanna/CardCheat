using System;
using System.Collections;
using System.Collections.Generic;
using BrewedInk.CardCheat.Core;
using UnityEngine;

namespace BrewedInk.CardCheat.Data
{
    [CreateAssetMenu(menuName = "Game/Game Data", order = -1000)]
    public class GameData : ScriptableObject
    {
        // use for editor setting and such...
        [SerializeField]
        private Game game;

        [NonSerialized]
        private Game _runtime = null;

        public Game Game
        {
            get
            {
                if (_runtime == null)
                {
                    _runtime = JsonUtility.FromJson<Game>(JsonUtility.ToJson(game));
                }

                return _runtime;
            }
        }

        #if UNITY_EDITOR
        public Game internalGameData => game;
        #endif
        
        public void ClearRuntime()
        {
            _runtime = null;
        }
    }
}