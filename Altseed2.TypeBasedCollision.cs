/*

---Altseed2.TypeBasedCollision---

MIT License

Copyright (c) 2021 wraikny

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using System;
using System.Collections.Generic;

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
            public static HashSet<CollisionNode<U>> Collisions;
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

        public override void CheckCollision<TargetKey>(Action<CollisionNode<TargetKey>> onCollided)
        {
            if (onCollided is null || Collider is null || CollisionStorage<TargetKey>.Collisions is null) return;

            if (!AppliedTransform)
            {
                Collider.Transform = AbsoluteTransform;
                AppliedTransform = true;
            }

            foreach (var cn in CollisionStorage<TargetKey>.Collisions)
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
            CollisionStorage<T>.Collisions ??= new HashSet<CollisionNode<T>>();
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