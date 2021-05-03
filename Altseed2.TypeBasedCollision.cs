using System;
using System.Collections.Generic;

using Altseed2;

namespace Altseed2.TypeBasedCollision
{
    public interface ICollisionMarker { }

    public abstract class CollisionNodeBase : TransformNode
    {
        public abstract void CheckCollision<U>(Action<CollisionNode<U>> onCollided)
            where U : ICollisionMarker;
    }

    public sealed class CollisionNode<T> : CollisionNodeBase
        where T : ICollisionMarker
    {
        private static class CollisionStorage<U>
            where U : ICollisionMarker
        {
            public static readonly HashSet<CollisionNode<U>> Collisions = new HashSet<CollisionNode<U>>();
        }

        private bool AppliedTransform { get; set; }

        public T Value { get; private set; }

        private Collider Collider { get; set; }

        public CollisionNode(T v, Collider collider)
        {
            Value = v;
            Collider = collider;

            AppliedTransform = false;
        }

        public override void CheckCollision<U>(Action<CollisionNode<U>> onCollided)
        {
            if (onCollided is null || Collider is null) return;

            if (!AppliedTransform)
            {
                Collider.Transform = AbsoluteTransform;
                AppliedTransform = true;
            }

            foreach (var cn in CollisionStorage<U>.Collisions)
            {
                if (!this.Equals(cn) && cn.IsUpdatedActually && cn.Collider is { })
                {
                    if (!cn.AppliedTransform)
                    {
                        cn.Collider.Transform = cn.AbsoluteTransform;
                        cn.AppliedTransform = true;
                    }
                    
                    if (cn.Collider.GetIsCollidedWith(Collider))
                    {
                        onCollided(cn);
                    }
                }
            }
        }

        protected override void OnAdded()
        {
            CollisionStorage<T>.Collisions.Add(this);
        }

        protected override void OnUpdate()
        {
            AppliedTransform = false;
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();

            CollisionStorage<T>.Collisions.Remove(this);
        }

        private bool FindAncestorStatus(RegisteredStatus status)
        {
            Node current = this;
            while (current != null)
            {
                if (current.Status == status) return true;
                current = current.Parent;
            }
            return false;
        }
    }
}