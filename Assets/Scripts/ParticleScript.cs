using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    private Renderer renderer;

    private Vector3 acceleration;
    private Vector3 velocity;

    private System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();

        acceleration = new Vector3(0, (float)random.NextDouble() * 3f, 0);
        velocity = new Vector3((float)random.NextDouble() * 0.05f, 1.5f + (float)random.NextDouble() * 2f, (float)random.NextDouble() * 0.05f);

        renderer.material.color = new Color(255f, 0f, 0f);
        float r = +(float)random.NextDouble() * 0.012f - 0.006f;
        transform.localScale = new Vector3(0.03f + r, 0.03f + r, 0.03f + r);
    }

    // Update is called once per frame
    void Update()
    {

        float life = (transform.position.y / -0.5f);

        life = Mathf.Clamp(life, 0f, 1f);

        renderer.material.color = new Color((1 - 0.7f) * life + 0.7f, 0.6f * life, 0, 1f - life);

        velocity += acceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;

        if (transform.position.y >= -0.5f && random.NextDouble() <= 0.25f) {
            Destroy(this.gameObject);
		}
    }
}
