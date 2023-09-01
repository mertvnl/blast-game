using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class CameraController : MonoBehaviour
    {
        private const float CAMERA_OFFSET = 0.5f;

        private void OnEnable()
        {
            GridManager.Instance.OnGridInitialized.AddListener(UpdateCameraPosition);
        }

        private void OnDisable()
        {
            GridManager.Instance.OnGridInitialized.RemoveListener(UpdateCameraPosition);
        }

        private void UpdateCameraPosition(int x, int y)
        {
            transform.position = new Vector3((float)x / 2 - CAMERA_OFFSET, (float)y / 2 - CAMERA_OFFSET, transform.position.z);
        }
    }
}
