using UnityEngine;
using System.Collections;

public class EliteMonster : MonoBehaviour, IMonster
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
    private GameObject eliteMonster;
    private GameObject detectRng;
    private GridManager gridMgr;
    private MonsterManager monMgr;
    private TextMesh textMesh;

    Coroutine chaseCoroutine;
    Coroutine patrolCoroutine;
    Vector3 monPos;

    public IEnemyState curState;
    public EAttackState atkState;
    public EPatrolState ptlState;
    public EChaseState chsState;

    void Start()
    {
        monHP = 40;
        monDir = Random.Range(0, 2);
        monAtk = 5;
        monAtkSpd = 0.666f;
        monSpd = 3.75f;
        isAtk = false;
        isReached = 0;

        player = GameObject.Find("Player");
        eliteMonster = this.gameObject;
        detectRng = GameObject.Find(eliteMonster.name + "/Range");
        gridMgr = player.GetComponentInChildren<GridManager>();
        monMgr = GameObject.Find("GameManager").GetComponent<MonsterManager>() ;
        textMesh = this.GetComponentInChildren<TextMesh>();
        
        monPos = new Vector3(Random.Range(1, 69), 0, Random.Range(1, 69));

        eliteMonster.transform.position = monPos;

        atkState = new EAttackState(this);
        ptlState = new EPatrolState(this);
        chsState = new EChaseState(this);

        detectRng.GetComponent<Renderer>().enabled = false;
        textMesh.GetComponent<Renderer>().enabled = false;

        curState = ptlState;
    }

    void Update()
    {
        disToPlayer = Vector3.Distance(this.transform.position, player.transform.position);

        textMesh.text = monHP + " / 40";

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

        patrolCoroutine = StartCoroutine(gridMgr.MoveRandom(gameObject, disToPlayer, monSpd));
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
            monMgr.RemoveElMon(eliteMonster);
            Destroy(eliteMonster);
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
