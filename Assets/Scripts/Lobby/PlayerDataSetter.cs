using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Core;

namespace Game.Lobby
{
    public class PlayerDataSetter : MonoBehaviour
    {
        private GameManager gameManager = null;

        [SerializeField] private SpriteRenderer tankSpriteRenderer = null;
        [SerializeField] private SpriteRenderer barrelSpriteRenderer = null;

        private void Start()
        {
            gameManager = GameManager.Instance;

            tankSpriteRenderer.color = gameManager.TankColor;
            barrelSpriteRenderer.color = gameManager.BarrelColor;
        }

        public void OnTankRSliderChanged(float value)
        {
            gameManager.TankColor.r = value / 255f;
            tankSpriteRenderer.color = gameManager.TankColor;

            gameManager.SetPlayerNetworkData();
        }

        public void OnTankGSliderChanged(float value)
        {
            gameManager.TankColor.g = value / 255f;
            tankSpriteRenderer.color = gameManager.TankColor;

            gameManager.SetPlayerNetworkData();
        }

        public void OnTankBSliderChanged(float value)
        {
            gameManager.TankColor.b = value / 255f;
            tankSpriteRenderer.color = gameManager.TankColor;

            gameManager.SetPlayerNetworkData();
        }

        public void OnBarrelRSliderChanged(float value)
        {
            gameManager.BarrelColor.r = value / 255f;
            barrelSpriteRenderer.color = gameManager.BarrelColor;

            gameManager.SetPlayerNetworkData();
        }

        public void OnBarrelGSliderChanged(float value)
        {
            gameManager.BarrelColor.g = value / 255f;
            barrelSpriteRenderer.color = gameManager.BarrelColor;

            gameManager.SetPlayerNetworkData();
        }

        public void OnBarrelBSliderChanged(float value)
        {
            gameManager.BarrelColor.b = value / 255f;
            barrelSpriteRenderer.color = gameManager.BarrelColor;

            gameManager.SetPlayerNetworkData();
        }

        public void OnPlayerNameInputFieldChange(string value)
        {
            gameManager.PlayerName = value;

            gameManager.SetPlayerNetworkData();
        }
    }

}
