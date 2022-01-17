using DAE.GameSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


[Serializable]
public class HighlightEvent : UnityEvent<bool> {}

class PieceEventArgs : EventArgs
{
    public Piece Piece { get; }

    public PieceEventArgs(Piece piece)
        => Piece = piece;
}

class Piece : MonoBehaviour, IPiece
{
    [SerializeField]
    private int _playerID;
    public int PlayerID => _playerID;

    [SerializeField]
    private GameObject _gameLoop;

    [SerializeField]
    private GameObject _gameOverScreen;
   

    public void MoveTo(Vector3 worldPosition)
    {
        transform.position = worldPosition;
    }
    public void Place(Vector3 worldPosition)
    {
        transform.position = worldPosition;
        gameObject.SetActive(true);
    }
    public void Taken()
    {
        if(PlayerID == 0)
        {
            Debug.Log("player tasks");
            //_gameLoop.GetComponent<GameLoop>().GameOverButton();
            _gameOverScreen.SetActive(true);
            _gameLoop.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}

