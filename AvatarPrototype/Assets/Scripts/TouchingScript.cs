using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class TouchingScript : MonoBehaviour {

    public bool touching = false;

    public int count = 0;

    private bool newContact = false;
    public bool NewContact
    {
        get
        {
            bool temp = newContact;
            newContact = false;
            return temp;
        }
    }

    private bool lostContact = false;
    public bool LostContact
    {
        get
        {
            bool temp = lostContact;
            lostContact = false;
            return temp;
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "SolidTerrain")
        {
            touching = ++count > 0;
            if (count == 1)
            {
                newContact = true;
                lostContact = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.tag == "SolidTerrain")
        {
            touching = --count > 0;
            if (count == 0)
            {
                lostContact = true;
                newContact = false;
            }
        }
    }
}
