using System.Collections;
using UnityEngine;

public class EPatrolState : IEnemyState
{
    private readonly EliteMonster elMon;

    public EPatrolState(EliteMonster eliteMonster)
    {
        elMon = eliteMonster;
    }

    public void UpdateState()
    {
        patrol();
    }

    public void ToPatrolState()
    {

    }

    public void ToChaseState()
    {
        elMon.curState = elMon.chsState;
    }

    public void ToAttackState()
    {

    }

    private void patrol()
    {
        elMon.Patrol();

        if (elMon.GetDistToPlayer() < 8.0f) { ToChaseState(); }
    }
}
