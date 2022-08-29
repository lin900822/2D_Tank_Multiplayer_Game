using System.Collections;
using UnityEngine;
using Fusion;
using System;

namespace Game.Core
{
    public class HealthPoint : NetworkBehaviour, IDamagable
    {
        [SerializeField] private int maxHP;
        public int MaxHP => maxHP;

        [Networked(OnChanged = nameof(HandleHpChanged))] public float HP { get; set; }

        private Action<float> OnHpChanged = null;

        public override void Spawned()
        {
            HP = maxHP;
        }

        public void TakeDamage(float damage)
        {
            if(Object.HasStateAuthority)
                HP -= damage;
        }

        private static void HandleHpChanged(Changed<HealthPoint> changed)
        {
            HealthPoint temp = changed.Behaviour;

            temp.OnHpChanged?.Invoke(temp.HP);
        }

        public void Subscribe(Action<float> action)
        {
            OnHpChanged += action;
        }

        public void Unsubscribe(Action<float> action)
        {
            OnHpChanged -= action;
        }
    }
}