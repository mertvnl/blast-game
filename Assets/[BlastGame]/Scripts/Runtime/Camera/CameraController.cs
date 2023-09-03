using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class CameraController : MonoBehaviour
    {
        private Camera _camera;
        public Camera Camera => _camera == null ? _camera = GetComponent<Camera>() : _camera;

        private const float CAMERA_OFFSET = 0.5f;
        private const float MIN_CAMERA_SIZE = 6;
        private const float MAX_CAMERA_SIZE = 10;

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
            Camera.orthographicSize = Mathf.Clamp(new Vector2(x, y).magnitude, MIN_CAMERA_SIZE, MAX_CAMERA_SIZE);
            transform.position = new Vector3((float)x / 2 - CAMERA_OFFSET, (float)y / 2 - CAMERA_OFFSET, transform.position.z);
        }
    }
}
