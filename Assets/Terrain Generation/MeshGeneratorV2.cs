using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class MeshGeneratorV2 : MonoBehaviour
{
    Mesh mesh;
    private int MESH_SCALE = 100;
    public GameObject[] objects;
    [SerializeField] private AnimationCurve heightCurve;
    private Vector3[] vertices;
    private int[] triangles;

    private Color[] colors = {
        new(122f/255f, 111f/255f, 116f/255f),
        new(133f/255f, 68f/255f, 55f/255f),
        new(189f/255f, 128f/255f, 98f/255f),
        new(129f/255f, 104f/255f, 78f/255f),
        new(172f/255f, 146f/255f, 119f/255f),
        new(197f/255f, 188f/255f, 153f/255f),
        new(128f/255f, 142f/255f, 158f/255f),
        new(117f/255f, 116f/255f, 146f/255f),
        new(192f/255f, 129f/255f, 192f/255f),
        new(87f/255f, 95f/255f, 89f/255f)
    };
    private Color[] randColor;
    //[SerializeField] private Gradient gradient;
    
    private float minTerrainheight;
    private float maxTerrainheight;

    public int xSize;
    public int zSize;

    public float scale; 
    public int octaves;
    public float lacunarity;

    public int seed;

    private float lastNoiseHeight;

    void Start()
    {
        // Use this method if you havn't filled out the properties in the inspector
        // SetNullProperties(); 

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateNewMap();
    }

    private void OnValidate()
    {
        Start();
    }

    private void SetNullProperties() 
    {
        if (xSize <= 0) xSize = 50;
        if (zSize <= 0) zSize = 50;
        if (octaves <= 0) octaves = 5;
        if (lacunarity <= 0) lacunarity = 2;
        if (scale <= 0) scale = 50;
    } 

    public void CreateNewMap()
    {
        CreateMeshShape();
        CreateTriangles();
        ColorMap();
        UpdateMesh();
    }

    private void CreateMeshShape ()
    {
        // Creates seed
        Vector2[] octaveOffsets = GetOffsetSeed();

        if (scale <= 0) scale = 0.0001f;
            
        // Create vertices
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                // Set height of vertices
                float noiseHeight = GenerateNoiseHeight(z, x, octaveOffsets);
                SetMinMaxHeights(noiseHeight);
                vertices[i] = new Vector3(x, noiseHeight, z);
                i++;
            }
        }
    }

    private Vector2[] GetOffsetSeed()
    {
        seed = Random.Range(0, 1000);
        
        // changes area of map
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
                    
        for (int o = 0; o < octaves; o++) {
            float offsetX = prng.Next(-100000, 100000);
            float offsetY = prng.Next(-100000, 100000);
            octaveOffsets[o] = new Vector2(offsetX, offsetY);
        }
        return octaveOffsets;
    }

    private float GenerateNoiseHeight(int z, int x, Vector2[] octaveOffsets)
    {
        float amplitude = 20;
        float frequency = 1;
        float persistence = 0.5f;
        float noiseHeight = 0;

        // loop over octaves
        for (int y = 0; y < octaves; y++)
        {
            float mapZ = z / scale * frequency + octaveOffsets[y].y;
            float mapX = x / scale * frequency + octaveOffsets[y].x;

            //The *2-1 is to create a flat floor level
            float perlinValue = (Mathf.PerlinNoise(mapZ, mapX)) * 2 - 1;
            noiseHeight += heightCurve.Evaluate(perlinValue) * amplitude;
            frequency *= lacunarity;
            amplitude *= persistence;
        }
        return noiseHeight;
    }

    private void SetMinMaxHeights(float noiseHeight)
    {
        // Set min and max height of map for color gradient
        if (noiseHeight > maxTerrainheight)
            maxTerrainheight = noiseHeight;
        if (noiseHeight < minTerrainheight)
            minTerrainheight = noiseHeight;
    }


    private void CreateTriangles() 
    {
        // Need 6 vertices to create a square (2 triangles)
        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;
        // Go to next row
        for (int z = 0; z < zSize; z++)
        {
            // fill row
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }
    
    private void ColorMap()
    {
        //colors = new Color[vertices.Length];

        randColor = new Color[vertices.Length];

        // Loop over vertices and apply a color from the depending on height (y axis value)
        for (int z = 0; z < vertices.Length; z++)
        {
            randColor[z] = colors[seed % colors.Length];
        }
    }
    
    /*
    private void MapEmbellishments() 
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            // find actual position of vertices in the game
            Vector3 worldPt = transform.TransformPoint(mesh.vertices[i]);
            var noiseHeight = worldPt.y;
            // Stop generation if height difference between 2 vertices is too steep
            if(System.Math.Abs(lastNoiseHeight - worldPt.y) < 25)
            {
                // min height for object generation
                if (noiseHeight > 100)
                {
                    // Chance to generate
                    if (Random.Range(1, 5) == 1)
                    {
                        GameObject objectToSpawn = objects[Random.Range(0, objects.Length)];
                        var spawnAboveTerrainBy = noiseHeight * 2;
                        Instantiate(objectToSpawn, new Vector3(mesh.vertices[i].x * MESH_SCALE, spawnAboveTerrainBy, mesh.vertices[i].z * MESH_SCALE), Quaternion.identity);
                    }
                }
            }
            lastNoiseHeight = noiseHeight;
        }
    }
    */

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.colors = randColor;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        GetComponent<MeshCollider>().sharedMesh = mesh;
        gameObject.transform.localScale = new Vector3(MESH_SCALE, MESH_SCALE, MESH_SCALE);

        //MapEmbellishments();
    }

}
