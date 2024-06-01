using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{

    [TaskDescription("Seek the target specified using Player component.")]
    [TaskCategory("PlayerActions")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]

    public class PlayerSeek : Action
    {
        public SharedGameObject target;
        public SharedFloat arrivedDistance = 1f;
        private Player player;

        private bool targetSet = false;

        public override void OnStart()
        {
            player = GetComponent<Player>();
            targetSet = false;
        }

        public override TaskStatus OnUpdate()
        {

            if (target.Value == null)
            {
                //Debug.Log("Task Failure returning "+ target.Value);
                return TaskStatus.Failure;
            }

            //Debug.Log("Before hasarrived "+target.Value);

            if (HasArrived())
            {
                return TaskStatus.Success;
            }

            if (!targetSet && player != null)
            {
                //Debug.Log("Target Set");
                player.SetMovementTarget(target.Value.transform);
                targetSet = true;
            }

            return TaskStatus.Running;
        }



        private bool HasArrived()
        {
            return ((target.Value.transform.position - transform.position).magnitude <= arrivedDistance.Value);
        }


    }
}