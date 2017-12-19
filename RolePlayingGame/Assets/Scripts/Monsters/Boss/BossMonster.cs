using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : MonoBehaviour, IMonster
{
    private int monHP;
    private int monDir;     // X or Z selection by random
    private int monAtk;
    private float monAtkSpd;
    private float monSpd;
    private float disToPlayer;
    private bool isAtk;
    private int isReached;
    private int randNum;

    private GameObject player;
    private GameObject bossMonster;
    private GameObject detectRng;
    private GameObject chaseRng;
    private GridManager gridMgr;
    private TextMesh textMesh;

    Coroutine chaseCoroutine;
    Coroutine patrolCoroutine;
    Vector3 monPos;

    public IEnemyState curState;
    public BAttackState atkState;
    public BPatrolState ptlState;
    public BChaseState chsState;

    void Start()
    {
        monHP = 100;
        monDir = Random.Range(0, 2);
        monAtk = 10;
        monAtkSpd = 0.666f;
        monSpd = 4.1666f;
        isAtk = false;
        isReached = 0;

        player = GameObject.Find("Player");
        bossMonster = this.gameObject;
        detectRng = GameObject.Find(bossMonster.name + "/DetectRange");
        chaseRng = GameObject.Find(bossMonster.name + "/ChaseRange");
        gridMgr = player.GetComponentInChildren<GridManager>();
        textMesh = this.GetComponentInChildren<TextMesh>();

        monPos = new Vector3(Random.Range(1, 69), 0, Random.Range(1, 69));

        bossMonster.transform.position = monPos;

        atkState = new BAttackState(this);
        ptlState = new BPatrolState(this);
        chsState = new BChaseState(this);

        detectRng.GetComponent<Renderer>().enabled = false;
        chaseRng.GetComponent<Renderer>().enabled = false;
        textMesh.GetComponent<Renderer>().enabled = false;

        curState = ptlState;
    }

    void Update()
    {
        disToPlayer = Vector3.Distance(this.transform.position, player.transform.position);

        textMesh.text = monHP + " / 100";

        curState.UpdateState();
    }

    public void Attack()
    {
        WaitForSeconds atkSpd = new WaitForSeconds(monAtkSpd);

        StartCoroutine(startAtk(atkSpd));
    }

    public void Chase()
    {
        if (chaseCoroutine != null) { StopCoroutine(chaseCoroutine); }
        if (patrolCoroutine != null) { StopCoroutine(patrolCoroutine); }

        chaseCoroutine = StartCoroutine(gridMgr.Move(gameObject, player.transform.position, monSpd));
    }

    public void Patrol()
    {
        if (chaseCoroutine != null) { StopCoroutine(chaseCoroutine); }
        if (patrolCoroutine != null) { StopCoroutine(patrolCoroutine); }

        if (disToPlayer < 30.0f && player.GetComponent<Character>().GetIsMoving().Equals(true))
        {
            Debug.Log("Patrol");
            chaseCoroutine = StartCoroutine(gridMgr.Move(gameObject, player.transform.position, monSpd));
        }
    }

    IEnumerator startAtk(WaitForSeconds atkSpd)
    {
        while (disToPlayer < 2.0f)
        {
            isAtk = true;
            player.GetComponent<Character>().PlayerLoseHP(monAtk);

            yield return new WaitForSeconds(monAtkSpd);

            isAtk = false;
        }
    }

    public float GetDistToPlayer()
    {
        return disToPlayer;
    }

    public void GetHit(int damage)
    {
        monHP -= damage;

        if (monHP <= 0)
        {
            monHP = 0;
            Destroy(bossMonster);
        }

        StartCoroutine(showHP());
    }

    private IEnumerator showHP()
    {
        textMesh.GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(3.0f);
        textMesh.GetComponent<Renderer>().enabled = false;
    }

    public bool GetIsAttack()
    {
        return isAtk;
    }

    public int GetIsReached()
    {
        return isReached;
    }

    public void SetIsReached(int input)
    {
        isReached = input;
    }

    public int GetRandNum()
    {
        return randNum;
    }

    public void SetRandNum(int input)
    {
        randNum = input;
    }
}
