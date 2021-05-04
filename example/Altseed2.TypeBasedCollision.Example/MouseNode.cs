using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altseed2.TypeBasedCollision.Example
{
    internal class MouseNode : CircleNode, ICollisionMarker
    {
        private int count = 0;
        private readonly CollisionNode<MouseNode> _collisionNode;

        public MouseNode()
        {
            Radius = 10.0f;
            VertNum = 32;

            // 円形コライダーを作成
            var collider = new CircleCollider
            {
                Radius = Radius,
            };

            // CollisionNodeを作成する。
            _collisionNode = new CollisionNode<MouseNode>(this, collider);

            // CollisionNodeを子ノードとして追加する。
            AddChildNode(_collisionNode);
        }

        protected override void OnUpdate()
        {
            Position = Engine.Mouse.Position;

            /*
                CollisionNode<T>.CheckCollision<U> を利用して衝突判定処理を行う。
                衝突対象のキーをジェネリックで指定する（自身とは衝突しない）
                継承したクラスなどは対象にならず、完全にキーの型が一致するものとのみ衝突判定が行われる。
                衝突した際に実行する処理を Action として渡す
            */

            _collisionNode.CheckCollision<CircleTargetNode>((target) =>
            {
                Console.WriteLine($"[{count}] MouseNode hits CircleTargetNode({target.Label})");
            });

            _collisionNode.CheckCollision<RectangleTargetNode>((target) =>
            {
                Console.WriteLine($"[{count}] MouseNode hits RectangleTargetNode({target.Label})");
            });

            count++;
        }
    }
}
