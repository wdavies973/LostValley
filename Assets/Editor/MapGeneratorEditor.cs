﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{

    public override void OnInspectorGUI() {
		MapGenerator mapGen = target as MapGenerator;

		if(DrawDefaultInspector()) {
			if(mapGen.autoUpdate) {
				mapGen.GenerateMap();
			}
		}

		if(GUILayout.Button("Generate")) {
			mapGen.GenerateMap();
		}

		if(GUILayout.Button("Spawn Boids")) {
			mapGen.SpawnBoids();
		}
	}
}
