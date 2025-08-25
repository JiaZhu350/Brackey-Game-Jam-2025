using System.Collections;
using UnityEngine;

public interface IAttack
{
    bool rbRestricted {  get; set; }
    IEnumerator AttackPlayer(Transform target, float dmg, float windup, float cd, Rigidbody2D rb);
}
