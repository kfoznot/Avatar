using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(HitBox))]
public class BasicAttack : Attack
{
    HashSet<GameObject> players = new HashSet<GameObject>();
    private float timer = 0.0f;
    public float time = 0.5f;
    public int damage = 10;
    public float magnitude = 10.0f;
    public float range = 3.0f;
    public Vector2 direction = new Vector2(1.0f, 0.0f);

    // Use this for initialization
    protected override void Start()
    {
    }

    // Update is called once per frame
    protected override void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            gameObject.GetComponent<HitBox>().Deactivate();
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

    public override void Activate()
    {
        gameObject.GetComponent<HitBox>().Activate(owner, this);
        players.Add(owner);
        timer = time;
        if (owner.GetComponent<PlayerController>().dirx > 0.0f)
        {
            direction = new Vector2(1.0f, 0.0f);
        }
        else
        {
            direction = new Vector2(-1.0f, 0.0f);
        }
    }

    public override void Deactivate()
    {
    }

    public override void OnTerrain(GameObject terrain)
    {
    }

    public override void OnPlayer(GameObject player)
    {
        if (players.Contains(player))
        {
            return;
        }

        players.Add(player);
        player.GetComponent<PlayerController>().Damage(damage);
        player.rigidbody2D.velocity = new Vector2(-magnitude, 0.0f);
    }
}
