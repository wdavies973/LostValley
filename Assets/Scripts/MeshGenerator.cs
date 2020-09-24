using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
	public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve) {
		int width = heightMap.GetLength(0);
		int height = heightMap.GetLength(1);
		float topLeftX = (width - 1) / -2f;
		float topLeftZ = (height - 1) / 2f;

		MeshData meshData = new MeshData(width, height);
		int vertexIndex = 0;

		for(int y = 0; y < height; y++) {
			for (int x = 0; x < height; x++) {
				meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
				meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

				if(x < width - 1 && y < height - 1) {
					meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
					meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
				}

				vertexIndex++;
			}
		}

		return meshData;
	}

	public class MeshData {
		public Vector3[] vertices;
		public Vector3[] normals;
		public int[] triangles;
		public Vector2[] uvs;

		int triangleIndex;

		public MeshData(int meshWidth, int meshHeight) {
			vertices = new Vector3[meshWidth * meshHeight];
			uvs = new Vector2[meshWidth * meshHeight];
			triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
			normals = new Vector3[meshWidth * meshHeight];
		}

		public void AddTriangle(int a, int b, int c) {
			
			Vector3 one = vertices[a];
			Vector3 two = vertices[b];
			Vector3 three = vertices[c];

			Vector3 normal = Vector3.Cross(two - three, one - three).normalized;
			normals[a] = normal.normalized;
			normals[b] = normal.normalized;
			normals[c] = normal.normalized;

			triangles[triangleIndex] = a;
			triangles[triangleIndex+1] = b;
			triangles[triangleIndex+2] = c;
			triangleIndex += 3;
		}

		Vector3[] CalculateNormals() {
			Vector3[] vertexNormals = new Vector3[vertices.Length];
			int triangleCount = triangles.Length / 3;
			for(int i = 0; i < triangleCount; i++) {
				int normalTriangleIndex = i * 3;
				int vertexIndexA = triangles[normalTriangleIndex];
				int vertexIndexB = triangles[normalTriangleIndex+1];
				int vertexIndexC = triangles[normalTriangleIndex+2];

				Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);

				vertexNormals[vertexIndexA] += triangleNormal;
				vertexNormals[vertexIndexB] += triangleNormal;
				vertexNormals[vertexIndexC] += triangleNormal;
			}

			for(int i = 0; i < vertexNormals.Length; i++) {
				vertexNormals[i].Normalize();
			}

			return vertexNormals;
		}
		Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC) {
			Vector3 pointA = vertices[indexA];
			Vector3 pointB = vertices[indexB];
			Vector3 pointC = vertices[indexC];

			Vector3 sideAB = pointB - pointA;
			Vector3 sideAC = pointC - pointA;

			return Vector3.Cross(sideAB, sideAC).normalized;
		}

		public Mesh CreateMesh() {
			Mesh mesh = new Mesh {
				vertices = vertices,
				triangles = triangles,
				normals = CalculateNormals(),
				uv = uvs
			};
			//mesh.RecalculateNormals();
			return mesh;
		}
	}

}
