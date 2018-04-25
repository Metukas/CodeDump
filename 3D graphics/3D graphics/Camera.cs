using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3D_graphics
{
    class Camera
    {
        public float X, Y, Z;

        public Camera(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Update(float dt, Keys key)
        {
            float s = dt * 0.1f;
            switch(key)
            {
                case Keys.A:
                    X -= s;
                    break;
                case Keys.D:
                    X += s;
                    break;
                case Keys.W:
                    Z += s;
                    break;
                case Keys.S:
                    Z -= s;
                    break;
                case Keys.Q:
                    Y -= s;
                    break;
                case Keys.E:
                    Y += s;
                    break;
            }

        }
    }
}
