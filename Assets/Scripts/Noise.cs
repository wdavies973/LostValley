using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise {
	public static float[,] GenerateHeightMap(int width, int height, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset) {
		if (scale <= 0) {
			scale = 0.0001f;
		}

		System.Random rand = new System.Random(seed);

		Vector2[] octaveOffsets = new Vector2[octaves];
		for(int i = 0; i < octaves; i++) {
			float offsetX = rand.Next(-100000, 100000) + offset.x;
			float offsetY = rand.Next(-100000, 100000) + offset.y;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);
		}

		float[,] heights = new float[width, height];

		float maxHeight = float.MinValue;
		float minHeight = float.MaxValue;

		float halfWidth = width / 2f;
		float halfHeight = height / 2f;

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {

				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++) {
					float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
					float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

					float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistence;
					frequency *= lacunarity;
				}

				if (noiseHeight > maxHeight) {
					maxHeight = noiseHeight;
				} else if (noiseHeight < minHeight) {
					minHeight = noiseHeight;
				}

				heights[x, y] = noiseHeight;
			}
		}

		// Normalizes map
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				heights[x, y] = Mathf.InverseLerp(minHeight, maxHeight, heights[x, y]);
			}
		}

		return heights;
	}
}
