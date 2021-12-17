using System;
using System.Collections.Generic;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ConsoleApp2
{
    class Asset2d
    {
        string Source = "D:/School/SMT5/Grafkom/Draw-Crossy-Road-CSharp-OpenTK-master";

        //verteks SEGITIGA DEFAULT
        float[] _vertices =
        {

        };

        uint[] _indices = //bikin nya dari vertex mana aja
        {

        };

        //DECLARE VAR==============================================
        int _vertexBufferObject;//buffer obj (handle variabel vertex spy bs di vgacard
        int _vertexArrayObject;//VAO  mengurus terkait array vertex yg kita kirim
        int _elementBufferObject;
        Shader _shader; //mengurus apa yg ditampilkan ke layar kita.
        int indeks = 0; //utk gambar lingkaran.
        int[] _pascal = { };


        public Asset2d(float[] vertices, uint[] indices)
        {
            _vertices = vertices;
            _indices = indices;
        }

        public void load()
        {
            //SETTINGAN BUFFER
            _vertexBufferObject = GL.GenBuffer(); //create buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject); //setting target dari buffer yang dituju
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            //SETTINGAN VAO
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            //SETINGAN CARA BACA BINARY
            //DEFAULT
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            //SETINGAN SETIAP VERTICES DENGAN COLOR (RGB)
            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);//posisi vertex
            //GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));//color vertex

            //Menyalakan var index[0] (layout location=0) yg ada pd shader.vert DEFAULT
            GL.EnableVertexAttribArray(0);

            //SETINGAN SHADER
            _shader = new Shader(Source + "/shaders/shader.vert",
                                 Source + "/shaders/shader.frag");
            _shader.Use();

            ////menyalakan WARNA dari WINDOW.CS di index ke 1 yg ada pd shader.vert (layout location=1) RGB
            //GL.EnableVertexAttribArray(1);


            if (_indices.Length != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            }

            //SETINGAN SHADER
            _shader = new Shader(Source + "/shaders/shader.vert",
                                 Source + "/shaders/shader.frag");
            _shader.Use();
        }

        public void render(int _lines)
        {
            //STEP menggambar sebuah objek
            //1 enable shader(default)
            _shader.Use();

            //MEWARNAI DI WINDOW.CS =============================
            //int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");//letak var
            //GL.Uniform4(vertexColorLocation, 0.0f, 0.0f, 1.0f, 1.0f); //SETTING WARNA
            //=====================================================

            //2 panggil bind VAO(default)
            GL.BindVertexArray(_vertexArrayObject);

            if (_indices.Length != 0)
            {
                //3.1 DRAW EBO ============ menggambar PERSEGI dari 2 SEGITIGA
                GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                if (_lines == 0) //gambar dengan isi DEFAULT
                {
                    GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
                }
                else if (_lines == 1) //gambar garis tpi gak sampai semua
                {
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, (_vertices.Length+1)/3);
                }
                else if (_lines == 2) // gambar tanpa isi
                {
                    GL.DrawArrays(PrimitiveType.LineLoop, 0, 3);
                }
                else if (_lines == 3)
                {
                    //lingkaran line
                    GL.DrawArrays(PrimitiveType.LineLoop, 0, (_vertices.Length+1)/3);

                    //lingkaran berisi
                    //GL.DrawArrays(PrimitiveType.TriangleFan, 0, (_vertices.Length + 1) / 3);
                }
                else if(_lines == 4)
                {
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, indeks);
                }
            }

        } //end of render

        public void createCircle(float center_x, float center_y, float radius)
        {
            _vertices = new float[1080];
            //loop mencari 360 titik
            for (int i = 0; i < 360; i++)
            {
                //titik i derajat dirubah jd radian (alpha), radian pi.180
                double degInRad = i * Math.PI / 180;
                //nyimpen x titik 1
                _vertices[i * 3] = (float)Math.Cos(degInRad) * radius + center_x;
                //nyimpen y titik 1
                _vertices[i * 3 + 1] = (float)Math.Sin(degInRad) * radius + center_y;
                //nyimpen z titik 1
                _vertices[i * 3 + 2] = 0;

                //vertex 1 start index 0-2
                //vertex 2 start index 3
            }
        }

        public void createElips(float center_x, float center_y, float radiusX,float radiusY)
        {
            _vertices = new float[1080];
            //loop mencari 360 titik
            for (int i = 0; i < 360; i++)
            {
                //titik i derajat dirubah jd radian (alpha), radian pi.180
                double degInRad = i * Math.PI / 180;
                //nyimpen x titik 1
                _vertices[i * 3] = (float)Math.Cos(degInRad) * radiusX + center_x;
                //nyimpen y titik 1
                _vertices[i * 3 + 1] = (float)Math.Sin(degInRad) * radiusY + center_y;
                //nyimpen z titik 1
                _vertices[i * 3 + 2] = 0;

                //vertex 1 start index 0-2
                //vertex 2 start index 3
            }
        }

        public void updateMousePosition(float _x, float _y, float _z)
        {
            //nyimpen x titik 1 
            _vertices[indeks * 3] = _x;
            //nyimpen y titik 1
            _vertices[indeks * 3 + 1] = _y;
            //nyimpen z titik 1
            _vertices[indeks * 3 + 2] = _z;
            indeks++;

            GL.BufferData(BufferTarget.ArrayBuffer, indeks * 3 * sizeof(float), _vertices, BufferUsageHint.StaticDraw);//kirim array vertex melalui buffer ke graphic card
            GL.BindVertexArray(_vertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
        }

        public List<float> CreateCurveBezier()
        {
            List<float> _vertices_bezier = new List<float>();
            List<int> pascal = getRow(indeks-1);
            _pascal = pascal.ToArray();

            for(float t=0; t<=1.0f; t += 0.01f) {
                Vector2 p = getP(indeks,t);
                _vertices_bezier.Add(p.X);
                _vertices_bezier.Add(p.Y);
                _vertices_bezier.Add(0);
            }

            return _vertices_bezier;
        }

        public Vector2 getP(int n, float t)
        {
            Vector2 p = new Vector2(0, 0);
            float[] k = new float[n];

            for (int i=0; i<n; i++)
            {
                k[i] = (float)Math.Pow((1 - t), n - 1 - i) * (float)Math.Pow(t, i) * _pascal[i];
            }

            for (int i = 0; i < n; i++)
            {
                p.X += k[i] * _vertices[i * 3];
                p.Y += k[i] * _vertices[i * 3 + 1];

            }

            return p;
        }

        public List<int> getRow (int rowIndex)
        {
            List<int> currow = new List<int>();

            //ellemen 1 pascal
            currow.Add(1);

            if(rowIndex == 0) {
                return currow;
            }
            List<int> prev = getRow(rowIndex - 1);
            //nambah elemen pascal di tengah
            for (int i = 1; i < prev.Count; i++) {
                int curr = prev[i - 1] + prev[i];
                currow.Add(curr);
            }

            //nambah elemen pascal di akhir
            currow.Add(1);


            return currow;
        }

        public void setVertices(float[] vertices)
        {
            _vertices = vertices;
        }

        public bool getVerticesLength()
        {
            if (_vertices[0] == 0)
            {
                return false;
            }
            if ((_vertices.Length+1)/3 > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
