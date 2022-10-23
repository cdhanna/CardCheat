using System.Collections;

namespace BrewedInk.CardCheat.Core
{
    public partial class GameListener : IGameListener
    {
        
    }

    public partial class GameListenerBehaviour : IGameListener
    {
        public Game Game { get; set; }

        public virtual void SetGame(Game game)
        {
            Game = game;
        }
    }
    
    public partial interface IGameListener
    {
        
    }
}