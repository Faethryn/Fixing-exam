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

        public delegate List<Tile> PositionCollector(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, TCard card);


        private PositionCollector _positionCollector;

        public ConfigurableMove(PositionCollector positionCollector)
        {
            _positionCollector = positionCollector;
        }

        public override List<Tile> Positions(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, TCard card)
            => _positionCollector(board, grid, piece, card);
    }
}