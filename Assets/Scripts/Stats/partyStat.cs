using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class partyStat: BaseClass
{
    public float Xp;
    public float NeededXp;
  
    public Type HeroType;

    public List<BaseAttack> MagicAttacks = new List<BaseAttack>();

}
