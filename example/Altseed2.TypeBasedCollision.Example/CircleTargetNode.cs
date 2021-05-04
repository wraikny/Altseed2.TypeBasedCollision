using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altseed2.TypeBasedCollision.Example
{
    internal class CircleTargetNode : CircleNode, ICollisionMarker
    {
        public string Label { get; private set; }

        public CircleTargetNode(string label)
        {
            Label = label;

            Radius = 50.0f;
            VertNum = 32;

            var collider = new CircleCollider { Radius = Radius };

            var collisionNode = new CollisionNode<CircleTargetNode>(this, collider);
            AddChildNode(collisionNode);
        }
    }
}
