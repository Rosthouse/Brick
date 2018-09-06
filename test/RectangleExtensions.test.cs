using Microsoft.Xna.Framework;
using NUnit.Framework;
using MonoRect = Microsoft.Xna.Framework.Rectangle;

namespace Brick.Test
{
    [TestFixture]
    class RectangleExtensionTest
    {


        [Test]
        public void ResizeRectangle()
        {
            MonoRect rect = new MonoRect(0, 0, 10, 10);
            MonoRect actual = rect.Scale(0.5f, 0.5f);
            Assert.That(actual.Size, Is.EqualTo(new Point(5, 5)));
        }
    }
}