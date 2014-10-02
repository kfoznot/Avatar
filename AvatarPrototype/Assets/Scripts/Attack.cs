using UnityEngine;
using System.Collections;

public abstract class Attack : MonoBehaviour
{
	// Use this for initialization
    protected virtual void Start()
	{
	}
	
	// Update is called once per frame
	protected virtual void Update()
    {
    }

	public abstract void Activate();

    public virtual void Deactivate() { }

    public virtual void OnTerrain(GameObject terrain) { }

    public virtual void OnPlayer(GameObject player) { }
}
