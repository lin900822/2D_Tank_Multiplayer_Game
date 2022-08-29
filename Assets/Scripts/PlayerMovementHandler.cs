using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class PlayerMovementHandler : MonoBehaviour
    {
        [SerializeField] private NetworkRigidbody2D playerNetworkRigidbody = null;

        [SerializeField] private float moveSpeed = 5f;

        public void Move(NetworkInputData data)
        {
            Vector2 moveVector = data.movementInput.normalized;
            playerNetworkRigidbody.Rigidbody.velocity = moveVector * moveSpeed;
        }

        public void SetRotation(float rotation)
        {
            playerNetworkRigidbody.Rigidbody.rotation = rotation;
        }
    }
}