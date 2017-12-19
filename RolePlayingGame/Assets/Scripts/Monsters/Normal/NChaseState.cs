using UnityEngine;
using System.Collections;

public class NChaseState : IEnemyState
{
    private readonly NormalMonster norMon;

    public NChaseState(NormalMonster normalMonster)
    {
        norMon = normalMonster;
    }

    public void UpdateState()
    {
        chase();
    }

    public void ToPatrolState()
    {
        norMon.curState = norMon.ptlState;
    }

    public void ToChaseState()
    {

    }

    public void ToAttackState()
    {
        norMon.curState = norMon.atkState;
    }

    private void chase()
    {
        if (norMon.GetDistToPlayer() < 5.0f && norMon.GetDistToPlayer() > 2.0f) { norMon.Chase(); }

        if(norMon.GetDistToPlayer() <= 2.0f) { ToAttackState(); }

        if(norMon.GetDistToPlayer() > 5.0f) { ToPatrolState(); }
    }
}
