using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    private GameObject player;
    private GameObject playerBody;
    private Transform playerTrans;
    private TextMesh textMesh;
    private Vector3 newPos;
    private bool isMoving;
    private int trapCnt;

    private float speed;
    private float hp;

    GridManager gm = null;
    Coroutine move_coroutine = null;

    private GameObject trap;
    private GameObject trapColne;
    GameObject curIdx;
    List<GameObject> trapList;
    Coroutine install_trap = null;

    void Start ()
    {
        player = this.gameObject;
        playerBody = GameObject.Find("Player/Body");
        playerTrans = player.transform;
        textMesh = this.GetComponentInChildren<TextMesh>();
        newPos = Vector3.zero;
        isMoving = false;
        trapCnt = 0;

        speed = 5.0f;
        hp = 50;

        gm = Camera.main.GetComponent<GridManager>() as GridManager;
        gm.BuildWorld(70, 70);

        trap = GameObject.Find("Trap");
        trapList = new List<GameObject>();
        enitTrap();

        textMesh.GetComponent<Renderer>().enabled = false;
    }
	
	void Update ()
    {
        textMesh.text = hp + " / 50";
        isMoving = false;
        newPos = playerTrans.position;
        moveByKey();
        BtnClick();
    }

    private void moveByKey()
    {
        if (hp <= 0) { return; }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            isMoving = true;

            if (move_coroutine != null) { StopCoroutine(move_coroutine); }
            if (install_trap != null) { StopCoroutine(install_trap); }

            transform.position += transform.right * Time.deltaTime * speed;
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            isMoving = true;

            if (move_coroutine != null) { StopCoroutine(move_coroutine); }
            if (install_trap != null) { StopCoroutine(install_trap); }

            transform.position -= transform.right * Time.deltaTime * speed;
        }
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            isMoving = true;

            if (move_coroutine != null) { StopCoroutine(move_coroutine); }
            if (install_trap != null) { StopCoroutine(install_trap); }

            transform.position += transform.forward * Time.deltaTime * speed;
        }
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            isMoving = true;

            if (move_coroutine != null) { StopCoroutine(move_coroutine); }
            if (install_trap != null) { StopCoroutine(install_trap); }

            transform.position -= transform.forward * Time.deltaTime * speed;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (install_trap != null) { StopCoroutine(install_trap); }

            install_trap = StartCoroutine(installTrap());
        }
    }

    private void BtnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (move_coroutine != null) { StopCoroutine(move_coroutine); }
                move_coroutine = StartCoroutine(gm.Move(gameObject, hit.point, 5.0f));
            }
        }
    }

    private void enitTrap()
    {
        trapList.Add(trap);

        while (trapList.Count < 10)
        {
            trapColne = Instantiate(trap, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            trapColne.name = "Trap" + trapList.Count;
            trapList.Add(trapColne);
        }

        int i = 0;
        while (i < trapList.Count)
        {
            trapList[i].GetComponent<Renderer>().enabled = false;
            i++;
        }
    }

    private IEnumerator installTrap()
    {
        int i = 0;

        while (trapList[i].GetComponent<Renderer>().enabled.Equals(true))
        {
            curIdx = trapList[i];
            i++;
            if (i > 9)
                break;
        }
        Debug.Log(i);
        if (i < 10)
        {
            yield return new WaitForSeconds(2.5f);

            trapList[i].GetComponent<Renderer>().enabled = true;
            trapList[i].transform.position = playerTrans.position;
            trapCnt++;
        }
    }

    public void PlayerLoseHP(int damage)
    {
        hp -= damage;

        StartCoroutine(showHP());

        if (hp <= 0)
        {
            hp = 0;
            player.GetComponent<Renderer>().enabled = false;
            playerBody.GetComponent<Renderer>().enabled = false;
        }

    }

    private IEnumerator showHP()
    {
        if (hp > 0)
        {
            textMesh.GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(3.0f);
            textMesh.GetComponent<Renderer>().enabled = false;
        }
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }

    public void SetIsMoving(bool input)
    {
        isMoving = input;
    }
}