using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Trap : MonoBehaviour
{
    private GameObject nMon;
    private GameObject eMon;
    private GameObject bMon;

    private GameObject trap;
    private int trapDmg;

	void Start ()
    {
        nMon = GameObject.Find("NormalMonster");
        eMon = GameObject.Find("EliteMonster");
        bMon = GameObject.Find("BossMonster");

        trap = this.gameObject;
        trapDmg = 9;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name.Contains("Normal"))
        {
            col.gameObject.GetComponent<NormalMonster>().GetHit(trapDmg);
            this.GetComponent<Renderer>().enabled = false;
            this.transform.position += new Vector3(0, 100, 0);
        }

        if(col.name.Contains("Elite"))
        {
            col.gameObject.GetComponent<EliteMonster>().GetHit(trapDmg);
            this.GetComponent<Renderer>().enabled = false;
            this.transform.position += new Vector3(0, 10, 0);
        }

        if (col.name.Contains("Boss"))
        {
            col.gameObject.GetComponent<BossMonster>().GetHit(trapDmg);
            this.GetComponent<Renderer>().enabled = false;
            this.transform.position += new Vector3(0, 10, 0);
        }
    }
}
