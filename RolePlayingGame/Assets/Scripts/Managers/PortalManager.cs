using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    GameObject portal;
    GameObject portalClone;
    List<GameObject> portalList;

    GridManager gridMgr;

	void Start ()
    {
        portal = GameObject.Find("Portal");
        portalList = new List<GameObject>();
        gridMgr = GameObject.Find("Player/MainCamera").GetComponent<GridManager>();

        portalList.Add(portal);
        initPortal();
    }

    private void initPortal()
    {
        Vector3 toCenter = new Vector3(0.5f, 0, 0.5f);
        portalList[0].transform.position = new Vector3(Random.Range(1, 69), 0, Random.Range(1, 69)) + toCenter;

        while (portalList.Count < 4)
        {
            portalClone = Instantiate(portal, new Vector3(Random.Range(1, 69), 0, Random.Range(1, 69)) + toCenter, Quaternion.identity) as GameObject;
            portalClone.name = "NormalMonster" + portalList.Count;
            portalList.Add(portalClone);
        }
    }

    public Vector3 GetPortalLoc(int num)
    {
        return portalList[num].transform.position;
    }
}
