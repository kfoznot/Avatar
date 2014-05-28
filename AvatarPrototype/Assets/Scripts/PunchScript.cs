using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PunchScript : MonoBehaviour {

    public int damage = 10;
    public float magnitude = 40.0f;
    public float time = 0.5f;
    public float range = 3.0f;

    private float timer = 0.0f;
    public Vector2 direction = new Vector2(1.0f, 0.0f);

    HashSet<GameObject> players = new HashSet<GameObject>();
    GameObject owner;

	// Use this for initialization
	void Start()
    {
        timer = time;
	}

    public void Use(GameObject owner, Vector2 dir)
    {
        this.owner = owner;
        players.Add(owner);
        direction = dir;
        transform.position = owner.transform.position;
    }
	
	// Update is called once per frame
	void Update()
    {
        timer -= Time.deltaTime;
        
        if (timer < 0)
        {
            Destroy(gameObject);
        }

        direction.Normalize();

        if (timer < time / 2.0f)
        {
            direction *= range * timer * 2.0f / time;
        }
        else
        {
            direction *= range * ((time - (2.0f * timer - time)) / time);
        }

        //Debug.Log(direction.ToString());
        transform.position = owner.transform.position + new Vector3(direction.x, direction.y, 0.0f);
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject obj = collider.gameObject;
        if (obj == null)
        {
            return;
        }

        PlayerController player = obj.GetComponent<PlayerController>();
        if (player == null)
        {
            return;
        }

        if (players.Contains(obj))
        {
            return;
        }
        players.Add(obj);

        Vector2 direction = collider.gameObject.transform.position - transform.position;
        direction.Normalize();
        direction.y += 1.0f;
        direction *= magnitude;
        collider.gameObject.rigidbody2D.velocity = direction;
        player.Damage(damage);
    }
}
