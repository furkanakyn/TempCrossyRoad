using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public Transform player;
    Vector3 velocity;
    public float smoothTime;
    public bool isCameraFollowingBackWards;

    private void Update()
    {

        if (transform.position.z < player.position.z || isCameraFollowingBackWards)
        {
            var targetPos = player.position;
            targetPos.x = transform.position.x;
            targetPos.y = 0;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
        if (Mathf.RoundToInt((transform.position - player.transform.position).magnitude) == 0)
        {
            isCameraFollowingBackWards = false;
        }
    }
    public void ResetCameraHolder()
    {
        isCameraFollowingBackWards = true;
    }
}
