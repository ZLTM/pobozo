using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseClass
{
    public string baseName;

    public int Level;

    public float Hp;
    public float CurrentHp;
    public float Chama;
    public float CurrentChama;

    public float Fue;
    public float Inte;
    public float Agi;
    public float Def;

    public float Stun;
    public float Freeze;

    public enum Type
    {
        Water,
        Fire,
        Earth,
        Wind,
        Dark,
        Light
    }

    public List<BaseAttack> Attacks = new List<BaseAttack>();


}
