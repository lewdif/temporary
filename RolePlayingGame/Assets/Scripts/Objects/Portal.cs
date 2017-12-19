using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private GameObject player;
    private GameObject portal;
    private PortalManager portalMgr;
    private bool onPortal;

	void Start ()
    {
        player = GameObject.Find("Player");
        portal = GameObject.Find("Portal");
        portalMgr = GameObject.Find("GameManager").GetComponent<PortalManager>();
        onPortal = false;
	}

    void OnTriggerEnter(Collider col)
    {
        int num = Random.Range(0, 4);
        /*if (this.transform.position.Equals(portalMgr.GetPortalLoc(num)))
        {
            if (num == 0)   { num = 3; }
            else            { num--; }
        }*/

        if (col.name.Contains("Player") && onPortal.Equals(false))
        {
            col.transform.position = portalMgr.GetPortalLoc(num);
        }
        onPortal = true;
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name.Contains("Player"))
        {
            onPortal = false;
        }
    }
}
