using System;
using UnityEngine;

namespace BRAmongUS.Coins
{
    public sealed class Coin : MonoBehaviour
    {
        public Transform SpawnPoint { get; private set; }
        
        public event Action<Coin> OnPicked;
        
        public void Init(in Transform tempSpawnPoint)
        {
            SpawnPoint = tempSpawnPoint;
        }
        
        public void Pick()
        {
            OnPicked?.Invoke(this);
        }
    }
}