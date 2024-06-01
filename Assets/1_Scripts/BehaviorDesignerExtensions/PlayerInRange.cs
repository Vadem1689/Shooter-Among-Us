using BRAmongUS;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Check Is Player In Range")]
    [TaskCategory("PlayerActions")]    

    public class PlayerInRange : Conditional
    {
        public SharedFloat searchDistance = 10f;

        public SharedGameObject foundedPlayer;
        
        private RaycastHit2D[] hits = new RaycastHit2D[20];
        
        private Transform cachedTransform;
        
        public override void OnStart()
        {
            cachedTransform = transform;
        }
        

        public override TaskStatus OnUpdate()
        {
            var closestPlayer = GameController.Instance.GetClosestPlayer(cachedTransform);

            if(closestPlayer == null) return TaskStatus.Failure;

            foundedPlayer.Value = closestPlayer.gameObject;

            if ((foundedPlayer.Value.transform.position - cachedTransform.position).magnitude < searchDistance.Value)
            {
                int hitsCount = Physics2D.LinecastNonAlloc(cachedTransform.position, foundedPlayer.Value.transform.position, hits);
                for (int i = 0; i < hitsCount; i++)
                {
                    if (hits[i].collider.CompareTag(Constants.Tags.Obstacle))
                    {
                        foundedPlayer.Value = null;
                        return TaskStatus.Failure;
                    }
                }

                return TaskStatus.Success; 
            }

            foundedPlayer.Value = null;
            return TaskStatus.Failure;
        }
    }
}
