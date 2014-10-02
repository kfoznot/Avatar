using UnityEngine;
using System.Collections;

public class BasicPunch : Attack
{
    public GameObject owner;
    public float time = 0.5f;
    public int damage = 10;
    private float timer = 0.0f;
    public Vector2 direction = new Vector2(1.0f, 0.0f);
    private HitBox hitbox;
    private SpriteRenderer sprite;
    public string button;

    protected override void Start()
    {
        hitbox = GetComponent<HitBox>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public override void Activate()
    {
        if (Active())
        {
            return;
        }

        timer = time;
        hitbox.Activate(this);
        sprite.enabled = true;
    }

    protected override void Update()
    {
        button = "Punch" + owner.GetComponent<PlayerController>().playerNumber;
        if (Input.GetButtonDown(button))
        {
            Activate();
        }

        if (!Active())
        {
            return;
        }
        timer -= Time.deltaTime;
        if (!Active())
        {
            Kill();
        }
    }

    protected bool Active()
    {
        return timer > 0.0f;
    }

    protected void Kill()
    {
        hitbox.Deactivate();
        sprite.enabled = false;
    }

    public override void OnPlayer(GameObject player)
    {
        player.GetComponent<PlayerController>().Damage(damage);
    }
}
