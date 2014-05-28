using UnityEngine;
using System.Collections;

public class Killbox : MonoBehaviour {

    public float scale = 1.1f;

	// Use this for initialization
	void Start()
    {
        BoxCollider2D b = GetComponent<BoxCollider2D>();
        Camera c = GetComponent<Camera>();
        float size = c.orthographicSize * 2.0f * scale;
        b.size = new Vector2(size * c.aspect, size);
	}
    
    void OnTriggerExit2D(Collider2D c)
    {
        PlayerController p = c.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            p.Kill();
        }
    }
}
