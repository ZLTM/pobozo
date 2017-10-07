using UnityEngine;
using System.Collections;

public class BaseAttack : MonoBehaviour
{
    public string description;

    public string attackName;

    public float attackDamage; //daño de ataque, varia de acuerdo al nivel, items, etc
    public float attackCost; //costo en chama(?)

    public float statsUp;
}
