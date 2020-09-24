using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyColorScript : MonoBehaviour {

	public GameObject plane;
	private MeshRenderer myRenderer;

	public void OnClickChangeColor() {
		myRenderer = plane.GetComponent<MeshRenderer>();
		myRenderer.enabled = !myRenderer.enabled;
	}

	// Start is called before the first frame update
	void Start() {
		Debug.Log("Starting script...");
	}

	// Update is called once per frame
	void Update() {

	}
}
