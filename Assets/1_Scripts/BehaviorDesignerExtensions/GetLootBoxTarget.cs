using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Get LootBox target")]
    [TaskCategory("PlayerActions")]

    public class GetLootBoxTarget : Action
    {
        public SharedGameObject lootBoxTarget;

        public override TaskStatus OnUpdate()
        {
            lootBoxTarget.Value = GameController.Instance.GetRandomLootBox().gameObject;
            //Debug.Log("LootBox selected by " + GetComponent<Player>().GetPlayerName() + ":" + lootBoxTarget.Value);
            return TaskStatus.Success;
        }
    }

}
