using UnityEngine;
using System.Collections;

public class NormalMonster : MonoBehaviour, IMonster
{
    private int monHP;
    private int monDir;     // X or Z selection by random
    private int monAtk;
    private float monAtkSpd;
    private float monSpd;
    private float disToPlayer;
    private bool isAtk;
    private bool isReached;

    private GameObject player;
    private GameObject normalMonster;
    private GameObject detectRng;
    private GridManager gridMgr;
    private MonsterManager monMgr;
    private TextMesh textMesh;

    Coroutine chaseCoroutine;
    Coroutine patrolCoroutine;
    Vector3 monPos;

    public IEnemyState curState;
    public NAttackState atkState;
    public NPatrolState ptlState;
    public NChaseState chsState;

    void Start()
    {
        monHP = 30;
        monDir = Random.Range(0, 2);
        monAtk = 2;
        monAtkSpd = 0.666f;
        monSpd = 3.333f;
        isAtk = false;
        isReached = false;

        player = GameObject.Find("Player");
        normalMonster = this.gameObject;
        detectRng = GameObject.Find(normalMonster.name + "/Range");
        gridMgr = player.GetComponentInChildren<GridManager>();
        monMgr = GameObject.Find("GameManager").GetComponent<MonsterManager>();
        textMesh = this.GetComponentInChildren<TextMesh>();
        
        monPos = new Vector3(Random.Range(1, 69), 0, Random.Range(1, 69));

        normalMonster.transform.position = monPos;

        atkState = new NAttackState(this);
        ptlState = new NPatrolState(this);
        chsState = new NChaseState(this);

        detectRng.GetComponent<Renderer>().enabled = false;
        textMesh.GetComponent<Renderer>().enabled = false;

        curState = ptlState;
    }

    void Update()
    {
        disToPlayer = Vector3.Distance(this.transform.position, player.transform.position);

        textMesh.text = monHP + " / 30";

        curState.UpdateState();
    }

    public void Attack()
    {
        WaitForSeconds atkSpd = new WaitForSeconds(monAtkSpd);

        StartCoroutine(startAtk(atkSpd));
    }

    public void Chase()
    {
        if (chaseCoroutine != null)     { StopCoroutine(chaseCoroutine); }
        if (patrolCoroutine != null)    { StopCoroutine(patrolCoroutine); }

        chaseCoroutine = StartCoroutine(gridMgr.Move(gameObject, player.transform.position, monSpd));
    }

    public void Patrol()
    {
        if (chaseCoroutine != null)     { StopCoroutine(chaseCoroutine); }
        if (patrolCoroutine != null)    { StopCoroutine(patrolCoroutine); }

        if (monDir.Equals(0))   { patrolCoroutine = StartCoroutine(gridMgr.MoveInCol(gameObject, disToPlayer, monSpd)); }
        else                    { patrolCoroutine = StartCoroutine(gridMgr.MoveInRow(gameObject, disToPlayer, monSpd)); }
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
            monMgr.RemoveNorMon(normalMonster);
            Destroy(normalMonster);
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

    public bool GetIsReached()
    {
        return isReached;
    }

    public void SetIsReached(bool input)
    {
        isReached = input;
    }
}