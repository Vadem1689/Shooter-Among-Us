using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class PlayerCanSeeObject : Conditional
    {
        public SharedGameObject target;

        public SharedFloat viewDistace;

        public override TaskStatus OnUpdate()
        {
            if ((target.Value.transform.position - transform.position).magnitude <= viewDistace.Value) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}