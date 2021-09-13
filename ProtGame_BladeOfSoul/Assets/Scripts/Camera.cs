using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float smoothing;

    [SerializeField] private Vector2 maxPosition;
    [SerializeField] private Vector2 minPosition;


    private void LateUpdate()
    {
        if (transform.position != target.position)
        {
            Vector2 targetPosition = new Vector2(target.position.x, target.position.y);

            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
            transform.position = Vector2.Lerp(transform.position, targetPosition, smoothing);
        }
    }
}
