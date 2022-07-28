using UnityEngine;

namespace GameCamera
{
    public class SnapCameraMixin : CameraMixin
    {
        private const float LERP_SPEED = 0.1f;

        private float lerpTime;
    
        private void Update()
        {
            lerpTime = Time.deltaTime / LERP_SPEED;

            var targetPosition = new Vector3(Mathf.Round(transform.localPosition.x), Mathf.Round(transform.localPosition.y), Mathf.Round(transform.localPosition.z));
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, lerpTime);
        }
    }
}

