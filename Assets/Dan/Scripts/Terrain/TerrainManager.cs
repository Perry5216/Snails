using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{

    public static TerrainManager tm;

    [Header("Chunk Prefab")]
    public GameObject chunkPrefab;
    [Space]
    [Header("Chunk Settings")]
    public int chunkSizeX = 10;
    public int chunkSizeY = 10;
    [Space]
    public int chunkCountX = 2;
    public int chunkCountY = 2;
    [Space]
    [Header("Map Settings")]
    public bool destructible = true;
    [Space]
    public string tagWhichCanDestroy = "Destroyer";
    [Space]
    public bool useRandomSeed = true;
    [Space]
    public string seed;
    [Space]
    [Range(0, 100)]
    public int randomFillPercentage = 45;
    [Space]
    [Range(1, 10)]
    public int smoothIterations = 5;

    [HideInInspector]
    public int[,] map;

    int width;
    int height;

    [Space]
    [HideInInspector]
    public Vector3 startPosition = new Vector3(0f, 0f, 0f);
    [Space]
    public bool gizmos = true;

    private List<TerrainChunk> chunks;

    private void Awake()
    {
        tm = this;
        chunks = new List<TerrainChunk>();
        startPosition = new Vector3(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), Mathf.FloorToInt(transform.position.z));
        width = chunkCountX * chunkSizeX;
        height = chunkCountY * chunkSizeY;
        map = new int[width, height];
        GenerateSeedMap();
        for (int i = 0; i < smoothIterations; i++)
        {
            Smooth();
        }
        if (chunkPrefab != null)
            InstantiateChunks();
    }

    private void GenerateSeedMap()
    {
        if (useRandomSeed)
            seed = DateTime.Now.Millisecond.ToString();

        System.Random pseudo = new System.Random(seed.GetHashCode());
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int ans = pseudo.Next(0, 100);
                if (ans < randomFillPercentage)
                    map[x, y] = 1;
                else
                    map[x, y] = 0;
            }
        }

    }

    private void Smooth()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int adjacentWalls = GetAdjacentTileCount(x, y);
                if (adjacentWalls > 4)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    private void InstantiateChunks()
    {
        for (int x = 0; x < chunkCountX; x++)
        {
            for (int y = 0; y < chunkCountY; y++)
            {
                Vector3 pos = startPosition;
                pos.x = pos.x + (x * chunkSizeX);
                pos.z = pos.z + (y * chunkSizeY);
                GameObject o = Instantiate(chunkPrefab, pos, Quaternion.identity);
                chunks.Add(o.GetComponent<TerrainChunk>());
                chunks[chunks.Count - 1].Construct(x, y); //o.GetComponent<TerrainChunk>()
                o.transform.parent = gameObject.transform;
            }
        }
    }

    int GetAdjacentTileCount(int posx, int posy)
    {
        int adjWallCount = 0;
        for (int x = posx - 1; x <= posx + 1; x++)
        {
            for (int y = posy - 1; y <= posy + 1; y++)
            {
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    if (x != posx || y != posy)
                    {
                        adjWallCount += map[x, y];
                    }
                }
                else
                {
                    adjWallCount++;
                }
            }
        }
        return adjWallCount;
    }


    private void OnDrawGizmos()
    {
        if (map != null && gizmos)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (map[x, y] == 1)
                        Gizmos.color = Color.yellow;
                    else
                        Gizmos.color = Color.black;
                    Vector3 pos = new Vector3(x, 0f, y);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }

    public void RefreshChunks()
    {
        foreach (TerrainChunk terrainChunk in chunks)
        {
            terrainChunk.GenerateMesh();
        }
    }
}
