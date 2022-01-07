using DAE.BoardSystem;
using System;
using UnityEngine;

namespace DAE.GameSystem
{

    [CreateAssetMenu(menuName = "DAE/Position Helper")]
    public class PositionHelper  : ScriptableObject
    {
        public float TileRadius;

        public Vector2 ToGridPosition(Vector3 worldPosition)
        {

            var q = ((2f / 3f) * worldPosition.x) / TileRadius;
            var r = ((-1f / 3f) * worldPosition.x) + (Mathf.Sqrt(3f) / 3f * worldPosition.z) / TileRadius;

            var x = (int)Mathf.Round(q);
            var y = (int)Mathf.Round(r);


            var worldX = ((3f / 2f) * x) * TileRadius;
            var worldY = (float)(Mathf.Sqrt(3f) / 2f * x + Mathf.Sqrt(3f) * y) * TileRadius;

            Debug.DrawRay(new Vector3(worldX, 0, worldY), Vector3.down, Color.green, 10f);

            return new Vector2(x, y);
        }

        public Vector3 ToWorldPosition(int x, int y)
        {

            var q = TileRadius * ((3f / 2f) * x);
            var r = TileRadius * (Mathf.Sqrt(3f) / 2f * x + Mathf.Sqrt(3f) * y);

            var tileposition = new Vector3(q, 0, r);



            return tileposition;
        }

    }

}

