

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAE.StateSystem
{
    class GameStateBase : IState<GameStateBase>
    {
        public StateMachine<GameStateBase> StateMachine { get; }

        public GameStateBase(StateMachine<GameStateBase> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void OnEnter()
        {   
        }

        public virtual void OnExit()
        {    
        }

       internal virtual void GameOver()
        {

        }

        internal virtual void GameStart()
        {

        }

        internal virtual void Forward()
        {

        }

        internal virtual void Backward()
        {

        }
    }
}
