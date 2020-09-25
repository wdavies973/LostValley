using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode {
        NoiseMap,
        ColorMap,
        Mesh
	}
    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistence;
    public float lacunarity;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public TerrainType[] regions;

    public GameObject boidPrefab;
    private GameObject[] boids;

    public int numBoids;

    public void GenerateMap() {
        float[,] noiseMap = Noise.GenerateHeightMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistence, lacunarity, offset);

        Color[] colors = new Color[mapWidth * mapHeight];

        for(int y = 0; y < mapHeight; y++) {
            for(int x = 0; x < mapWidth; x++) {
                float currentHeight = noiseMap[x, y];
                for(int i = 0; i < regions.Length; i++) {
                    if(currentHeight <= regions[i].height) {
                        colors[y * mapWidth + x] = regions[i].color;
                        break;
					}
				}
			}
		}

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if(drawMode == DrawMode.NoiseMap) {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        } else if(drawMode == DrawMode.ColorMap) {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colors, mapWidth, mapHeight));
        } else if(drawMode == DrawMode.Mesh) {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColorMap(colors, mapWidth, mapHeight));
		}
        
	}

    public void SpawnBoids() {
        System.Random random = new System.Random();

        for (int i = 0; i < numBoids; i++) {
            GameObject boid = Instantiate(boidPrefab, new Vector3(600 - random.Next(-50, 50), 70 - random.Next(-10, 10), 600 - random.Next(-50, 50)), Quaternion.identity);

            BoidScript script = boid.GetComponent<BoidScript>();
            script.boids = boids;
            boids[i] = boid;
		}
	}

	private void OnValidate() {
	    if(mapWidth <= 1) {
            mapWidth = 1;
		} 
        if(mapHeight <= 1) {
            mapHeight = 1;
		} 
        if(lacunarity < 1) {
            lacunarity = 1;
		} 
        if(octaves < 0) {
            octaves = 0;
		}
	}

	// Start is called before the first frame update
	void Start()
    {
        boids = new GameObject[numBoids];

        GenerateMap();
        SpawnBoids();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color color;
}
