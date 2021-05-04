# Altseed2.TypeBasedCollision

```csharp
using Altseed2;
using Altseed2.TypeBasedCollision;

#ifdef NET5_0

// CollisionNode のキーとして使うためには ICollisionMarker インターフェースを実装する
record Key1(string Label) : ICollisionMarker;
record Key2(string Label) : ICollisionMarker;

#else

/* 省略 */

#endif

class MyNode<Key> : CircleNode
    where Key : ICollisionMarker
{
    CollisionNodeBase _collisionNode;

    public MyNode(Key key)
      : base()
    {
        Radius = 50.0f;
        VertNum = 32;
        
        // 円形コライダーを作成
        var collider = new CircleCollider {
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
        _collisionNode.CheckCollision<Key1>((c) => {
            Console.WriteLine($"{Label}: Hit with Key1({c.target.Label})");
        });
        
        // Key2 の CollisionNode との衝突判定を行う
        _collisionNode.CheckCollision<Key2>((c) => {
            Console.WriteLine($"{Label}: Hit with Key2({c.target.Label})");
        });
    }
}

```
