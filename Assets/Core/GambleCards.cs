using System.Collections;

namespace BrewedInk.CardCheat.Core
{
    public class GambleCard : GameListenerBehaviour
    {
        public string title;
        public string description;
        public GambleCardState state;
        public bool processImmediately;
        
        
        public virtual IEnumerable MarkAsDefeat()
        {
            state = GambleCardState.LOSS;
            if (processImmediately)
            {
                Game.SubmitEvent(new ProcessGambleCardGameEvent());
            }
            yield break;
        }

        public virtual IEnumerable MarkAsSuccess()
        {
            state = GambleCardState.WIN;
            yield break;
        }

        public IEnumerable Process()
        {
            if (state == GambleCardState.WIN)
            {
                foreach (var x in GiveRewards())
                {
                    yield return x;
                }
            } else
            {
                foreach (var x in GivePenalty())
                {
                    yield return x;
                }
            }
        }
        
        protected virtual IEnumerable GiveRewards()
        {
            yield break;
        } 
        
        
        protected virtual IEnumerable GivePenalty()
        {
            yield break;
        } 
    }
    
    public enum GambleCardState {
        PENDING, LOSS, WIN
    }

    public class TakeNoDamageGamble : GambleCard
    {
        public override IEnumerable Handle(EnemyAttackGameEvent evt)
        {
            // bummer...
            foreach (var x in MarkAsDefeat()) yield return x;
        }
    }

}