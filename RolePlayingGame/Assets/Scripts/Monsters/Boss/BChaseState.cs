using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BChaseState : IEnemyState
{
    private readonly BossMonster bossMon;
    
    public BChaseState(BossMonster bossMonster)
    {
        bossMon = bossMonster;
    }

    public void UpdateState()
    {
        chase();
    }

    public void ToPatrolState()
    {
        bossMon.curState = bossMon.ptlState;
    }

    public void ToChaseState()
    {

    }

    public void ToAttackState()
    {
        bossMon.curState = bossMon.atkState;
    }

    private void chase()
    {
        if (bossMon.GetDistToPlayer() < 10.0f && bossMon.GetDistToPlayer() > 2.0f) { bossMon.Chase(); }

        if (bossMon.GetDistToPlayer() <= 2.0f) { ToAttackState(); }

        if (bossMon.GetDistToPlayer() > 10.0f) { ToPatrolState(); }
    }
}
