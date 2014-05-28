using UnityEngine;
using System.Collections;

public abstract class Attack : MonoBehaviour
{
	public GameObject owner;

	// Use this for initialization
    protected virtual void Start()
	{
	}
	
	// Update is called once per frame
	protected virtual void Update()
    {
    }

	public abstract void Activate();

	public abstract void Deactivate();

	public abstract void OnTerrain(GameObject terrain);

    public abstract void OnPlayer(GameObject player);
}
