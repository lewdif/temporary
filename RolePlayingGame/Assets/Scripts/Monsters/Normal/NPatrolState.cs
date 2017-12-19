using UnityEngine;
using System.Collections;

public class NPatrolState : IEnemyState
{
    private readonly NormalMonster norMon;

    public NPatrolState(NormalMonster normalMonster)
    {
        norMon = normalMonster;
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
        norMon.curState = norMon.chsState;
    }

    public void ToAttackState()
    {

    }

    private void patrol()
    {
        norMon.Patrol();

        if (norMon.GetDistToPlayer() < 5.0f) { ToChaseState(); }
    }
}
