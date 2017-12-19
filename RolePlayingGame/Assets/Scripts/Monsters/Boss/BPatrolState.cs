using System.Collections;
using UnityEngine;

public class BPatrolState : IEnemyState
{
    private readonly BossMonster bossMon;

    public BPatrolState(BossMonster bossMonster)
    {
        bossMon = bossMonster;
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
        bossMon.curState = bossMon.chsState;
    }

    public void ToAttackState()
    {

    }

    private void patrol()
    {
        bossMon.Patrol();

        if (bossMon.GetDistToPlayer() < 10.0f) { ToChaseState(); }
    }
}
