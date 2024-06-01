using System.Collections.Generic;
using UnityEngine;

namespace BRAmongUS.Coins
{
    public sealed class CoinsSpawner : MonoBehaviour
    {
        [SerializeField] private Coin coinPrefab;
        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField] private int maxCoinsAmount = 10;
        
        private Transform selectedSpawnPoint;
        private Transform previousSpawnPoint;
        
        private void Start()
        {
            for (int i = 0; i < maxCoinsAmount; i++)
            {
                SpawnCoin();
            }
        }
        
        private void OnCoinPicked(Coin coin)
        {
            previousSpawnPoint = coin.SpawnPoint; 
            selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            spawnPoints.Remove(selectedSpawnPoint);
            
            coin.transform.position = selectedSpawnPoint.position;
            coin.Init(selectedSpawnPoint);
            spawnPoints.Add(previousSpawnPoint);
        }
        
        private void SpawnCoin()
        {
            selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Coin spawnedCoin = Instantiate(coinPrefab, selectedSpawnPoint.position, Quaternion.identity);
            spawnedCoin.Init(selectedSpawnPoint);
            spawnedCoin.OnPicked += OnCoinPicked;
            
            spawnPoints.Remove(selectedSpawnPoint);
        }
    }
}