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
            //var relativePosition = worldPosition - parent.position;


            //var x = relativePosition.x;
            //var y = relativePosition.z;
            //Debug.DrawRay(new Vector3(x, 0, y), Vector3.down, Color.red, 10f);



            //float q = (Mathf.Sqrt(3f)/3f *x -1f/3f * y)  ;
            //float r = (2f/3f *y )  ;
            //float s = -q *2 - r *2;

            //q = q / 2f;
            //r = r / 2f;

            var q = ((2f / 3f) * worldPosition.x) / 1;
            var r = ((-1f / 3f) * worldPosition.x) + (Mathf.Sqrt(3f) / 3f * worldPosition.z) / 1;



            int x = (int)Mathf.Round(q);
            int y = (int)Mathf.Round(r);
            int intS = -x - y;

            var worldY = ((3f / 2f) * r);
            var worldX = (float)(Mathf.Sqrt(3f) / 2f * r + Mathf.Sqrt(3f) * q);

            Debug.DrawRay(new Vector3(worldX, 0, worldY), Vector3.down, Color.green, 10f);

            return (x, y, intS);
        }

        public Vector3 ToWorldPosition(Grid<Tile> grid, Transform parent, int q, int r)
        {

            var y = ((3f / 2f) * r);
            var x = (float)(Mathf.Sqrt(3f) / 2f * r + Mathf.Sqrt(3f) * q);

            var worldPosition = new Vector3(x,0,y );

            return worldPosition;
        }
    }
}
