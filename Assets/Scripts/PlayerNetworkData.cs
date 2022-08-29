using System.Collections;
using UnityEngine;
using Fusion;

namespace Game.Core
{
    public class PlayerNetworkData : NetworkBehaviour
    {
		private GameManager gameManager = null;

		[Networked(OnChanged = nameof(OnPlayerNameChanged))] public string PlayerName { get; set; }
		[Networked(OnChanged = nameof(OnIsReadyChanged))] public NetworkBool IsReady { get; set; }

		[Networked] public int KillAmount { get; set; }
		
		[Networked] public Color TankColor { get; set; }

		[Networked] public Color BarrelColor { get; set; }

        public override void Spawned()
        {
			gameManager = GameManager.Instance;

			transform.SetParent(GameManager.Instance.transform);

			gameManager.PlayerList.Add(Object.InputAuthority, this);
			gameManager.UpdatePlayerList();

			if (Object.HasInputAuthority)
			{
				SetPlayerName_RPC(gameManager.PlayerName);
				SetTankColor_RPC(gameManager.TankColor);
				SetBarrelColor_RPC(gameManager.BarrelColor);
			}
		}

        #region - RPCs -

        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
		public void SetPlayerName_RPC(string name)
        {
			PlayerName = name;
		}

		[Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
		public void SetReady_RPC(bool isReady)
		{
			IsReady = isReady;
		}

		[Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
		public void SetTankColor_RPC(Color color)
		{
			TankColor = color;
		}

		[Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
		public void SetBarrelColor_RPC(Color color)
		{
			BarrelColor = color;
		}
        #endregion

        #region - OnChanged Events -
        private static void OnPlayerNameChanged(Changed<PlayerNetworkData> changed)
        {
			GameManager.Instance.UpdatePlayerList();
		}

		private static void OnIsReadyChanged(Changed<PlayerNetworkData> changed)
		{
			GameManager.Instance.UpdatePlayerList();
		}
        #endregion
    }
}