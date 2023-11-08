using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TexDate
{
    internal class Loads : Game
    {
        public static GraphicsDevice graphicsDevice;
        public void OnLoad() {
            GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);
            graphicsDevice = graphics.GraphicsDevice;
        }
    }
}
