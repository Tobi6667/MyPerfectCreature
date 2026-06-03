using System;
using UnityEngine;

namespace Game.Body
{

    public abstract class BodyPartBase : MonoBehaviour
    {
        public abstract void Initialize();
        public abstract void MoveToObject(Transform target, Action onReached,float speed = 4f, float arriveDistance = 0f);

       

    }
}
