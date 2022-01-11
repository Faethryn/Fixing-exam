using DAE.BoardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace DAE.GameSystem
{

    abstract class MoveBase<TPiece, TCard> : IMove<TPiece, TCard>
       where TPiece : IPiece
        where TCard : ICard
    {
        

        public virtual bool CanExecute(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, TCard card, Tile position)
        {
            return true;
        }

       

        public virtual void Execute(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, Tile position, TCard card)
        {
            if (board.TryGetPieceAt(position, out var toPiece))
                board.Take(toPiece);

            board.Move(piece, position);
        }

        public abstract List<Tile> Positions(Board<Tile, TPiece> board, Grid<Tile> grid, TPiece piece, TCard card, Tile position);

    }
}