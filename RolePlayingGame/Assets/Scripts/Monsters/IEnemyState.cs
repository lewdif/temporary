using UnityEngine;
using System.Collections;

public interface IEnemyState
{
    void UpdateState();

    void ToPatrolState();

    void ToChaseState();

    void ToAttackState();
}
