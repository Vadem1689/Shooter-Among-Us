using System.Collections;
using System.Collections.Generic;
using BRAmongUS.Loot;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrototype;

    [SerializeField] private Transform[] playerSpawnPoints;

    [SerializeField] private GameObject[] players;

    [SerializeField] private AmmoBox[] lootBoxes;
    
    public List<Player> Spawn()
    {
        Debug.Log("Spawn initiated");

        var players = new List<Player>();

        for (var i = 0; i < playerSpawnPoints.Length; i++)
        {
            players.Add(SpawnPlayer(playerSpawnPoints[i].position));
        }

        return players;
    }
    
    private Player SpawnPlayer(in Vector3 position)
    {
        var player = Instantiate(playerPrototype, position, Quaternion.identity)
            .GetComponent<Player>();
        
        var randomName = GameController.Instance.GetRandomPlayerName();
        player.SetPlayerName(randomName);
        player.Init();

        return player;
    }

    public Player SpawnPlayer()
    {
        Vector3 spawnPoint = playerSpawnPoints[Random.Range(0, playerSpawnPoints.Length)].position;
        Player player = SpawnPlayer(spawnPoint);
        
        return player;
    }

    public AmmoBox GetRandomLootBox()
    {
        return lootBoxes[Random.Range(0,lootBoxes.Length)];
    }
}
