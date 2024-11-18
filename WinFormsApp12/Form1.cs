using OpenTK;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Platform.Windows;
using System;
using System.Windows.Forms;
using ClearBufferMask = OpenTK.Graphics.OpenGL.ClearBufferMask;
using EnableCap = OpenTK.Graphics.OpenGL.EnableCap;
using GL = OpenTK.Graphics.OpenGL.GL;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;

namespace WinFormsApp12
{
    public partial class Form1 : Form
    {
        private OpenTK.WinForms.GLControl glControl; // Declare glControl

        private float angle = 0.0f;

        public Form1()
        {
            InitializeComponent();
            glControl = new OpenTK.WinForms.GLControl(); // Initialize glControl
            InitializeOpenTK(glControl);
        }


        private WinForms.GLControl GetGlControl()
        {
            return glControl;
        }

        private void InitializeOpenTK(WinForms.GLControl glControl)
        {
            this.glControl = glControl; // Assign the parameter to the field
            glControl.Dock = DockStyle.Fill;
            glControl.Paint += GlControl_Paint;
            glControl.Resize += GlControl_Resize;
            glControl.Load += GlControl_Load;
            Controls.Add((Control)glControl); // Rzutowanie na System.Windows.Forms.Control
        }

        private void GlControl_Load(object? sender, EventArgs e)
        {
            GL.ClearColor(System.Drawing.Color.Black);
            GL.Enable(EnableCap.DepthTest);
        }

        private void GlControl_Resize(object? sender, EventArgs e)
        {
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, glControl.Width / (float)glControl.Height, 1.0f, 64.0f);
            GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        private void GlControl_Paint(object? sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 modelview = Matrix4.LookAt(Vector3.UnitZ * 5, Vector3.Zero, Vector3.UnitY);
            GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            GL.Rotate(angle, Vector3.UnitY);
            angle += 1.0f;

            GL.Color3(System.Drawing.Color.Green);
            Teapot(1.0, 10, true);

            glControl.SwapBuffers();
            glControl.Invalidate();
        }

        private void Teapot(double size, int detail, bool wireframe)
        {
            float[,] vertices = {
                { -0.5f, 0.0f, 0.5f },
                { 0.5f, 0.0f, 0.5f },
                { 0.5f, 0.0f, -0.5f },
                { -0.5f, 0.0f, -0.5f },
                { 0.0f, 1.0f, 0.0f }
            };

            int[,] indices = {
                { 0, 1, 4 },
                { 1, 2, 4 },
                { 2, 3, 4 },
                { 3, 0, 4 },
                { 0, 1, 2 },
                { 2, 3, 0 }
            };

            for (int i = 0; i < indices.GetLength(0); i++)
            {
                GL.Begin(PrimitiveType.Triangles);
                for (int j = 0; j < indices.GetLength(1); j++)
                {
                    int vertexIndex = indices[i, j];
                    GL.Vertex3(vertices[vertexIndex, 0] * size, vertices[vertexIndex, 1] * size, vertices[vertexIndex, 2] * size);
                }
                GL.End();
            }
        }
    }
}
