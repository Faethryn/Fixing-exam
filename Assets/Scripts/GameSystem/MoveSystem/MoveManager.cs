using DAE.BoardSystem;
using DAE.Commons;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DAE.GameSystem
{

    public class MoveManager<TPiece, TCard>
        where TPiece : IPiece
        where TCard : ICard

    {

        private MultiValueDictionary<CardType, IMove<TPiece,TCard>> _moves = new MultiValueDictionary<CardType, IMove<TPiece,TCard>>();

        private readonly Board<Tile, TPiece> _board;
        private readonly Grid<Tile> _grid;

        public MoveManager(Board<Tile, TPiece> board, Grid<Tile> grid)
        {
            _board = board;
            _grid = grid;

            InitializeMoves();
        }

        public List<Tile> ValidPositionFor(TPiece piece,Tile position ,TCard card)
        {

            return _moves[card.cardType]
               .Where(m => m.CanExecute(_board, _grid, piece,card))
               .SelectMany(m => m.Positions(_board, _grid, piece,card))
               .ToList();
        }

        public List<Tile> IsolatedValidPositionFor(TPiece piece, Tile position, TCard card)
        {

            return _moves[card.cardType]
               .Where(m => m.CanExecute(_board, _grid, piece, card))
               .SelectMany(m => m.Positions(_board, _grid, piece, card))
               .ToList();
        }

        public void Move(TPiece piece, TCard card, Tile position)
        {
            _moves[card.cardType]
                .Where(m => m.CanExecute(_board, _grid, piece, card))
                .First(m => m.Positions(_board, _grid, piece,card).Contains(position))
                .Execute(_board, _grid, piece, position,card);
        }

        private void InitializeMoves()
        {
            _moves.Add(CardType.Bash, new ConfigurableMove<TPiece, TCard>(
                (Board<Tile, TPiece> b, Grid<Tile> g, TPiece p, TCard c) => new MovementHelper<TPiece, TCard>(b, g, p,c)
                                .NorthEast(1, MovementHelper<TPiece, TCard>.IsEmptyTile)
                                .NorthWest(1, MovementHelper<TPiece, TCard>.IsEmptyTile)
                                .East(1, MovementHelper<TPiece, TCard>.IsEmptyTile)
                                .West(1, MovementHelper<TPiece, TCard>.IsEmptyTile)
                                .SouthEast(1, MovementHelper<TPiece, TCard>.IsEmptyTile)
                                .SouthWest(1, MovementHelper<TPiece, TCard>.IsEmptyTile)
                                .Collect()));
            _moves.Add(CardType.Dash, new ConfigurableMove<TPiece, TCard>(
              (Board<Tile, TPiece> b, Grid<Tile> g, TPiece p, TCard c) => new MovementHelper<TPiece, TCard>(b, g, p, c)
                              .NorthEast(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                              .NorthWest(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                              .East(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                              .West(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                              .SouthEast(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                              .SouthWest(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                              .Collect()));
            _moves.Add(CardType.Warp, new ConfigurableMove<TPiece, TCard>(
            (Board<Tile, TPiece> b, Grid<Tile> g, TPiece p, TCard c) => new MovementHelper<TPiece, TCard>(b, g, p, c)
                            .NorthEast(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                            .NorthWest(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                            .East(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                            .West(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                            .SouthEast(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                            .SouthWest(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                            .Collect()));
            _moves.Add(CardType.Slash, new ConfigurableMove<TPiece, TCard>(
            (Board<Tile, TPiece> b, Grid<Tile> g, TPiece p, TCard c) => new MovementHelper<TPiece, TCard>(b, g, p, c)
                            .NorthEast(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                            .NorthWest(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                            .East(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                            .West(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                            .SouthEast(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                            .SouthWest(8, MovementHelper<TPiece, TCard>.IsEmptyTile)
                            .Collect()));

        }

    }
}