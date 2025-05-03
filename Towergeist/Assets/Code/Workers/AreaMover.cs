using AStarPathFinding.Path;
using System;
using System.Collections.Generic;
using AStarPathFinding.Nodes;
using UnityEngine;

namespace Movement
{
    // [RequireComponent(typeof(AStarPathfinder))]
    public class AreaMover : MonoBehaviour
    {
        #region Areas' Enum
        public enum Destination
        {
            WorkingArea,
            GatheringArea,
            ChattingArea,
            RestingArea
        }
        #endregion

        #region Variables

        [Header("Area Transforms")]
        public Transform workingArea;
        public Transform gatheringArea;
        public Transform chattingArea;
        public Transform restingArea;

        [Header("Movement Settings")]
        [Tooltip("Reference to  A star pathfinder.")]
        public AStarPathfinder pathfinder;
        [Tooltip("Movement speed in seconds")]
        public float moveSpeed = 3f;

        private List<Node> _path;
        private int _pathIndex;
        private bool _isMoving;

        /// <summary>Fired once whenever the agent arrives at their destination.</summary>
        public event Action OnArrived;
        #endregion

        void Awake()
        {
            pathfinder = pathfinder ?? GetComponent<AStarPathfinder>();
        }

        void Update()
        {
            if (!_isMoving || _path == null || _pathIndex >= _path.Count) return;

            Vector3 next = _path[_pathIndex].worldPosition;
            transform.position = Vector3.MoveTowards(transform.position, next, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, next) < 0.1f)
            {
                _pathIndex++;
                if (_pathIndex >= _path.Count)
                {
                    _isMoving = false;
                    OnArrived?.Invoke();
                }
            }
        }

        public void MoveTo(Destination dest)
        {
            Vector3 targetPos = dest switch
            {
                Destination.WorkingArea => workingArea.position,
                Destination.GatheringArea => gatheringArea.position,
                Destination.ChattingArea => chattingArea.position,
                Destination.RestingArea => restingArea.position,
                _ => transform.position
            };

            _path = pathfinder.FindPath(transform.position, targetPos);
            _pathIndex = 0;
            _isMoving = _path != null && _path.Count > 0;
            if (_isMoving)
            {
                Debug.Log($"{name}: Moving to {dest} at {targetPos}");
            }
            else
            {
                Debug.LogWarning($"{name}: No path found to {dest}");
            }
        }
    }
}