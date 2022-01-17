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
        private MultiValueDictionary<CardType, IMove<TPiece, TCard>> _adjustedMoves = new MultiValueDictionary<CardType, IMove<TPiece, TCard>>();

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
               .Where(m => m.CanExecute(_board, _grid, piece,card, position))
               .SelectMany(m => m.Positions(_board, _grid, piece,card,position))
               .ToList();
        }

        public List<Tile> IsolatedValidPositionFor(TPiece piece, Tile position, TCard card)
        {

            return _adjustedMoves[card.cardType]
               .Where(m => m.CanExecute(_board, _grid, piece, card, position))
               .SelectMany(m => m.Positions(_board, _grid, piece, card,position))
               .ToList();
        }

        public void Move(TPiece piece, TCard card, Tile position)
        {
            _adjustedMoves[card.cardType]
                .Where(m => m.CanExecute(_board, _grid, piece, card, position))
                .First(m => m.Positions(_board, _grid, piece, card, position).Contains(position))
                .Execute(_board, _grid, piece, position,card);
        }

       

        private void InitializeMoves()
        {
            _moves.Add(CardType.Bash, new BashMove<TPiece, TCard>(
                (Board<Tile, TPiece> b, Grid<Tile> g, TPiece p, TCard c, Tile e) => new MovementHelper<TPiece, TCard>(b, g, p,c, e)
                                .NorthEast(1)
                                .NorthWest(1)
                                .East(1)
                                .West(1)
                                .SouthEast(1)
                                .SouthWest(1)
                                .Collect()));
            _moves.Add(CardType.Beam, new SlashMove<TPiece, TCard>(
              (Board<Tile, TPiece> b, Grid<Tile> g, TPiece p, TCard c, Tile e) => new MovementHelper<TPiece, TCard>(b, g, p, c, e)
                              .NorthEast( )
                              .NorthWest()
                              .East()
                              .West()
                              .SouthEast()
                              .SouthWest()
                              .Collect()));
            _moves.Add(CardType.Warp, new ConfigurableMove<TPiece, TCard>(
            (Board<Tile, TPiece> b, Grid<Tile> g, TPiece p, TCard c, Tile e) => new MovementHelper<TPiece, TCard>(b, g, p, c, e)
                            .Warp()
                            .Collect()));
            _moves.Add(CardType.Slash, new SlashMove<TPiece, TCard>(
            (Board<Tile, TPiece> b, Grid<Tile> g, TPiece p, TCard c, Tile e) => new MovementHelper<TPiece, TCard>(b, g, p, c, e)
                            .NorthEast(1)
                            .NorthWest(1)
                            .East(1)
                            .West(1)
                            .SouthEast(1)
                            .SouthWest(1)
                            .Collect()));
            _moves.Add(CardType.Bomb, new BombMove<TPiece, TCard>(
          (Board<Tile, TPiece> b, Grid<Tile> g, TPiece p, TCard c, Tile e) => new MovementHelper<TPiece, TCard>(b, g, p, c, e)
                          .NorthEastBomb(1)
                          .NorthWestBomb(1)
                          .EastBomb(1)
                          .WestBomb(1)
                          .SouthEastBomb(1)
                          .SouthWestBomb(1)
                          .Bomb()
                          .Collect()));


            _adjustedMoves.Add(CardType.Slash, new SlashMove<TPiece, TCard>(
            (Board<Tile, TPiece> b, Grid<Tile> g, TPiece p, TCard c, Tile e) => new MovementHelper<TPiece, TCard>(b, g, p, c, e)
                            .BashNorthEast(1)
                            .BashNorthWest(1)
                            .BashEast(1)
                            .BashWest(1)
                            .BashSouthEast(1)
                            .BashSouthWest(1)
                            .Collect()));

            _adjustedMoves.Add(CardType.Warp, new ConfigurableMove<TPiece, TCard>(
           (Board<Tile, TPiece> b, Grid<Tile> g, TPiece p, TCard c, Tile e) => new MovementHelper<TPiece, TCard>(b, g, p, c, e)
                           .Warp()
                           .Collect()));
            _adjustedMoves.Add(CardType.Beam, new SlashMove<TPiece, TCard>(
           (Board<Tile, TPiece> b, Grid<Tile> g, TPiece p, TCard c, Tile e) => new MovementHelper<TPiece, TCard>(b, g, p, c, e)
                           .BeamNorthEast()
                           .BeamNorthWest()
                           .BeamEast()
                           .BeamWest()
                           .BeamSouthEast()
                           .BeamSouthWest()
                           .Collect()));
            _adjustedMoves.Add(CardType.Bash, new BashMove<TPiece, TCard>(
           (Board<Tile, TPiece> b, Grid<Tile> g, TPiece p, TCard c, Tile e) => new MovementHelper<TPiece, TCard>(b, g, p, c, e)
                           .BashNorthEast(1)
                           .BashNorthWest(1)
                           .BashEast(1)
                           .BashWest(1)
                           .BashSouthEast(1)
                           .BashSouthWest(1)
                           .Collect()));
            _adjustedMoves.Add(CardType.Bomb, new BombMove<TPiece, TCard>(
        (Board<Tile, TPiece> b, Grid<Tile> g, TPiece p, TCard c, Tile e) => new MovementHelper<TPiece, TCard>(b, g, p, c, e)
                        .NorthEastBomb(1)
                        .NorthWestBomb(1)
                        .EastBomb(1)
                        .WestBomb(1)
                        .SouthEastBomb(1)
                        .SouthWestBomb(1)
                        .Bomb()
                        .Collect()));

        }

    }
}