using System.Collections;
using UnityEngine;

public class BAttackState : IEnemyState
{
    private readonly BossMonster bossMon;

    public BAttackState(BossMonster BossMonster)
    {
        bossMon = BossMonster;
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
        bossMon.curState = bossMon.chsState;
    }

    public void ToAttackState()
    {

    }

    private void attack()
    {
        if (bossMon.GetIsAttack().Equals(false)) { bossMon.Attack(); }

        if (bossMon.GetDistToPlayer() > 2.0f) { ToChaseState(); }
    }
}
