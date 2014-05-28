using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicAttack : Attack
{
    HashSet<GameObject> players = new HashSet<GameObject>();
    private float timer = 0.0f;
    public float time = 0.5f;
    public int damage = 10;
    public float magnitude = 10.0f;

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
    }

    public override void Activate()
    {
        gameObject.GetComponent<HitBox>().Activate(owner, this);
        players.Add(owner);
        timer = time;
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
