using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Open target LootBox")]
    [TaskCategory("PlayerActions")]

    public class OpenLootBox : Action
    {
        private Player player;

        public override void OnAwake()
        {
            player = GetComponent<Player>();

        }

        public override TaskStatus OnUpdate()
        {
            Debug.Log("Пробуем");
            if (player.CanOpenAmmoBox()) {

                Debug.Log("Gjkex");
                player.SwitchGunFromBox();
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
