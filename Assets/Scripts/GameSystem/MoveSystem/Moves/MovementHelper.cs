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

        public MovementHelper(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, TCard card)
        {
            _board = board;
            _grid = grid;
            _piece = piece;
            _card = card;
        }



        public MovementHelper<TPiece ,TCard> NorthEast(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(0, 1, -2, numTiles, validators);

        public MovementHelper<TPiece, TCard> East(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(1, 0, -2, numTiles, validators);

        public MovementHelper<TPiece, TCard> SouthEast(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(1, -1, 0, numTiles, validators);

        public MovementHelper<TPiece, TCard> SouthWest(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(0, -1, 2, numTiles, validators);

        public MovementHelper<TPiece, TCard> West(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(-1, 0, 2, numTiles, validators);

        public MovementHelper<TPiece, TCard> NorthWest(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(-1, 1, 0, numTiles, validators);

        public delegate bool Validator(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, Tile position);

        public MovementHelper<TPiece, TCard> Move(int xOffset, int yOffset, int zOffset, int numTiles = int.MaxValue, params Validator[] validators)
        {




            if (!_board.TryGetPositionOf(_piece, out var position))
                return this;

            if (!_grid.TryGetCoordinateOf(position, out var coordinate))
                return this;

            var nextXCoordinate = coordinate.x + xOffset;
            var nextYCoordinate = coordinate.y + yOffset;
            var nextZCoordinate = coordinate.z;

            var hasNextPosition = _grid.TryGetPositionAt(nextXCoordinate, nextYCoordinate, nextZCoordinate, out var nextPosition);
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
                    if (nextPiece.PlayerID == _piece.PlayerID)
                        return this;

                    _validPositions.Add(nextPosition);
                    return this;
                }

                nextXCoordinate = coordinate.x;
                nextYCoordinate = coordinate.y;
                nextZCoordinate = coordinate.z;

                hasNextPosition = _grid.TryGetPositionAt(nextXCoordinate, nextYCoordinate, nextZCoordinate, out nextPosition);

                step++;
            }

            return this;
        }

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