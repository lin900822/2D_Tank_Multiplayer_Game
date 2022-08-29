using UnityEditor;
using UnityEngine;
using Fusion;

namespace Game.Core
{
    public enum InputButtons
    {
        FIRE
    }

    public struct NetworkInputData : INetworkInput
    {
        public NetworkButtons buttons;

        public float rotation;
        public Vector2 movementInput;
    }
}