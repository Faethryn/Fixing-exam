using DAE.BoardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.GameSystem
{


    interface IMove<TPiece, TCard>
       where TPiece : IPiece
        where TCard : ICard
    {
        bool CanExecute(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, TCard card);

        void Execute(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, Tile position, TCard card);

        List<Tile> Positions(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, TCard card);
    }
}