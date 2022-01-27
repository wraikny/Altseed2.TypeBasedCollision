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
        protected internal bool AppliedTransform { get; private set; }
        protected internal Matrix44F ColliderTransform { get; private set; }
        protected internal Collider Collider { get; private set; }

        public CollisionNodeBase(Collider collider)
        {
            if (collider is null)
            {
                throw new ArgumentNullException(nameof(collider));
            }

            Collider = collider;
            ColliderTransform = collider.Transform;
            AppliedTransform = false;
        }

        protected internal void ApplyTransform()
        {
            if (!AppliedTransform)
            {
                Collider.Transform = ColliderTransform * AbsoluteTransform;
                AppliedTransform = true;
            }
        }

        public abstract IEnumerable<(TargetKey key, bool isCollided)> EnumerateCollisions<TargetKey>()
            where TargetKey : ICollisionMarker;

        public void CheckCollision<TargetKey>(Action<TargetKey> actionWhenCollided)
            where TargetKey : ICollisionMarker
        {
            if (actionWhenCollided is null)
            {
                throw new ArgumentNullException(nameof(actionWhenCollided));
            }

            foreach (var (key, isCollided) in EnumerateCollisions<TargetKey>())
            {
                if (isCollided) actionWhenCollided(key);
            }
        }

        public void CheckCollision<TargetKey>(Action<TargetKey, bool> action)
            where TargetKey : ICollisionMarker
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var (key, isCollided) in EnumerateCollisions<TargetKey>())
            {
                action(key, isCollided);
            }
        }

        protected override void OnUpdate()
        {
            AppliedTransform = false;
        }
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
        public T Key { get; private set; }

        public CollisionNode(T key, Collider collider)
            : base(collider)
        {
            Key = key;
        }

        public override IEnumerable<(TargetKey key, bool isCollided)> EnumerateCollisions<TargetKey>()
        {
            ApplyTransform();

            foreach (var cn in CollisionStorage<TargetKey>.CollisionsHashSet)
            {
                if (!Equals(cn) && cn.IsUpdatedActually && cn.Collider is { })
                {
                    cn.ApplyTransform();

                    yield return (cn.Key, cn.Collider.GetIsCollidedWith(Collider));
                }
            }
        }

        protected override void OnAdded()
        {
            CollisionStorage<T>.CollisionsHashSet.Add(this);
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();

            CollisionStorage<T>.CollisionsHashSet.Remove(this);
        }
    }
}
