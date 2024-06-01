using BRAmongUS.Utils;
using UnityEngine;

namespace BRAmongUS.Input
{
    public sealed class DesktopPlayerSight
    {
        private Vector3 mousePosition;
        
        public void SetPosition(ref Vector2 lookingDirection, in Vector3 input, in Transform sightTransform, in Transform playerTransform)
        {
            mousePosition = input;
            mousePosition.z = 10;

            sightTransform.position = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 position = playerTransform.position;
            position.z = 0;
            lookingDirection = (sightTransform.position - position).normalized;
        }
    }
}