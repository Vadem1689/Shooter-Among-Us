using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("ShootTargetPlayer")]
    [TaskCategory("PlayerActions")]

    public class ShootPlayer : Action
    {
        public SharedGameObject targetPlayer;

        private Player player;

        public override void  OnStart() {
            player = GetComponent<Player>();
        }

        public override TaskStatus OnUpdate()
        {
            if (targetPlayer.Value != null && player != null) {
                player.Fire(targetPlayer.Value.transform.position);
                player.Look(targetPlayer.Value.transform.position);
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }

    }
}
