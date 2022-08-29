using Fusion;
using System.Collections;
using UnityEngine;
using TMPro;    

namespace Game.Core
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private PlayerMovementHandler movementHandler = null;

        [SerializeField] private PlayerAttackHandler attackHandler = null;

        [SerializeField] private PlayerViewHandler viewHandler = null;

        [SerializeField] private HealthPoint healthPoint = null;

        [SerializeField] private NetworkRigidbody2D playerNetworkRigidbody = null;

        [Networked] private NetworkButtons buttonsPrevious { get; set; }

        [Networked] private TickTimer respawnTimer { get; set; }

        public override void Spawned()
        {
            healthPoint.Subscribe(OnHPChanged);

            if(GameManager.Instance.PlayerList.TryGetValue(Object.InputAuthority, out var playerNetworkData))
            {
                viewHandler.SetPlayerNameTxt(playerNetworkData.PlayerName);
                viewHandler.SetTankColor(playerNetworkData.TankColor);
                viewHandler.SetBarrelColor(playerNetworkData.BarrelColor);
            }

            if (Object.HasInputAuthority)
            {
                playerNetworkRigidbody.InterpolationDataSource = InterpolationDataSources.Predicted;
            }
            else
            {
                playerNetworkRigidbody.InterpolationDataSource = InterpolationDataSources.Snapshots;
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            healthPoint.Unsubscribe(OnHPChanged);
        }

        private void OnHPChanged(float value)
        {
            viewHandler.SetHPBarValue((float)value / healthPoint.MaxHP);
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data) && !respawnTimer.IsRunning)
            {
                ApplyInput(data);
            }

            if (healthPoint.HP <= 0 && !respawnTimer.IsRunning)
            {
                Die();
            }

            if (respawnTimer.Expired(Runner))
            {
                Respawn();
                respawnTimer = TickTimer.None;
            }
        }

        private void ApplyInput(NetworkInputData data)
        {
            NetworkButtons buttons = data.buttons;
            var pressed = buttons.GetPressed(buttonsPrevious);
            buttonsPrevious = buttons;

            movementHandler.Move(data);
            movementHandler.SetRotation(data.rotation);

            if (pressed.IsSet(InputButtons.FIRE))
            {
                attackHandler.Shoot();
            }
        }

        private void Die()
        {
            if (Object.HasStateAuthority)
            {
                respawnTimer = TickTimer.CreateFromSeconds(Runner, 2f);

                playerNetworkRigidbody.TeleportToPosition(new Vector2(-100, -100));

            }
        }

        private void Respawn()
        {
            if (Object.HasStateAuthority)
            {
                healthPoint.HP = healthPoint.MaxHP;

                var spawnManager = FindObjectOfType<SpawnManager>();

                var spawnPoint = spawnManager.GetRandomSpawnPoint();

                playerNetworkRigidbody.TeleportToPosition(spawnPoint.position);
            }
        }
    }
}