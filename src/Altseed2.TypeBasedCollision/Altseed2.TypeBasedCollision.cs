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
        public abstract void CheckCollision<U>(Action<U> onCollided)
            where U : ICollisionMarker;

    }

    public static class CollisionStorage<U>
        where U : ICollisionMarker
    {
        internal static readonly HashSet<CollisionNode<U>> CollisionsHashSet = new HashSet<CollisionNode<U>>();

        public static IReadOnlyCollection<CollisionNode<U>> Collisions => CollisionsHashSet;
    }

    public sealed class CollisionNode<T> : CollisionNodeBase
        where T : ICollisionMarker
    {

        private bool AppliedTransform { get; set; }

        public T Key { get; private set; }

        private Collider Collider { get; set; }

        public CollisionNode(T key, Collider collider)
        {
            Key = key;
            Collider = collider;

            AppliedTransform = false;
        }

        public override void CheckCollision<TargetKey>(Action<TargetKey> onCollided)
        {
            if (onCollided is null || Collider is null) return;

            if (!AppliedTransform)
            {
                Collider.Transform = AbsoluteTransform;
                AppliedTransform = true;
            }

            foreach (var cn in CollisionStorage<TargetKey>.CollisionsHashSet)
            {
                if (!Equals(cn) && cn.IsUpdatedActually && cn.Collider is { })
                {
                    if (!cn.AppliedTransform)
                    {
                        cn.Collider.Transform = cn.AbsoluteTransform;
                        cn.AppliedTransform = true;
                    }

                    if (cn.Collider.GetIsCollidedWith(Collider))
                    {
                        onCollided(cn.Key);
                    }
                }
            }
        }

        protected override void OnAdded()
        {
            CollisionStorage<T>.CollisionsHashSet.Add(this);
        }

        protected override void OnUpdate()
        {
            AppliedTransform = false;
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();

            CollisionStorage<T>.CollisionsHashSet.Remove(this);
        }
    }
}
