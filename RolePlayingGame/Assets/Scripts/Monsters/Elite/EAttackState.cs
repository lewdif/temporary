using System.Collections;
using UnityEngine;

public class EAttackState : IEnemyState
{
    private readonly EliteMonster elMon;

    public EAttackState(EliteMonster eliteMonster)
    {
        elMon = eliteMonster;
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
        elMon.curState = elMon.chsState;
    }

    public void ToAttackState()
    {

    }

    private void attack()
    {
        if (elMon.GetIsAttack().Equals(false)) { elMon.Attack(); }

        if (elMon.GetDistToPlayer() > 2.0f) { ToChaseState(); }
    }
}
