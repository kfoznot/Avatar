using UnityEngine;
using System.Collections;

public class HitBox : MonoBehaviour
{

	public GameObject owner;
	public Attack attack;
	public bool activated;

	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
	}

    public void Activate(GameObject owner, Attack attack)
    {
        this.owner = owner;
        this.attack = attack;
        activated = true;
    }

    public void Deactivate()
    {
        activated = false;
    }
	
	void OnTriggerEnter2D(Collider2D c)
	{
		if (!activated || c == null || c.gameObject == owner)
		{
			return;
		}

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
