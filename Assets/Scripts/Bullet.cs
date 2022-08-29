using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class Bullet : NetworkBehaviour, IPredictedSpawnBehaviour
    {
        [SerializeField] private NetworkRigidbody2D networkRigidbody = null;

        [SerializeField] private float speed = 20f;
        [SerializeField] private int damage = 10;

        [Networked] private TickTimer life { get; set; }

        private List<LagCompensatedHit> hits = new List<LagCompensatedHit>();

        private Vector3 interpolateFrom;
        private Vector3 interpolateTo;

        public override void Spawned()
        {
            life = TickTimer.CreateFromSeconds(Runner, 5.0f);

            networkRigidbody.InterpolationTarget.gameObject.SetActive(true);

            networkRigidbody.Rigidbody.velocity = Vector2.zero;
        }

        public override void FixedUpdateNetwork()
        {
            networkRigidbody.Rigidbody.velocity = networkRigidbody.Transform.TransformDirection(Vector2.up) * speed;

            if (life.Expired(Runner))
            {
                Runner.Despawn(Object);
            }

            DetectCollision();
        }

        private void DetectCollision()
        {
            hits.Clear();

            if (Object == null) return;

            Runner.LagCompensation.OverlapBox(
                networkRigidbody.Transform.position,
                new Vector3(.2f, 0f, .4f),
                Quaternion.Euler(0, 0, networkRigidbody.Rigidbody.rotation),
                Object.InputAuthority,
                hits,
                -1,
                HitOptions.IncludePhysX);

            foreach (var hit in hits)
            {
                var netObj = hit.GameObject.GetComponent<NetworkBehaviour>().Object;

                if (netObj == null) return;

                var damagable = hit.GameObject.GetComponent<IDamagable>();

                if (damagable != null && netObj.InputAuthority != Object.InputAuthority)
                {
                    damagable.TakeDamage(damage);

                    networkRigidbody.InterpolationTarget.gameObject.SetActive(false);

                    Runner.Despawn(Object, true);
                }
            }
        }

        public void PredictedSpawnSpawned()
        {
            interpolateTo = transform.position;
            interpolateFrom = interpolateTo;
            networkRigidbody.InterpolationTarget.position = interpolateTo;
        }

        public void PredictedSpawnUpdate()
        {
            interpolateFrom = interpolateTo;
            interpolateTo = transform.position;

            Vector3 pos = networkRigidbody.Transform.position;
            pos += networkRigidbody.Transform.TransformDirection(Vector2.up) * speed * Runner.DeltaTime;
            networkRigidbody.Transform.position = pos;
        }

        public void PredictedSpawnRender()
        {
            var a = Runner.Simulation.StateAlpha;
            networkRigidbody.InterpolationTarget.position = Vector3.Lerp(interpolateFrom, interpolateTo, a);
        }

        public void PredictedSpawnFailed()
        {
            Runner.Despawn(Object, true);
        }

        public void PredictedSpawnSuccess()
        {
            
        }
    }
}

