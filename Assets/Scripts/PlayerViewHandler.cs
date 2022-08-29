using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.Core
{
    public class PlayerViewHandler : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer tankSpriteRenderer = null;
        [SerializeField] private SpriteRenderer barrelSpreiteRenderer = null;

        [SerializeField] private Transform canvasTransorm = null;

        [SerializeField] private Image hpBarImg = null;

        [SerializeField] private TMP_Text playerNameTxt = null;

        public Transform CanvasTransorm => canvasTransorm;

        public void SetTankColor(Color color)
        {
            tankSpriteRenderer.color = color;
        }

        public void SetBarrelColor(Color color)
        {
            barrelSpreiteRenderer.color = color;
        }

        public void SetHPBarValue(float ratio)
        {
            hpBarImg.fillAmount = ratio;
        }

        public void SetPlayerNameTxt(string name)
        {
            playerNameTxt.text = name;
        }
    }
}