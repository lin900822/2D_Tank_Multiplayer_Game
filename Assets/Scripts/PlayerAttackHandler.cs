using Fusion;
using System.Collections;
using UnityEngine;

namespace Game.Core
{
    public class PlayerAttackHandler : NetworkBehaviour
    {
        [SerializeField] private Bullet bulletPrefab = null;

        [SerializeField] private Transform shootPoint = null;

        public void Shoot()
        {
            var key = new NetworkObjectPredictionKey { Byte0 = (byte)Object.InputAuthority.RawEncoded, Byte1 = (byte)Runner.Simulation.Tick };

            Runner.Spawn(bulletPrefab, shootPoint.position, transform.rotation, Object.InputAuthority, null, key);
        }
    }
}