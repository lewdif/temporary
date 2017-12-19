using UnityEngine;
using System.Collections;

public interface IMonster
{
    void Attack();

    void Chase();

    void Patrol();

    void GetHit(int damage);
}
