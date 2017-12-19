using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private GameObject nMon;
    private GameObject eMon;
    private GameObject bMon;

    private GameObject nMonColne;
    private GameObject eMonColne;
    private GameObject bMonColne;

    private List<GameObject> nMonsterList;
    private List<GameObject> eMonsterList;

    void Start ()
    {
        nMon = GameObject.Find("NormalMonster");
        eMon = GameObject.Find("EliteMonster");
        bMon = GameObject.Find("BossMonster");

        nMonsterList = new List<GameObject>();
        eMonsterList = new List<GameObject>();

        nMonsterList.Add(nMon);
        eMonsterList.Add(eMon);

        StartCoroutine(createNorMon());
        StartCoroutine(createElMon());
    }
	
	void Update ()
    {
        showDetectRng();
    }

    private IEnumerator createNorMon()
    {
        int i = 0;
        while (nMonsterList.Count <= 4)
        {
            nMonColne = Instantiate(nMon, new Vector3(Random.Range(1, 69), 0, Random.Range(1, 69)), Quaternion.identity) as GameObject;
            nMonColne.name = "NormalMonster" + i;
            nMonsterList.Add(nMonColne);

            i++;
            yield return null;
        }
    }

    private IEnumerator createElMon()
    {
        int i = 0;
        while (eMonsterList.Count <= 2)
        {
            eMonColne = Instantiate(eMon, new Vector3(Random.Range(1, 69), 0, Random.Range(1, 69)), Quaternion.identity) as GameObject;
            eMonColne.name = "EliteMonster" + i;
            eMonsterList.Add(eMonColne);

            i++;
            yield return null;
        }
    }

    private void showDetectRng()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            for (int i = 0; i < nMonsterList.Count; i++)
            {
                if (GameObject.Find(nMonsterList[i].name + "/Range").GetComponent<Renderer>().enabled.Equals(true))
                {
                    GameObject.Find(nMonsterList[i].name + "/Range").GetComponent<Renderer>().enabled = false;
                }
                else
                {
                    GameObject.Find(nMonsterList[i].name + "/Range").GetComponent<Renderer>().enabled = true;
                }
            }

            for (int i = 0; i < eMonsterList.Count; i++)
            {
                if (GameObject.Find(eMonsterList[i].name + "/Range").GetComponent<Renderer>().enabled.Equals(true))
                {
                    GameObject.Find(eMonsterList[i].name + "/Range").GetComponent<Renderer>().enabled = false;
                }
                else
                {
                    GameObject.Find(eMonsterList[i].name + "/Range").GetComponent<Renderer>().enabled = true;
                }
            }

            if (bMon)
            {
                if (GameObject.Find(bMon.name + "/DetectRange").GetComponent<Renderer>().enabled.Equals(true))
                {
                    GameObject.Find(bMon.name + "/ChaseRange").GetComponent<Renderer>().enabled = false;
                    GameObject.Find(bMon.name + "/DetectRange").GetComponent<Renderer>().enabled = false;
                }
                else
                {
                    GameObject.Find(bMon.name + "/ChaseRange").GetComponent<Renderer>().enabled = true;
                    GameObject.Find(bMon.name + "/DetectRange").GetComponent<Renderer>().enabled = true;
                }
            }
        }
    }

    public void RemoveNorMon(GameObject _nMon)
    {
        nMonsterList.Remove(_nMon);
    }

    public void RemoveElMon(GameObject _eMon)
    {
        eMonsterList.Remove(_eMon);
    }
}
