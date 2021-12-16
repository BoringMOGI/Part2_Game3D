using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stateable : MonoBehaviour
{
    [System.Serializable]
    public struct StatData
    {
        public float hp;
        public float abilityPower;        // ���� ���ݷ�.
        public float attackDamage;        // ���� ���ݷ�.
        public float abilityDefence;      // ���� ����.
        public float attackDefence;       // ���� ����.
    }

    [SerializeField] StatData stat;

    public StatData Stat => stat;
}
