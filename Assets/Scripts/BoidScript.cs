using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class BoidScript : MonoBehaviour
{
    public float speed;
    public float dx, dy;

    public Vector3 velocity;
    public Vector3 acceleration;

    private static System.Random random = new System.Random();

    public GameObject[] boids;
    public BoidScript[] scripts;

    private Vector3[] mountains = {
        new Vector3(847f,127.3f,688f),
        new Vector3(865f,127.3f,519.3f),
        new Vector3(865f, 127.3f,48.1f),
        new Vector3(359f,127.3f,224.3f),
        new Vector3(359f,127.3f,556f),
        new Vector3(537f,127.3f,881f),
        new Vector3(550f,116f,827.3f)
    };

    private Vector3 wander;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0, 0, 0);
        wander = mountains[random.Next(0, mountains.Length - 1)];

        scripts = new BoidScript[boids.Length];
        for(int i = 0; i < boids.Length; i++) {
            scripts[i] = boids[i].GetComponent<BoidScript>();
		}
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * Perform boid update code
         */
        acceleration = new Vector3(0, 0, 0);

		// Separation force
		for (int i = 0; i < boids.Length; i++) {
			float dist = Vector3.Distance(transform.position, boids[i].transform.position);
			if (dist < .01 || dist > 150) continue;

			Vector3 separationForce = (transform.position - boids[i].transform.position).normalized;
			separationForce *= (50f / (float)Mathf.Pow(dist, 2));
			acceleration += separationForce;
		}

		// Attraction force
		Vector3 avgPos = new Vector3();
		int count = 0;
		for (int i = 0; i < boids.Length; i++) {
			float dist = Vector3.Distance(transform.position, boids[i].transform.position);
			if (dist < 100 && dist > 0) {
				avgPos += boids[i].transform.position;
				count++;
			}
		}
		avgPos *= (1.0f / count);
		if (count >= 1) {
			Vector3 attractionForce = avgPos - transform.position;
			attractionForce.Normalize();
			attractionForce *= 5;
			Vector3.ClampMagnitude(attractionForce, 50);
			acceleration += attractionForce;
		}

		// Alignment force
		Vector3 avgVel = new Vector3(0, 0, 0);
		count = 0;
		for (int i = 0; i < boids.Length; i++) {
			float dist = Vector3.Distance(transform.position, boids[i].transform.position);
			if (dist < 100 && dist > 0) {
				BoidScript script = scripts[i];
				avgVel += script.velocity;
				count++;
			}
		}
		avgVel *= (1.0f / count);
		if (count >= 1) {
			Vector3 towards = avgVel - velocity;
			towards.Normalize();
			acceleration += towards * 0.01f;
		}

		// Wander force
		// If close to wander, find a new one
		if (Vector3.Distance(transform.position, wander) < 50f) {
			wander = mountains[random.Next(0, mountains.Length - 1)];
		}

		Vector3 wanderNormal = (wander - transform.position).normalized;
		acceleration += wanderNormal * 2.2f;

		velocity += acceleration * Time.deltaTime;
        transform.position = transform.position + velocity * Time.deltaTime;

		// Rotate boid object
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * 10f);

        if(velocity.magnitude > 30) {
            velocity.Normalize();
            velocity *= 30f;
		}
    }
}
