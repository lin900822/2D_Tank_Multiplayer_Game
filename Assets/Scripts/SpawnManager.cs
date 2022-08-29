using System.Collections;
using UnityEngine;

namespace Game.Core
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints = null;

        public Transform GetRandomSpawnPoint()
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);

            return spawnPoints[randomIndex];
        }
    }
}