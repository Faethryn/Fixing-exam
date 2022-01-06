using DAE.BoardSystem;
using System;
using UnityEngine;

namespace DAE.GameSystem
{

    [CreateAssetMenu(menuName = "DAE/Position Helper")]
    public class PositionHelper  : ScriptableObject
    {

        private void OnValidate()
        {
            if (_tileDimension <= 0)
                _tileDimension = 1;
        }

        [SerializeField]
        private float _tileDimension = 1f; 

        public (int x, int y, int z) ToGridPostion(Grid<Tile> grid, Transform parent, Vector3 worldPosition)
        {
            var relativePosition = worldPosition - parent.position;
          

            var x = relativePosition.x;
            var y = relativePosition.z;
            Debug.DrawRay(new Vector3(x, 0, y), Vector3.down, Color.red, 10f);

            

            float q = (Mathf.Sqrt(3f)/3f *x -1f/3f * y)  ;
            float r = (2f/3f *y )  ;
            float s = -q *2 - r *2;

            //q = q / 2f;
            //r = r / 2f;
            


            var worldX = (float)(Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r);
            var worldY = (3f / 2f * r);

            Debug.DrawRay(new Vector3(worldX, 0, worldY), Vector3.down, Color.green, 10f);

            int intQ = (int)q;
            int intR = (int)r;
            int intS = (int)s;

            return (intQ, intR, intS);
        }

        public Vector3 ToWorldPosition(Grid<Tile> grid, Transform parent, int q, int r)
        {

            var x = (float)(Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r);
            var y = (3f / 2f * r);

            var worldPosition = new Vector3(x,0,y );

            return worldPosition;
        }
    }
}
