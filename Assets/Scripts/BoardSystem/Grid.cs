using DAE.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DAE.BoardSystem
{
    public class Grid<TPosition>
    {

        public int Rows { get; }

        public int Columns { get; }

        public Grid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

        private BidirectionalDictionary<(int x, int y, int z), TPosition> _positions = new BidirectionalDictionary<(int, int, int), TPosition>();
        public bool TryGetPositionAt(int x, int y, int z, out TPosition position)
            => _positions.TryGetValue((x, y, z), out position);

        public bool TryGetCoordinateOf(TPosition position, out (int x, int y, int z) coordinate)
           => _positions.TryGetKey(position, out coordinate);


        public void Register(int x, int y, int z, TPosition position)
        {

#if UNITY_EDITOR

#endif

            Debug.Log((x, y, z));
            _positions.Add((x,y, z), position);
        }
    }
}
