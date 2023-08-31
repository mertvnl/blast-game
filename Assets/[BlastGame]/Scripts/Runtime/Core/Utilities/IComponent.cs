using UnityEngine;

namespace Core.Utilities
{
    public interface IComponent
    {
        public Transform transform { get; }
        public GameObject gameObject { get; }
    }
}