using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.StateSystem
{
    class PlayState : GameStateBase
    {
        private Component _camera;

        private GameObject _gameOverScreen;
      

        private int _currentPlayerID = 0;

        public PlayState(StateMachine<GameStateBase> stateMachine, Component gameCam, GameObject gameOverMenu) : base(stateMachine)
        {
            _camera = gameCam;
            _gameOverScreen = gameOverMenu;
        }


        internal override void GameOver()
        {
            GameObject.Destroy(_camera);
            _gameOverScreen.SetActive(true);
            StateMachine.MoveToState(GameState.GameOverState);
        }



    }
}