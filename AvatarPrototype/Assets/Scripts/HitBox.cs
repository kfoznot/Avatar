using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitBox : MonoBehaviour
{

	public GameObject owner;
	private Attack attack;
	private bool activated;
    private HashSet<GameObject> players;
    private BoxCollider2D collider;

	// Use this for initialization
	void Start()
	{
        collider = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update()
	{
	}

    public void Activate(Attack attack)
    {
        players = new HashSet<GameObject>();
        players.Add(owner);
        this.attack = attack;
		activated = true;
        collider.enabled = true;
    }

    public void Deactivate()
    {
        collider.enabled = true;
        activated = false;
        players = null;
    }
	
	void OnTriggerEnter2D(Collider2D c)
	{
		if (!activated || c == null || players == null || players.Contains(c.gameObject))
		{
			return;
		}
		players.Add(c.gameObject);

        PlayerController p = c.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            attack.OnPlayer(c.gameObject);
        }
		else if (c.tag == "SolidTerrain")
		{
			attack.OnTerrain(c.gameObject);
		}
	}
}
