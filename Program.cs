using System;

using Altseed2;

namespace Altseed2.TypeBasedCollision
{
    // CollisionNode のキーとして使うためには ICollisionMarker を実装する必要がある
    class Node1 : CircleNode, ICollisionMarker
    {
        private string Label { get; set;}
        CollisionNodeBase _collisionNode;
        public Node1(string label)
        {
            Label = label;

            Radius = 50.0f;
            VertNum = 32;

            var collider = new CircleCollider {
                Radius = Radius,
            };

            // キーとなる型を指定する
            // ここではNode1を指定して、その値として this を渡している
            _collisionNode = new CollisionNode<Node1>(this, collider);
            AddChildNode(_collisionNode);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            // 衝突対象のキーをジェネリックで指定する（自身とは衝突しない）
            // 衝突した際に実行する処理を Action として渡す
            _collisionNode.CheckCollision<Node1>((c) => {
                var target = c.Value;
                Console.WriteLine(
                    $"[{Program.Frame:000000}] {Label}{Position}: Hit with {target.Label} {target.Position}"
                );
            });
        }
    }

    class Program
    {
        private static int s_frame;
        public static int Frame => s_frame;

        static void Main(string[] args)
        {
            Engine.Initialize("Collision", 800, 600);

            var n1 = new Node1("1");

            Engine.AddNode(n1);
            Engine.AddNode(new Node1("2") { Position = new Vector2F(50.0f, 50.0f) });
            Engine.AddNode(new Node1("3") { Position = new Vector2F(200.0f, 200.0f) });

            while (Engine.DoEvents())
            {
                n1.Position = Engine.Mouse.Position;

                Engine.Update();
                s_frame++;
            }

            Engine.Terminate();
        }
    }
}
