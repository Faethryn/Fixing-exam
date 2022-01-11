using DAE.BoardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.GameSystem
{ 
 class SlashMove<TPiece, TCard> : MoveBase<TPiece, TCard>
       where TPiece : IPiece
      where TCard : ICard
    {
        public delegate List<Tile> PositionCollector(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, TCard card, Tile position);


        private PositionCollector _positionCollector;

        public SlashMove(PositionCollector positionCollector)
        {
            _positionCollector = positionCollector;
        }

        public override void Execute(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, Tile position, TCard card)
        {
            if (board.TryGetPieceAt(position, out var toPiece))
                board.Take(toPiece);

          
        }

        public override List<Tile> Positions(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, TCard card, Tile position)
            => _positionCollector(board, grid, piece, card, position);
    }
}