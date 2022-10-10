using System;

namespace Altseed2.TypeBasedCollision.Example
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] _)
        {
            Engine.Initialize("Collision", 800, 600);

            Engine.AddNode(new MouseNode());
            Engine.AddNode(new CircleTargetNode("1") { Position = new Vector2F(50.0f, 50.0f) });
            Engine.AddNode(new CircleTargetNode("2") { Position = new Vector2F(250.0f, 50.0f) });
            Engine.AddNode(new RectangleTargetNode("3") { Position = new Vector2F(200.0f, 200.0f) });
            Engine.AddNode(new RectangleTargetNode("4") { Position = new Vector2F(300.0f, 400.0f) });

            while (Engine.DoEvents())
            {
                Engine.Update();
            }

            Engine.Terminate();
        }
    }
}
