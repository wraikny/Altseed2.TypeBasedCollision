using System;

namespace Altseed2.TypeBasedCollision
{
    internal interface IHasLabel
    {
        string Label { get; }
    }

    // CollisionNode のキーとして使うためには ICollisionMarker インターフェースを実装する
#if NET5_0

    internal record Key1(string Label) : ICollisionMarker, IHasLabel;
    internal record Key2(string Label) : ICollisionMarker, IHasLabel;

#else

    sealed class Key1 : ICollisionMarker, IHasLabel
    {
        public string Label { get; private set; }

        public Key1(string label)
        {
            Label = label;
        }
    }

    sealed class Key2 : ICollisionMarker, IHasLabel
    {
        public string Label { get; private set; }

        public Key2(string label)
        {
            Label = label;
        }
    }

#endif

    internal class MyNode<Key> : CircleNode
        where Key : ICollisionMarker, IHasLabel
    {
        private readonly Key key;
        private readonly CollisionNode<Key> _collisionNode;

        public MyNode(Key key)
        : base()
        {
            Radius = 50.0f;
            VertNum = 32;

            this.key = key;

            // 円形コライダーを作成
            var collider = new CircleCollider
            {
                Radius = Radius,
            };

            // CollisionNodeを作成する。
            _collisionNode = new CollisionNode<Key>(key, collider);

            // CollisionNodeを子ノードとして追加する。
            AddChildNode(_collisionNode);
        }

        protected override void OnUpdate()
        {
            /*
                CollisionNode<T>.CheckCollision<U> を利用して衝突判定処理を行う。
                衝突対象のキーをジェネリックで指定する（自身とは衝突しない）
                継承したクラスなどは対象にならず、完全にキーの型が一致するものとのみ衝突判定が行われる。
                衝突した際に実行する処理を Action として渡す
            */

            // Key1 の CollisionNode との衝突判定を行う
            _collisionNode.CheckCollision<Key1>((k) =>
            {
                Console.WriteLine($"{key.Label}: Hit with Key1({k.Label})");
            });
        }
    }
}
