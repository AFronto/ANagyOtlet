using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PerlinNoise : MonoBehaviour {

    public Tile ground;
    public Tile water;
    public Tilemap map;

    public int width = 1000;
    public int hight = 1000;

    public float scale = 4f;
    public float offsetX = 100f;
    public float offsetY = 100f;

    [Range(0f,1f)]
    public float ratio = 0.6f;

    private void Start()
    {
        offsetX = Random.Range(0f, 1000001f);
        offsetY = Random.Range(0f, 1000001f);
    }

    // Use this for initialization
    void Update () {
        for (int x=0;x<width;x++)
        {
            for (int y=0;y<hight;y++)
            {
                map.SetTile(new Vector3Int(-x + width / 2, -y + hight / 2,0),CalculateTile(x,y));
            }
        }
	}

    public Tile CalculateTile(int x,int y)
    {
        float xCord = (float)x / width *scale + offsetX;
        float yCord = (float)y / hight *scale + offsetY;
        Tile back;
        float whichTile = Mathf.PerlinNoise(xCord, yCord);
        if (whichTile < ratio)
        {
            back = ground;
        }
        else
        {
            back = water;
        }
        return back;
    }

}
