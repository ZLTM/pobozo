using UnityEngine;
using System.Collections;

public class Flow : BaseAttack
{
    public Flow()
    {
        description = "aumenta tu concentracion y fluye como el agua, aumenta tu agilidad en " + statsUp+ " puntos.";
        attackName = "Flow";
        attackDamage = 0f;
        attackCost = 10f;
        statsUp = 5f;
    }	
}
