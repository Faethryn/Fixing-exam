using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HexGen : MonoBehaviour
{
    [SerializeField]
    private GameObject _hexPrefab;
    [SerializeField]
    private int _layers;


    void Start()
    {
      
        GenerateBoard();


    }
    private void GenerateBoard()
    {
        for (int q = -_layers; q <= _layers; q++)
        {
            int rNegative = Mathf.Max(-_layers, -q - _layers);
            int rPositive = Mathf.Min(_layers, -q + _layers);
            for (int r = rNegative; r <= rPositive; r++)
            {
                Vector2 positionV2 = HexLocationCalc(q, r);
                HexInstantiation(positionV2, q, r);
            }
        }
    }




   

    private Vector2 HexLocationCalc(int q, int r)
    {
        //var x = (3f / 2f * q);
        //var y = ((Mathf.Sqrt(3) / 2f * q) + (Mathf.Sqrt(3) * r));

        var y = ((3f / 2f) * r) ;
        var x = (float)( Mathf.Sqrt(3f) / 2f * r  + Mathf.Sqrt(3f) * q);

        return new Vector2(x, y);
    }

    private void HexInstantiation(Vector2 position, int q, int r)
    {
        Vector3 spawnPosition = new Vector3(position.x, 0, position.y);
        int s = -q  - r ;

        GameObject hex = Instantiate(_hexPrefab, spawnPosition, _hexPrefab.transform.rotation);
        hex.GetComponent<Tile>().Q = q;
        hex.GetComponent<Tile>().R = r;
        hex.GetComponent<Tile>().S = s;
        hex.name = $"Hex {q}, {r}, {s}";
    }
}
