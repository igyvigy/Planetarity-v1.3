using System;
using System.Collections;
using UnityEngine;

namespace Planetarity
{
    /// <summary>
    /// Behaviour responsible for controlling over the main camera: follow target planet, zoom in and out.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        public event Action<float> OnCameraZoomChanged = delegate { };

        [SerializeField]
        private float cameraDistanceMax = 300f;
        [SerializeField]
        private float cameraDistanceMin = 10f;
        [SerializeField]
        private float scrollSpeed = 5f;
        [SerializeField]
        private float moveSpeed = 1f;
        [SerializeField]

        private Transform target;
        private float cameraDistance;
        private bool isMovingToTarget = false;
        private float distanceToTarget = 0f;
        
        private void Start()
        {
            cameraDistance = transform.position.z * -1;
        }

        public void SetTarget(Transform target, bool animated)
        {
            if (this.target == target) return;
            this.target = target;
            if (animated)
            {
                isMovingToTarget = true;
                StartCoroutine(MoveToTarget());
            }
        }

        private void LateUpdate()
        {
            cameraDistance += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);
            OnCameraZoomChanged((1 - ((cameraDistance - cameraDistanceMin) / (cameraDistanceMax - cameraDistanceMin))));

            distanceToTarget = Vector3.Distance(transform.position, GetProjectedTargetPosition());

            if (isMovingToTarget)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, cameraDistance * -1);
            }
            else
            {
                if (target != null)
                {
                    transform.position = new Vector3(target.position.x, target.position.y, cameraDistance * -1);
                }
                else
                {
                    transform.position = new Vector3(0, 0, cameraDistance * -1);
                }
            }
        }

        private Vector3 GetProjectedTargetPosition()
        {
            return new Vector3(target.position.x, target.position.y, transform.position.z);
        }

        private IEnumerator MoveToTarget()
        {
            while (Vector3.Distance(transform.position, GetProjectedTargetPosition()) > 1)
            {
                Vector3 direction = (GetProjectedTargetPosition() - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                yield return null;
            }
            isMovingToTarget = false;
        }
    }
}