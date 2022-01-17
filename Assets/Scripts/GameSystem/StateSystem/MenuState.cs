using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.StateSystem
{


 class MenuState : GameStateBase
{
        private GameObject _menu;

        public MenuState(StateMachine<GameStateBase> stateMachine, GameObject menuTab) : base(stateMachine)
        {
            _menu = menuTab;
        }


        internal override void GameStart()
        {
            _menu.SetActive(false);
            StateMachine.MoveToState(GameState.GamePlayState);
        }
    }
}