using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator {
	public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve) {
		int width = heightMap.GetLength(0);
		int height = heightMap.GetLength(1);
		float topLeftX = 0; //(width - 1) / -2f;
		float topLeftZ = 0;// (height - 1) / 2f;

		MeshData meshData = new MeshData(width, height);

		Debug.Log(width + "," + height);

		for (int y = 0; y < height - 1; y++) {
			for (int x = 0; x < width - 1; x++) {
				Vector3 p1 = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, y - topLeftZ);
				Vector3 p2 = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y + 1]) * heightMultiplier, (y + 1) - topLeftZ);
				Vector3 p3 = new Vector3(topLeftX + x + 1, heightCurve.Evaluate(heightMap[x + 1, y + 1]) * heightMultiplier, (y + 1) - topLeftZ);
				Vector2 uv1 = new Vector2(x / (float)width, y / (float)height);
				Vector2 uv2 = new Vector2(x / (float)width, (y + 1) / (float)height);
				Vector2 uv3 = new Vector2((x + 1) / (float)width, (y + 1) / (float)height);
				meshData.AddTriangle(p1, p2, p3, uv1, uv2, uv3);

				p1 = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, y - topLeftZ);
				p2 = new Vector3(topLeftX + x + 1, heightCurve.Evaluate(heightMap[x + 1, y + 1]) * heightMultiplier, (y + 1) - topLeftZ);
				p3 = new Vector3(topLeftX + x + 1, heightCurve.Evaluate(heightMap[x + 1, y]) * heightMultiplier, y - topLeftZ);
				uv1 = new Vector2(x / (float)width, y / (float)height);
				uv2 = new Vector2((x + 1) / (float)width, (y + 1) / (float)height);
				uv3 = new Vector2((x + 1) / (float)width, y / (float)height);
				meshData.AddTriangle(p1, p2, p3, uv1, uv2, uv3);
			}
		}

		return meshData;
	}

	public class MeshData {
		public Vector3[] vertices;
		public Vector3[] normals;
		public int[] triangleVerticles;
		public Vector2[] uvs;

		int triangleIndex;

		public MeshData(int meshWidth, int meshHeight) {
			triangleVerticles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
			vertices = new Vector3[triangleVerticles.Length];
			uvs = new Vector2[triangleVerticles.Length];
			normals = new Vector3[triangleVerticles.Length];
		}

		public void AddTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 uv1, Vector2 uv2, Vector2 uv3) {
			vertices[triangleIndex] = p1;
			vertices[triangleIndex + 1] = p2;
			vertices[triangleIndex + 2] = p3;

			uvs[triangleIndex] = uv1;
			uvs[triangleIndex + 1] = uv2;
			uvs[triangleIndex + 2] = uv3;

			Vector3 normal = Vector3.Cross(p2 - p1, p3 - p1).normalized;
			normals[triangleIndex] = normal;
			normals[triangleIndex + 1] = normal;
			normals[triangleIndex + 2] = normal;

			triangleVerticles[triangleIndex] = triangleIndex;
			triangleVerticles[triangleIndex + 1] = triangleIndex + 1;
			triangleVerticles[triangleIndex + 2] = triangleIndex + 2;
			triangleIndex += 3;
		}

		public Mesh CreateMesh() {
			Mesh mesh = new Mesh {
				vertices = vertices,
				triangles = triangleVerticles,
				normals = normals,
				uv = uvs
			};

			//mesh.RecalculateNormals();
			return mesh;
		}
	}

}
