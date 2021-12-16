using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stateable : MonoBehaviour
{
    [System.Serializable]
    public struct StatData
    {
        public float hp;
        public float abilityPower;        // 마법 공격력.
        public float attackDamage;        // 물리 공격력.
        public float abilityDefence;      // 마법 방어력.
        public float attackDefence;       // 물리 방어력.
    }

    [SerializeField] StatData stat;

    public StatData Stat => stat;
}
