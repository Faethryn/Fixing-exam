using DAE.BoardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.GameSystem
{
    class BashMove<TPiece, TCard> : MoveBase<TPiece, TCard>
           where TPiece : IPiece
          where TCard : ICard
    {
        public delegate List<Tile> PositionCollector(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, TCard card, Tile position);


        private PositionCollector _positionCollector;

        public BashMove(PositionCollector positionCollector)
        {
            _positionCollector = positionCollector;
        }

        public override void Execute(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, Tile position, TCard card)
        {
            if (board.TryGetPieceAt(position, out var toPiece))
            {
                grid.TryGetCoordinateOf(position, out var enemyCoordinate);
                board.TryGetPositionOf(piece, out var playerTile);
                grid.TryGetCoordinateOf(playerTile, out var playerCoordinate);

               var xOffset = enemyCoordinate.x - playerCoordinate.x;
                var yOffset = enemyCoordinate.y - playerCoordinate.y;

                var nextXCoordinate =  enemyCoordinate.x + xOffset;
                var nextYCoordinate =  enemyCoordinate.y + yOffset;


                if (grid.TryGetPositionAt(nextXCoordinate, nextYCoordinate, out var nextTile))
                {
                    board.Move(toPiece, nextTile);

                }
                else
                {
                    board.Take(toPiece);
                }
               

            }
                


        }

        public override List<Tile> Positions(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, TCard card, Tile position)
            => _positionCollector(board, grid, piece, card, position);
    }
}