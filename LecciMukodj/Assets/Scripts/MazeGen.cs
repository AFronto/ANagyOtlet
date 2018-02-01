using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGen : MonoBehaviour {

    public Tile dungeonFloor;
    public Tile dungeonWall;
    public Tilemap map;

    public int width = 101;
    public int height = 101;

    public int corridorLength = 8;

    [Range(0f,1f)]
    public float roomChance;

    [Range(0f, 1f)]
    public float roomAdditionChance;

    public int tilesizeX=1;
    public int tilesizeY = 1;

    private int[,] maze;

    // Use start in real game its update for testing
    void Start () {
        generateMazeBase();
        mapValidator();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 0)
                {
                    //map.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), dungeonFloor);
                }
                else
                {
                    for(int innerx = 0; innerx < tilesizeX; innerx++)
                    {
                        for (int innery = 0; innery < tilesizeY; innery++)
                        {
                            map.SetTile(new Vector3Int(-x*tilesizeX+innerx + width*tilesizeX / 2, -y*tilesizeY+innery + height*tilesizeY / 2, 0), dungeonWall);
                        }
                    }
                }
            }
        }

    }

    public int[,] generateMazeBase()
    {
        maze = new int[height, width];
        // Initialize
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                maze[i, j] = 1;
            }
        }

        int r = Random.Range(0, height);
        while(r%2 == 0)
        {
            r = Random.Range(0, height);
        }

        int c = Random.Range(0, width);
        while (c % 2 == 0)
        {
            c = Random.Range(0, width);
        }

        maze[r,c] = 0;
        
        recursion(r, c);

        return maze;
    }

    public void recursion(int r, int c)
    {
        // 4 random directions
        List<int> randDirs = generateRandomDirections();
        // Examine each direction
        for (int i = 0; i < randDirs.Count; i++)
        {
            switch (randDirs[i])
            {
                case 0: // Up
                        //　Whether 2 cells up is out or not
                    if (r-corridorLength <= 0)
                        continue;
                    if (maze[r -corridorLength,c] != 0)
                    {
                       
                        for (int act = 1; act < corridorLength + 1; act++)
                        {
                            maze[r - act, c] = 0;
                            if (act == corridorLength / 2)
                            {
                                GenRoom(r - act, c);
                            }
                        }
                        recursion(r - corridorLength, c);
                    }
                    break;
                case 1: // Right
                        // Whether 2 cells to the right is out or not
                    if (c +corridorLength >= width - 1)
                        continue;
                    if (maze[r,c+corridorLength] != 0)
                    {
                        for (int act = 1; act < corridorLength + 1; act++)
                        {
                            maze[r, c + act] = 0;
                            if (act == corridorLength / 2)
                            {
                                GenRoom(r, c + act);
                            }
                        }
                        recursion(r, c + corridorLength);
                    }
                    break;
                case 2: // Down
                        // Whether 2 cells down is out or not
                    if (r + corridorLength >= height - 1)
                        continue;
                    if (maze[r + corridorLength,c] != 0)
                    {
                        for (int act = 1; act < corridorLength + 1; act++)
                        {
                            maze[r + act, c] = 0;
                            if (act == corridorLength / 2)
                            {
                                GenRoom(r + act, c);
                            }
                        }

                        recursion(r + corridorLength, c);
                    }
                    break;
                case 3: // Left
                        // Whether 2 cells to the left is out or not
                    if (c - corridorLength <= 0)
                        continue;
                    if (maze[r,c - corridorLength] != 0)
                    {   
                        for(int act =1; act< corridorLength+1; act++)
                        {
                            maze[r, c - act] = 0;
                            if (act == corridorLength / 2)
                            {
                                GenRoom(r, c - act);
                            }
                        }
                        recursion(r, c - corridorLength);
                    }
                    break;
            }
        }
    }

    public List<int> generateRandomDirections()
    {
        List<int> MyArray = new List<int>();
        for (int i =0; i < 4; i++)
        {
            int value = Random.Range(0, 4);
            while (MyArray.Contains(value))
            {
                value = Random.Range(0, 4);
            }
            MyArray.Add(value);
        }

        return MyArray;
    }

    public void GenRoom(int x,int y)
    {
        float chanche = Random.Range(0f, 1f);
        if (x - corridorLength / 2 + 1 > 1 && y - corridorLength / 2 + 1 > 1 && x + corridorLength / 2 - 1<width-1 && y + corridorLength / 2 - 1 < height-1 && chanche<roomChance)
        {
            for (int i = -corridorLength / 2 + 1; i < corridorLength / 2; i++)
            {
                for (int j = -corridorLength / 2 + 1; j < corridorLength / 2; j++)
                {
                    maze[x + i, y + j] = 0;
                }
            }

            for (int i = -corridorLength / 2 + 1; i < corridorLength / 2; i++)
            {
                for (int j = -corridorLength / 2 + 1; j < corridorLength / 2; j++)
                {
                    int neighbours = 0;
                    for(int n = -1; n < 2; n++)
                    {
                       if(maze[x + i+n, y + j] == 1)
                       {
                            neighbours++;
                       }
                    }
                    for (int n = -1; n < 2; n++)
                    {
                        if (maze[x + i, y + j +n] == 1)
                        {
                            neighbours++;
                        }
                    }

                    if (neighbours*roomAdditionChance>Random.Range(0f,1f) && neighbours!=0)
                    {
                        maze[x + i, y + j] = 1;
                    }

                    if (j==0 || i==0)
                    {
                        maze[x + i, y + j] = 0;
                    }
                }
            }
        }
    }

    public void mapValidator()
    {
        for (int x = 1; x < width-1; x++)
        {
            for (int y = 1; y < height-1; y++)
            {
                int differentNeighbours = 0;
                for (int n = -1; n < 2; n++)
                {
                    if (maze[x + n, y ] != maze[x , y])
                    {
                        differentNeighbours++;
                    }
                }
                for (int n = -1; n < 2; n++)
                {
                    if (maze[x , y + n] != maze[x, y])
                    {
                        differentNeighbours++;
                    }
                }

                if (differentNeighbours==4)
                {
                    maze[x, y] = (maze[x, y] + 1) % 2;
                }
            }
        }
    }
}
