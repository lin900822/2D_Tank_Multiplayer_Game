using Fusion;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private NetworkRunner runner = null;
        public NetworkRunner Runner
        {
            get
            {
                if (runner == null)
                {
                    runner = gameObject.AddComponent<NetworkRunner>();
                    
                    runner.ProvideInput = true;
                }

                return runner;
            }
        }

        public Color TankColor = new Color(1, 1, 1, 1);
        public Color BarrelColor = new Color(1, 1, 1, 1);
        public string PlayerName = null;

        public Dictionary<PlayerRef, PlayerNetworkData> PlayerList = new Dictionary<PlayerRef, PlayerNetworkData>();

        public event Action OnPlayerListUpdated = null;

        private void Awake()
        {
            Runner.ProvideInput = true;

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        private bool CheckAllPlayerIsReady()
        {
            if (!Runner.IsServer) return false;

            foreach (var playerData in PlayerList.Values)
            {
                if (!playerData.IsReady) return false;
            }

            foreach (var playerData in PlayerList.Values)
            {
                playerData.IsReady = false;
            }

            return true;
        }

        public void UpdatePlayerList()
        {
            OnPlayerListUpdated?.Invoke();

            if (CheckAllPlayerIsReady())
            {
                Runner.SetActiveScene("GamePlay");
            }
        }

        public void SetPlayerNetworkData()
        {
            if (PlayerList.TryGetValue(runner.LocalPlayer, out PlayerNetworkData playerNetworkData))
            {
                playerNetworkData.SetPlayerName_RPC(PlayerName);
                playerNetworkData.SetTankColor_RPC(TankColor);
                playerNetworkData.SetBarrelColor_RPC(BarrelColor);
            }
        }
    }

}
