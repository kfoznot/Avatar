using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitBox : MonoBehaviour
{

	public GameObject owner;
	private Attack attack;
	private bool activated;
	private HashSet<GameObject> players = new HashSet<GameObject>();

	// Use this for initialization
	void Start()
	{
		players.Add(owner);
	}
	
	// Update is called once per frame
	void Update()
	{
	}

    public void Activate(Attack attack)
    {
        this.attack = attack;
		activated = true;
    }

    public void Deactivate()
    {
        activated = false;
    }
	
	void OnTriggerEnter2D(Collider2D c)
	{
		if (!activated || c == null || players.Contains(c.gameObject))
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
