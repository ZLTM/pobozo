using UnityEngine;
using System.Collections;

public class StandarAttack :BaseAttack
{
    public StandarAttack()
    {
        description = "Golpe normal, limpia el ajayu de su objetivo";
        attackName = "golpe";
        attackDamage = 50f;
        attackCost = 0f;
        statsUp = 0;
    }
}
