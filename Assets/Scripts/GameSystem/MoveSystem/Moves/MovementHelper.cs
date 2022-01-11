using DAE.BoardSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace DAE.GameSystem
{

    class MovementHelper<TPiece, TCard>
       where TPiece : IPiece
        where TCard : ICard
    {
        private Board<Tile, TPiece> _board;
        private Grid<Tile> _grid;
        private TPiece _piece;
        private TCard _card;
        private List<Tile> _validPositions = new List<Tile>();
        private Tile _tile;

        public MovementHelper(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, TCard card, Tile position)
        {
            _board = board;
            _grid = grid;
            _piece = piece;
            _card = card;
            _tile = position;
        }


        #region move
        public MovementHelper<TPiece ,TCard> NorthEast(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(1, 0, 0 , numTiles, validators);
        public MovementHelper<TPiece, TCard> NorthWest(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(-1, 1, 4, numTiles, validators);

        public MovementHelper<TPiece, TCard> East(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(0, 1, 5 ,numTiles, validators);

        public MovementHelper<TPiece, TCard> SouthEast(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(1, -1,1  ,numTiles, validators);

        public MovementHelper<TPiece, TCard> SouthWest(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(0, -1,2  ,numTiles, validators);

        public MovementHelper<TPiece, TCard> West(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(-1, 0, 3, numTiles, validators);
        #endregion

        #region Bash

        public MovementHelper<TPiece, TCard> BashNorthEast(int numTiles = int.MaxValue, params Validator[] validators)
          => BashAndDash(1, 0, 0, numTiles, validators);
        public MovementHelper<TPiece, TCard> BashNorthWest(int numTiles = int.MaxValue, params Validator[] validators)
            => BashAndDash(-1, 1, 4, numTiles, validators);

        public MovementHelper<TPiece, TCard> BashEast(int numTiles = int.MaxValue, params Validator[] validators)
            => BashAndDash(0, 1, 5, numTiles, validators);

        public MovementHelper<TPiece, TCard> BashSouthEast(int numTiles = int.MaxValue, params Validator[] validators)
            => BashAndDash(1, -1, 1, numTiles, validators);

        public MovementHelper<TPiece, TCard> BashSouthWest(int numTiles = int.MaxValue, params Validator[] validators)
            => BashAndDash(0, -1, 2, numTiles, validators);

        public MovementHelper<TPiece, TCard> BashWest(int numTiles = int.MaxValue, params Validator[] validators)
            => BashAndDash(-1, 0, 3, numTiles, validators);


        #endregion

        #region beam
        public MovementHelper<TPiece, TCard> BeamNorthEast(int numTiles = int.MaxValue, params Validator[] validators)
         => Beam(1, 0, 0, numTiles, validators);
        public MovementHelper<TPiece, TCard> BeamNorthWest(int numTiles = int.MaxValue, params Validator[] validators)
            => Beam(-1, 1, 4, numTiles, validators);

        public MovementHelper<TPiece, TCard> BeamEast(int numTiles = int.MaxValue, params Validator[] validators)
            => Beam(0, 1, 5, numTiles, validators);

        public MovementHelper<TPiece, TCard> BeamSouthEast(int numTiles = int.MaxValue, params Validator[] validators)
            => Beam(1, -1, 1, numTiles, validators);

        public MovementHelper<TPiece, TCard> BeamSouthWest(int numTiles = int.MaxValue, params Validator[] validators)
            => Beam(0, -1, 2, numTiles, validators);

        public MovementHelper<TPiece, TCard> BeamWest(int numTiles = int.MaxValue, params Validator[] validators)
            => Beam(-1, 0, 3, numTiles, validators);

        #endregion

        public delegate bool Validator(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, Tile position);

        public    MovementHelper<TPiece, TCard> Warp()
        {
           
            
            _validPositions.Add(_tile);

            
            return this;
        }
        public MovementHelper<TPiece, TCard> BashAndDash(int xOffset, int yOffset,int direction, int numTiles = int.MaxValue, params Validator[] validators)
        {
            List<Tile> tempValidTiles = new List<Tile>();




            if (!_board.TryGetPositionOf(_piece, out var position))
                return this;

            if (!_grid.TryGetCoordinateOf(position, out var coordinate))
                return this;

            var nextXCoordinate = coordinate.x + xOffset ;
            var nextYCoordinate = coordinate.y + yOffset;

            var hasNextPosition = _grid.TryGetPositionAt(nextXCoordinate, nextYCoordinate, out var nextPosition);
            var isOk = validators.All((v) => v(_board, _grid, _piece, nextPosition));
            if (!isOk)
                return this;

            tempValidTiles.Add(nextPosition);

            if (tempValidTiles.Contains(_tile))
            {
                foreach (var tempPosition in tempValidTiles)
                {

                    _validPositions.Add(tempPosition);
                }
            }
            else
            {
                return this;
            }

            var otherPosition1 = GetNextDirectionDown(direction);
            var otherPosition1XCoordinate = coordinate.x + (int)otherPosition1.x;
            var otherPosition1YCoordinate = coordinate.y + (int)otherPosition1.y;

            _grid.TryGetPositionAt(otherPosition1XCoordinate, otherPosition1YCoordinate, out var otherPosition1NextPosition);

            var isOtherPosition1OK = validators.All(v => v(_board, _grid, _piece,otherPosition1NextPosition));

            if(!isOtherPosition1OK)
            {
                return this;
            }

            _validPositions.Add(otherPosition1NextPosition);

            var otherPosition2 = GetNextDirectionUp(direction);
            var otherPosition2XCoordinate = coordinate.x + (int)otherPosition2.x;
            var otherPosition2YCoordinate = coordinate.y + (int)otherPosition2.y;

            _grid.TryGetPositionAt(otherPosition2XCoordinate, otherPosition2YCoordinate, out var otherPosition2NextPosition);

            var isOtherPosition2OK = validators.All(v => v(_board, _grid, _piece, otherPosition2NextPosition));

            if (!isOtherPosition2OK)
            {
                return this;
            }

            _validPositions.Add(otherPosition2NextPosition);

           

            return this;
        }

        public MovementHelper<TPiece, TCard> Move(int xOffset, int yOffset, int direction, int numTiles = int.MaxValue, params Validator[] validators)
        {
           




            if (!_board.TryGetPositionOf(_piece, out var position))
                return this;

            if (!_grid.TryGetCoordinateOf(position, out var coordinate))
                return this;

            var nextXCoordinate = coordinate.x + xOffset;
            var nextYCoordinate = coordinate.y + yOffset;





            var hasNextPosition = _grid.TryGetPositionAt(nextXCoordinate, nextYCoordinate, out var nextPosition);
            int step = 0;
            while (hasNextPosition && step < numTiles)
            {

                var isOk = validators.All((v) => v(_board, _grid, _piece, nextPosition));
                if (!isOk)
                    return this;

                var hasPiece = _board.TryGetPieceAt(nextPosition, out var nextPiece);
                if (!hasPiece)
                {
                    _validPositions.Add(nextPosition);
                }
                else
                {
                    if (nextPiece.PlayerID != _piece.PlayerID)
                    {
                        _validPositions.Add(nextPosition);

                    }

                    //return this;
                }

                nextXCoordinate += xOffset;
                nextYCoordinate += yOffset;




                hasNextPosition = _grid.TryGetPositionAt(nextXCoordinate, nextYCoordinate, out nextPosition);

                step++;
            }

            return this;
        }

        public MovementHelper<TPiece, TCard> Beam(int xOffset, int yOffset, int direction, int numTiles = int.MaxValue, params Validator[] validators)
        {
            List<Tile> tempValidTiles = new List<Tile>();
           



            if (!_board.TryGetPositionOf(_piece, out var position))
                return this;

            if (!_grid.TryGetCoordinateOf(position, out var coordinate))
                return this;

            var nextXCoordinate = coordinate.x + xOffset;
            var nextYCoordinate = coordinate.y + yOffset;

            var hasNextPosition = _grid.TryGetPositionAt(nextXCoordinate, nextYCoordinate, out var nextPosition);
            var isOk = validators.All((v) => v(_board, _grid, _piece, nextPosition));
           

            tempValidTiles.Add(nextPosition);

           
            
                int step = 0;
                while (hasNextPosition && step < numTiles )
                {

                    isOk = validators.All((v) => v(_board, _grid, _piece, nextPosition));
                    if (!isOk)
                        return this;

                    var hasPiece = _board.TryGetPieceAt(nextPosition, out var nextPiece);
                    if (!hasPiece)
                    {
                        
                    tempValidTiles.Add(nextPosition);
                    }
                    else
                    {
                        if (nextPiece.PlayerID != _piece.PlayerID)
                        {
                           
                            tempValidTiles.Add(nextPosition);
                        }

                        //return this;
                    }

                    nextXCoordinate += xOffset;
                    nextYCoordinate += yOffset;




                    hasNextPosition = _grid.TryGetPositionAt(nextXCoordinate, nextYCoordinate, out nextPosition);

                    step++;
                }

            if(tempValidTiles.Contains(_tile))
            {
                foreach(Tile tile in tempValidTiles)
                {
                    _validPositions.Add(tile);

                }
            }
            else
            {
                return this;
            }
            
           

          

            

            return this;
        }

        public Vector2 GetNextDirectionDown(int CurrentDirection)
        {
            if(CurrentDirection -1 == -1)
            {
                return _directions[5];
            }
            else
            {
                return _directions[CurrentDirection - 1];
            }
        }

        public Vector2 GetNextDirectionUp(int CurrentDirection)
        {
            if (CurrentDirection + 1 == 6)
            {
                return _directions[0];
            }
            else
            {
                return _directions[CurrentDirection + 1];
            }
        }


        static public Vector2[] _directions =
            new Vector2[6] { new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, 1) };

        public List<Tile> Collect()
        {
            return _validPositions;
        }

        public static bool IsEmptyTile(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, Tile position)
            => !board.TryGetPieceAt(position, out _);

        public static bool HasEnemyPiece(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, Tile position)
            => board.TryGetPieceAt(position, out var enemyPiece) && enemyPiece.PlayerID != piece.PlayerID;


    }
}