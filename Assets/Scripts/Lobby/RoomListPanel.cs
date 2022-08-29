using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace Game.Lobby
{
    public class RoomListPanel : MonoBehaviour, IPanel
    {
        [SerializeField] private LobbyManager lobbyManager = null;

        [SerializeField] private CanvasGroup canvasGroup = null;

        [SerializeField] private RoomCell roomCellPrefab = null;

        [SerializeField] private Transform contentTrans = null;

        private List<RoomCell> roomCells = new List<RoomCell>();

        public void DisplayPanel(bool value)
        {
            canvasGroup.alpha = value ? 1 : 0;
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
        }

        public void UpdateRoomList(List<SessionInfo> sessionList)
        {
            foreach(var cell in roomCells)
            {
                Destroy(cell.gameObject);
            }

            roomCells.Clear();

            foreach(var session in sessionList)
            {
                var cell = Instantiate(roomCellPrefab, contentTrans);

                cell.SetInfo(lobbyManager, session.Name);
            }
        }

        public void OnCreateRoomBtnClick()
        {
            lobbyManager.SetPairState(PairState.CreatingRoom);
        }
    }

}
