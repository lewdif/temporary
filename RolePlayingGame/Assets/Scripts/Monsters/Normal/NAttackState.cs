using UnityEngine;
using System.Collections;

public class NAttackState : IEnemyState
{
    private readonly NormalMonster norMon;

    public NAttackState(NormalMonster normalMonster)
    {
        norMon = normalMonster;
    }

    public void UpdateState()
    {
        attack();
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

    private void attack()
    {
        if (norMon.GetIsAttack().Equals(false)) { norMon.Attack(); }

        if (norMon.GetDistToPlayer() > 2.0f) { ToChaseState(); }
    }
}
