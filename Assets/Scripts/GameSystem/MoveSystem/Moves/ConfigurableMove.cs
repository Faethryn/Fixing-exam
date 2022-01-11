using DAE.BoardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.GameSystem
{

    class ConfigurableMove<TPiece,TCard> : MoveBase<TPiece, TCard>
       where TPiece : IPiece
      where TCard : ICard
    {

        public delegate List<Tile> PositionCollector(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, TCard card, Tile position);


        private PositionCollector _positionCollector;

        public ConfigurableMove(PositionCollector positionCollector)
        {
            _positionCollector = positionCollector;
        }

        public override bool CanExecute(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, TCard card, Tile position)
        {
           if(board.TryGetPieceAt(position, out var toPosition))
            {
                return false;
            }
           else
            {
                return true;
            }
        }

        public override void Execute(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, Tile position, TCard card)
        {
            board.Move(piece, position);
            

        }

        public override List<Tile> Positions(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, TCard card, Tile position)
            => _positionCollector(board, grid, piece, card, position);
    }
}