using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3D_graphics
{
    public partial class Form1 : Form
    {
        //System.Drawing.Graphics formGraphics;
        Camera camera;

        public Form1()
        {
            InitializeComponent();
            camera = new Camera(0, 0, -5);
            this.ClientSizeChanged += OnSizeChanged;
            KeyDown += UpdateCameraPosition;
        }

        private void UpdateCameraPosition(object sender, KeyEventArgs e)
        {
            camera.Update(1, e.KeyCode);
            Draw3D();
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            Draw3D();
        }

        private void DrawIt()
        {
            var formGraphics = this.CreateGraphics();
            formGraphics.Clear(Color.White);
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
            formGraphics.DrawEllipse(myPen, 0, 0, 200, 200);
            myPen.Dispose();
            formGraphics.Dispose();
        }

        private void Draw3D()
        {
            ValueTuple<int, int, int>[] vertices = { (-1, -1, -1), (1, -1, -1), (1, 1, -1), (-1, 1, -1), (-1, -1, 1), (1, -1, 1), (1, 1, 1), (-1, 1, 1) };
            ValueTuple<int, int>[] edges = { (0, 1), (1, 2), (2, 3), (3, 0), (4, 5), (5, 6), (6, 7), (7, 4), (0, 4), (1, 5), (2, 6), (3, 7) };
            int thickness = 3;

            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            formGraphics.Clear(Color.White);

            int centerOfScreenX = ClientRectangle.Width / 2;
            int centerOfScreenY = ClientRectangle.Height / 2;
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(System.Drawing.Color.Red, thickness);

            //foreach(var (x,y,z) in vertices)
            //{
            //    int zz = z + 5;
            //
            //    int f = 200 / zz;
            //
            //    int xx = x * f;
            //    int yy = y * f;
            //
            //    formGraphics.DrawEllipse(myPen, centerOfScreenX + xx, centerOfScreenY + yy, thickness, thickness);
            //}

            foreach (var edge in edges)
            {
                var first = vertices[edge.Item1];
                var second = vertices[edge.Item2];
                List<PointF> pointList = new List<PointF>();
                ValueTuple<float, float, float>[] temp = new ValueTuple<float, float, float>[] {first, second};
                foreach (var (x, y, z) in temp)
                {
                    float xx = x;
                    float yy = y;
                    float zz = z;
                    //int zz = z + 5;

                    xx = x - camera.X;
                    yy = y - camera.Y;
                    zz = z - camera.Z;

                    float f = 200 / zz;
                    xx = xx * f;
                    yy = yy * f;
                    pointList.Add(new PointF(centerOfScreenX + xx, centerOfScreenY + yy));
                }
                formGraphics.DrawLine(myPen, pointList[0], pointList[1]);
                
            }
        }
        void ClearScreen()
        {
            //formGraphics.Clear(Color.White);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Draw3D();
        }
    }
}

