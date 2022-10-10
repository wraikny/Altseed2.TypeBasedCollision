using System;
using System.Linq;
using System.Reflection;
using System.Threading;

using Altseed2;
using Altseed2.TypeBasedCollision;

using NUnit.Framework;

namespace Test;

public class Collision
{
    internal sealed record Key1() : ICollisionMarker { }
    internal sealed record Key2() : ICollisionMarker { }
    internal sealed record Key3() : ICollisionMarker { }

    private static CollisionNode<T> CreateCircleCollision<T>(T key, float radius)
        where T : ICollisionMarker
    {
        var collider = new CircleCollider { Radius = radius };
        return new CollisionNode<T>(key, collider);
    }

    private static void Test<T1, T2>(CollisionNode<T1> collision, int expected)
        where T1 : ICollisionMarker
        where T2 : ICollisionMarker
    {
        var count = collision.EnumerateCollisions<T2>().Count();

        Assert.AreEqual(count, expected, $"{typeof(T1)} to {typeof(T2)}");
    }

    private CollisionNode<Key1>[] collisions1;
    private CollisionNode<Key2>[] collisions2;

    [SetUp, Apartment(ApartmentState.STA)]
    public void Setup()
    {
        var config = new Configuration { EnabledCoreModules = CoreModules.None };
        Engine.Initialize("Test", 1, 1, config);

        collisions1 = Enumerable
            .Range(1, 7)
            .Select(_ => CreateCircleCollision(new Key1(), 50f))
            .ToArray();

        collisions2 = Enumerable
            .Range(1, 5)
            .Select(_ => CreateCircleCollision(new Key2(), 50f))
            .ToArray();

        Array.ForEach(collisions1, Engine.AddNode);
        Array.ForEach(collisions2, Engine.AddNode);

        var updateComponents = typeof(Engine)
            .GetMethod("UpdateComponents", BindingFlags.NonPublic | BindingFlags.Static)
            .CreateDelegate<Func<bool, bool, bool>>();

        Engine.DoEvents();
        updateComponents(true, false);

        Engine.DoEvents();
    }

    [Test, Apartment(ApartmentState.STA)]
    public void Test1to1()
    {
        Test<Key1, Key1>(collisions1[0], collisions1.Length - 1);
    }

    [Test, Apartment(ApartmentState.STA)]
    public void Test1to2()
    {
        Test<Key1, Key2>(collisions1[0], collisions2.Length);
    }

    [Test, Apartment(ApartmentState.STA)]
    public void Test2to1()
    {
        Test<Key2, Key1>(collisions2[0], collisions1.Length);
    }

    [Test, Apartment(ApartmentState.STA)]
    public void Test2to2()
    {
        Test<Key2, Key2>(collisions2[0], collisions2.Length - 1);
    }

    [TearDown, Apartment(ApartmentState.STA)]
    public void TearDown()
    {
        Engine.Terminate();
    }
}
