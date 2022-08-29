using System.Collections;
using UnityEngine;
using Fusion;
using Game.Core;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

namespace Game.Lobby
{
    public class InRoomPanel : MonoBehaviour, IPanel
    {
        private GameManager gameManager = null;

        [SerializeField] private CanvasGroup canvasGroup = null;

        [SerializeField] private TMP_Text roomNameTxt = null;

        [SerializeField] private PlayerCell playerCellPrefab = null;
        [SerializeField] private Transform contentTrans = null;

        private List<PlayerCell> playerCells = new List<PlayerCell>();

        private void Start()
        {
            gameManager = GameManager.Instance;

            gameManager.OnPlayerListUpdated += UpdatePlayerList;
        }

        private void OnDestroy()
        {
            gameManager.OnPlayerListUpdated -= UpdatePlayerList;
        }

        public void UpdatePlayerList()
        {
            foreach(var cell in playerCells)
            {
                Destroy(cell.gameObject);
            }

            playerCells.Clear();

            foreach(var player in gameManager.PlayerList)
            {
                var cell = Instantiate(playerCellPrefab, contentTrans);

                var playerData = player.Value;

                cell.SetInfo(playerData.PlayerName, playerData.IsReady);

                playerCells.Add(cell);
            }
        }

        public void DisplayPanel(bool value)
        {
            canvasGroup.alpha = value ? 1 : 0;
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;

            var runner = gameManager.Runner;

            roomNameTxt.text = runner.SessionInfo.Name;
        }

        public void OnReadyBtnClicked()
        {
            var runner = gameManager.Runner;

            if (gameManager.PlayerList.TryGetValue(runner.LocalPlayer, out PlayerNetworkData playerNetworkData))
            {
                playerNetworkData.SetReady_RPC(true);
            }
        }
    }
}