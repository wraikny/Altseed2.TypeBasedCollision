using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altseed2.TypeBasedCollision.Example
{
    internal class RectangleTargetNode : RectangleNode, ICollisionMarker
    {
        public string Label { get; private set; }

        public RectangleTargetNode(string label)
        {
            Label = label;

            RectangleSize = new Vector2F(100.0f, 100.0f);

            var collider = new RectangleCollider { Size = RectangleSize };

            var collisionNode = new CollisionNode<RectangleTargetNode>(this, collider);
            AddChildNode(collisionNode);
        }
    }
}
