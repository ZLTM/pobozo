using UnityEngine;
using System.Collections;

public class Freeze1 : BaseAttack
{
    public Freeze1()
    {
        description = "congela al enemigo haciendo " + attackCost + " puntos de daño.";
        attackName = "Congelar 1";
        attackDamage = 30f;
        attackCost = 10f;
        statsUp = 0f;
    }
}
