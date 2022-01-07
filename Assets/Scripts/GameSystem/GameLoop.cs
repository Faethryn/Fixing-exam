using DAE.BoardSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace DAE.GameSystem
{
    class GameLoop : MonoBehaviour
    {
        [SerializeField]
        private GameObject _dashCard;
        [SerializeField]
        private GameObject _bashCard;
        [SerializeField]
        private GameObject _slashCard;
        [SerializeField]
        private GameObject _warpCard;
        [SerializeField]
        private GameObject _playerHand;
        [SerializeField]
        private PositionHelper _positionHelper;

        //internal void DebugPosition(Tile tile)
        //{
        //    var (x,y) = _positionHelper.ToGridPostion(_grid, _boardParent, tile.transform.position);
        //    Debug.Log($"Value of Tile {tile.name} is X: {x} and Y: {y}");
        //}

        [SerializeField]
        private Transform _boardParent;

        public Card _currentCard = null;

        private MoveManager<Piece,Card> _moveManager;
       
        private Grid<Tile> _grid;
        private Board<Tile, Piece> _board;
        [SerializeField]
        private GameObject _player;

        public void Start()
        {
            _grid = new Grid<Tile>(8, 8);
            ConnectGrid(_grid);

            _board = new Board<Tile, Piece>();

            _moveManager = new MoveManager<Piece, Card>(_board,_grid);

            ConnectPiece(_grid,_board);

            DrawCard();
            DrawCard();
            DrawCard();
            DrawCard();
            DrawCard();

            _board.Moved += (s, e) =>
            {
                if (_grid.TryGetCoordinateOf(e.ToPosition, out var toCoordinate))
                {
                    var worldPosition =
                        _positionHelper.ToWorldPosition(
                            _grid, _boardParent, toCoordinate.x, toCoordinate.y);

                    e.Piece.MoveTo(worldPosition);

                }
            };

            _board.Placed += (s, e) =>
            {
                if (_grid.TryGetCoordinateOf(e.ToPosition, out var toCoordinate))
                {
                    var worldPosition =
                        _positionHelper.ToWorldPosition(
                            _grid, _boardParent, toCoordinate.x, toCoordinate.y);

                    e.Piece.Place(worldPosition);
                }
            };

            _board.Taken += (s, e) =>
            {
                e.Piece.Taken();
            };


        }



      

        private void ConnectGrid(Grid<Tile> grid)
        {
            var tiles = FindObjectsOfType<Tile>();
            foreach (var tile in tiles)
            {
                var position = new PositionModel();
                tile.Model = position;


                tile.DroppedOn += (s, e) =>
                {
                    var validPositions = _moveManager.ValidPositionFor(_player.GetComponent<Piece>(), e.Position, _currentCard);
                    var isolatedPositions = _moveManager.IsolatedValidPositionFor(_player.GetComponent<Piece>(), e.Position, _currentCard);

                    if (isolatedPositions.Contains(e.Position))
                    {
                        _moveManager.Move(_player.GetComponent<Piece>(), _currentCard, e.Position);

                        _currentCard.Used();

                        DrawCard();
                    foreach(var position in validPositions)
                    {
                        position.PositionDeactivated(this, e);
                    }

                    }


                };

                tile.Entered += (s, e) =>
                {
                    Debug.Log(  " tile was entered: "+ e.Position);
                    var validPositions = _moveManager.ValidPositionFor(_player.GetComponent<Piece>(), e.Position, _currentCard);
                    var isolatedPositions = _moveManager.IsolatedValidPositionFor(_player.GetComponent<Piece>(), e.Position, _currentCard);

                    if (isolatedPositions.Contains(e.Position))
                    {
                       

                       

                      

                    }
                    foreach (var position in validPositions)
                    {
                        position.PositionActivated(this, e);
                    }

                };

                tile.Exited += (s, e) =>
                {
                    var validPositions = _moveManager.ValidPositionFor(_player.GetComponent<Piece>(), e.Position, _currentCard);
                    var isolatedPositions = _moveManager.IsolatedValidPositionFor(_player.GetComponent<Piece>(), e.Position, _currentCard);

                    if (isolatedPositions.Contains(e.Position))
                    {
                       

                       

                        

                    }

                    foreach (var position in validPositions)
                    {
                        position.PositionDeactivated(this, e);
                    }

                };

                var (x, y ,z) = _positionHelper.ToGridPostion(grid, _boardParent, tile.transform.position);
                grid.Register(x, y, z, tile);


               
            }
        }

        private void ConnectPiece( Grid<Tile> grid, Board<Tile, Piece> board)
        {
            var pieces = FindObjectsOfType<Piece>();
            foreach( var piece in pieces)
            {
                var (x,y,z) = _positionHelper.ToGridPostion(grid,_boardParent, piece.gameObject.transform.position);
                if (grid.TryGetPositionAt(x, y, z, out var position))
                {
                    
                    board.Place(piece, position);


                }

               
            }
           
        }

        private void DrawCard()
        {
            //var randomNumber = new System.Random();

            //var cardNumber = randomNumber.Next(0, 4);
            var cardNumber = UnityEngine.Random.Range(0, 4);

            if (cardNumber == 0)
            {
               var card = Instantiate(_slashCard, _playerHand.transform);
                card.GetComponent<Card>().BeginCardDrag += (s, e)
                    => _currentCard = e.Card;
            }
            if (cardNumber == 1)
            {
                var card = Instantiate(_bashCard, _playerHand.transform);
                card.GetComponent<Card>().BeginCardDrag += (s, e)
                    => _currentCard = e.Card;
            }
            if (cardNumber == 2)
            {
                var card = Instantiate(_dashCard, _playerHand.transform);
                card.GetComponent<Card>().BeginCardDrag += (s, e)
                    => _currentCard = e.Card;

            }
            if (cardNumber == 3)
            {
                var card = Instantiate(_warpCard, _playerHand.transform);
                card.GetComponent<Card>().BeginCardDrag += (s, e)
                    => _currentCard = e.Card;
            }
        }
    }
}
