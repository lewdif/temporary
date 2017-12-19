using System.Collections;
using UnityEngine;

public class EChaseState : IEnemyState
{
    private readonly EliteMonster elMon;

    public EChaseState(EliteMonster eliteMonster)
    {
        elMon = eliteMonster;
    }

    public void UpdateState()
    {
        chase();
    }

    public void ToPatrolState()
    {
        elMon.curState = elMon.ptlState;
    }

    public void ToChaseState()
    {

    }

    public void ToAttackState()
    {
        elMon.curState = elMon.atkState;
    }

    private void chase()
    {
        if (elMon.GetDistToPlayer() < 8.0f && elMon.GetDistToPlayer() > 2.0f) { elMon.Chase(); }

        if (elMon.GetDistToPlayer() <= 2.0f) { ToAttackState(); }

        if (elMon.GetDistToPlayer() > 8.0f) { ToPatrolState(); }
    }
}
