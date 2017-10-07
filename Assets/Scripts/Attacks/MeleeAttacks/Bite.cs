using UnityEngine;
using System.Collections;

public class Bite : BaseAttack {

    public Bite()
        {
            description = "Muerde a su objetivo usando sus fauces";
            attackName = "Mordida";
            attackDamage = 100f;
            attackCost = 0f;
            statsUp = 0f;
        }
}
