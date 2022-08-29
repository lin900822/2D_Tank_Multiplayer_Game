using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;
using Game.Core;
using FusionExamples.FusionHelpers;

namespace Game.Core
{
    public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
    {
        private GameManager gameManager = null;
        private NetworkRunner networkRunner = null;

        [SerializeField] private NetworkObject playerPrefab = null;

        [SerializeField] private Camera mainCamera = null;

        private Dictionary<PlayerRef, NetworkObject> playerList = new Dictionary<PlayerRef, NetworkObject>();

        private void Start()
        {
            gameManager = GameManager.Instance;

            networkRunner = gameManager.Runner;

            networkRunner.AddCallbacks(this);

            SpawnAllPlayers();
        }

        private void SpawnAllPlayers()
        {
            foreach(var player in gameManager.PlayerList.Keys)
            {
                Vector3 spawnPosition = Vector3.zero;
                NetworkObject networkPlayerObject = networkRunner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);

                networkRunner.SetPlayerObject(player, networkPlayerObject);

                playerList.Add(player, networkPlayerObject);
            }
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Vector3 spawnPosition = Vector3.zero;
            NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);

            runner.SetPlayerObject(player, networkPlayerObject);

            playerList.Add(player, networkPlayerObject);
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (playerList.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                playerList.Remove(player);
            }
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new NetworkInputData();

            float xInput = Input.GetAxisRaw("Horizontal");
            float yInput = Input.GetAxisRaw("Vertical");

            Vector2 mouseToWorldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            if (runner.TryGetPlayerObject(runner.LocalPlayer, out NetworkObject networkObject))
            {
                float angle = Vector2.SignedAngle(Vector2.up, mouseToWorldPoint - new Vector2(networkObject.transform.position.x, networkObject.transform.position.y));

                data.rotation = angle;
            }

            data.movementInput = new Vector2(xInput, yInput);

            data.buttons.Set(InputButtons.FIRE, Input.GetKey(KeyCode.Mouse0));

            input.Set(data);
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnDisconnectedFromServer(NetworkRunner runner) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    }
}


