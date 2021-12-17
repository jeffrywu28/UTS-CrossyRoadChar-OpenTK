using System;
using System.Collections.Generic;
using System.IO;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ConsoleApp2
{
    internal class robot : Asset3d
    {



        string Source = "D:/School/SMT5/Grafkom/Draw-Crossy-Road-CSharp-OpenTK-master+complete";

        List<Vector3> _vertices = new List<Vector3>();
        List<Vector3> _textureVertices = new List<Vector3>();
        List<Vector3> _normals = new List<Vector3>();

        List<uint> _indices = new List<uint>();
        String color;
        int _vertexBufferObject;//buffer obj (handle variabel vertex spy bs di vgacard
        int _vertexArrayObject;//VAO  mengurus terkait array vertex yg kita kirim
        int _elementBufferObject;
        Shader _shader; //mengurus apa yg ditampilkan ke layar kita.
        int indeks = 0; //utk gambar lingkaran.
        int[] _pascal = { };

        Matrix4 transform = Matrix4.Identity;

        public Vector3 _centerPosition = new Vector3(0, 0, 0);
        public List<Vector3> _euler = new List<Vector3>();

        Matrix4 _view;
        Matrix4 _projection;
        public List<robot> Child = new List<robot>();//biar bisa pny child masing2

        public robot(String color = "")
        {
            this.color = color;
            _euler.Add(new Vector3(1, 0, 0));
            _euler.Add(new Vector3(0, 1, 0));
            _euler.Add(new Vector3(0, 0, 1));
        }

        public void load(int size_x, int size_y)
        {
            //SETTINGAN BUFFER
            _vertexBufferObject = GL.GenBuffer(); //create buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject); //setting target dari buffer yang dituju
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * Vector3.SizeInBytes, _vertices.ToArray(), BufferUsageHint.StaticDraw);

            //SETTINGAN VAO
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            //SETINGAN CARA BACA BINARY
            //DEFAULT
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            //Menyalakan var index[0] (layout location=0) yg ada pd shader.vert DEFAULT
            GL.EnableVertexAttribArray(0);

            ////menyalakan WARNA dari WINDOW.CS di index ke 1 yg ada pd shader.vert (layout location=1) RGB
            //GL.EnableVertexAttribArray(1);


            if (_indices.Count != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Count * sizeof(uint), _indices.ToArray(), BufferUsageHint.StaticDraw);
            }

            //SETINGAN SHADER
            _shader = new Shader(Source + "/shaders/shader.vert",
                                 Source + "/shaders/shader" + color + ".frag");
            _shader.Use();

            //_shader = new Shader("D:/grafkom/C#/ConsoleApp2/shaders/shader.vert",
            //                     "D:/grafkom/C#/ConsoleApp2/shaders/shader1.frag");
            //_shader.Use();

            //liat dari titik mana
            _view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            //pandangan- berapa derajat
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), size_x / (float)size_y, 0.1f, 100.0f);

            foreach (var item in Child)
            { //load semua anak
                item.load(size_x, size_y);
            }
        }

        public void render(int _lines)
        {
            //STEP menggambar sebuah objek
            //1 enable shader(default)

            //geser ke (kanan,atas)
            //transform = transform * Matrix4.CreateTranslation(0.01f, 0.0f, 0.0f);
            //membesar
            //transform = transform * Matrix4.CreateScale(1.001f);
            //berputar di sumbu Z
            //transform = transform * Matrix4.CreateRotationZ(0.01f);
            _shader.Use();

            _shader.SetMatrix4("transform", transform);
            _shader.SetMatrix4("view", _view);
            _shader.SetMatrix4("projection", _projection);

            //MEWARNAI DI WINDOW.CS =============================
            //int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");//letak var
            //GL.Uniform4(vertexColorLocation, 0.0f, 0.0f, 1.0f, 1.0f); //SETTING WARNA
            //=====================================================

            //2 panggil bind VAO(default)
            GL.BindVertexArray(_vertexArrayObject);

            if (_indices.Count != 0)
            {
                //3.1 DRAW EBO ============ menggambar PERSEGI dari 2 SEGITIGA
                GL.DrawElements(PrimitiveType.Triangles, _indices.Count, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                if (_lines == 0) //gambar dengan isi DEFAULT
                {
                    GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Count);
                }
                else if (_lines == 1) //gambar garis tpi gak sampai semua
                {
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, _vertices.Count);
                }
                else if (_lines == 2) // gambar tanpa isi
                {
                    GL.DrawArrays(PrimitiveType.LineLoop, 0, _vertices.Count);
                }
                else if (_lines == 3)
                {
                    //lingkaran line
                    //GL.DrawArrays(PrimitiveType.LineLoop, 0, _vertices.Count);

                    //lingkaran berisi
                    GL.DrawArrays(PrimitiveType.TriangleFan, 0, _vertices.Count);
                }
            }

            foreach (var item in Child)
            {
                item.render(_lines);
            }

        } //end of render

        public void setVertices(List<Vector3> vertices)
        {
            _vertices = vertices;
        }

        public bool getVerticesLength()
        {
            if (_vertices.Count == 0)
            {
                return false;
            }
            if (_vertices.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void createBoxVertices(float x, float y, float z, float length)//ttik pusat dari box dimana
        {//length panjang dari titik kubus

            _centerPosition.X = x; //jgn lupa selalu tambahkan ini
            _centerPosition.Y = y;
            _centerPosition.Z = z;

            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);

            //TITIK 5
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);

            _indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
        }
        public void createBodyRobot(float x, float y, float z, float length, String m_color)//ttik pusat dari box dimana
        {//length panjang dari titik kubus

            _centerPosition.X = x; //jgn lupa selalu tambahkan ini
            _centerPosition.Y = y;
            _centerPosition.Z = z;

            this.color = m_color;

            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - length / 1.75f;
            temp_vector.Y = y + length / 1.5f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / 1.75f;
            temp_vector.Y = y + length / 1.5f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - length / 1.75f;
            temp_vector.Y = y - length / 1.5f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + length / 1.75f;
            temp_vector.Y = y - length / 1.5f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);

            //TITIK 5
            temp_vector.X = x - length / 1.75f;
            temp_vector.Y = y + length / 1.5f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / 1.75f;
            temp_vector.Y = y + length / 1.5f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - length / 1.75f;
            temp_vector.Y = y - length / 1.5f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + length / 1.75f;
            temp_vector.Y = y - length / 1.5f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);


            _indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
        }

        public void creatlongbox(float x, float y, float z, float length, String m_color)//ttik pusat dari box dimana
        {//length panjang dari titik kubus

            _centerPosition.X = x; //jgn lupa selalu tambahkan ini
            _centerPosition.Y = y;
            _centerPosition.Z = z;

            this.color = m_color;

            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - length / 2.5f;
            temp_vector.Y = y + length / 1.5f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / 2.5f;
            temp_vector.Y = y + length / 1.5f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - length / 2.5f;
            temp_vector.Y = y - length / 1.5f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + length / 2.5f;
            temp_vector.Y = y - length / 1.5f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);

            //TITIK 5
            temp_vector.X = x - length / 2.5f;
            temp_vector.Y = y + length / 1.5f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / 2.5f;
            temp_vector.Y = y + length / 1.5f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - length / 2.5f;
            temp_vector.Y = y - length / 1.5f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + length / 2.5f;
            temp_vector.Y = y - length / 1.5f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);


            _indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
        }

        public void creatflatlongbox(float x, float y, float z, float length, String m_color)//ttik pusat dari box dimana
        {//length panjang dari titik kubus

            _centerPosition.X = x; //jgn lupa selalu tambahkan ini
            _centerPosition.Y = y;
            _centerPosition.Z = z;

            this.color = m_color;

            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - length / 1.0f;
            temp_vector.Y = y + length / 2.5f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / 1.0f;
            temp_vector.Y = y + length / 2.5f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - length / 1.0f;
            temp_vector.Y = y - length / 2.5f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + length / 1.0f;
            temp_vector.Y = y - length / 2.5f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);

            //TITIK 5
            temp_vector.X = x - length / 1.0f;
            temp_vector.Y = y + length / 2.5f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / 1.0f;
            temp_vector.Y = y + length / 2.5f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - length / 1.0f;
            temp_vector.Y = y - length / 2.5f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + length / 1.0f;
            temp_vector.Y = y - length / 2.5f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);


            _indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
        }

        public void creattallbox(float x, float y, float z, float length, String m_color)//ttik pusat dari box dimana
        {//length panjang dari titik kubus

            _centerPosition.X = x; //jgn lupa selalu tambahkan ini
            _centerPosition.Y = y;
            _centerPosition.Z = z;

            this.color = m_color;

            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - length / 2.5f;
            temp_vector.Y = y + length / 1.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / 2.5f;
            temp_vector.Y = y + length / 1.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - length / 2.5f;
            temp_vector.Y = y - length / 1.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + length / 2.5f;
            temp_vector.Y = y - length / 1.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);

            //TITIK 5
            temp_vector.X = x - length / 2.5f;
            temp_vector.Y = y + length / 1.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / 2.5f;
            temp_vector.Y = y + length / 1.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - length / 2.5f;
            temp_vector.Y = y - length / 1.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + length / 2.5f;
            temp_vector.Y = y - length / 1.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);


            _indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
        }

        public void creatslimbox(float x, float y, float z, float length, String m_color)//ttik pusat dari box dimana
        {//length panjang dari titik kubus

            _centerPosition.X = x; //jgn lupa selalu tambahkan ini
            _centerPosition.Y = y;
            _centerPosition.Z = z;

            this.color = m_color;

            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - length / 3.5f;
            temp_vector.Y = y + length / 1.0f;
            temp_vector.Z = z - length / 1.0f;
            _vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / 3.5f;
            temp_vector.Y = y + length / 1.0f;
            temp_vector.Z = z - length / 1.0f;
            _vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - length / 3.5f;
            temp_vector.Y = y - length / 1.0f;
            temp_vector.Z = z - length / 1.0f;
            _vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + length / 3.5f;
            temp_vector.Y = y - length / 1.0f;
            temp_vector.Z = z - length / 1.0f;
            _vertices.Add(temp_vector);

            //TITIK 5
            temp_vector.X = x - length / 3.5f;
            temp_vector.Y = y + length / 1.0f;
            temp_vector.Z = z + length / 1.0f;
            _vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / 3.5f;
            temp_vector.Y = y + length / 1.0f;
            temp_vector.Z = z + length / 1.0f;
            _vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - length / 3.5f;
            temp_vector.Y = y - length / 1.0f;
            temp_vector.Z = z + length / 1.0f;
            _vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + length / 3.5f;
            temp_vector.Y = y - length / 1.0f;
            temp_vector.Z = z + length / 1.0f;
            _vertices.Add(temp_vector);


            _indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
        }

        public void creathandb(float x, float y, float z, float length, String m_color)//ttik pusat dari box dimana
        {//length panjang dari titik kubus

            _centerPosition.X = x; //jgn lupa selalu tambahkan ini
            _centerPosition.Y = y;
            _centerPosition.Z = z;

            this.color = m_color;

            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - length / 3.5f;
            temp_vector.Y = y + length / 1.0f;
            temp_vector.Z = z - length / 0.75f;
            _vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / 3.5f;
            temp_vector.Y = y + length / 1.0f;
            temp_vector.Z = z - length / 0.75f;
            _vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - length / 3.5f;
            temp_vector.Y = y - length / 1.0f;
            temp_vector.Z = z - length / 0.75f;
            _vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + length / 3.5f;
            temp_vector.Y = y - length / 1.0f;
            temp_vector.Z = z - length / 0.75f;
            _vertices.Add(temp_vector);

            //TITIK 5
            temp_vector.X = x - length / 3.5f;
            temp_vector.Y = y + length / 1.0f;
            temp_vector.Z = z + length / 0.75f;
            _vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / 3.5f;
            temp_vector.Y = y + length / 1.0f;
            temp_vector.Z = z + length / 0.75f;
            _vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - length / 3.5f;
            temp_vector.Y = y - length / 1.0f;
            temp_vector.Z = z + length / 0.75f;
            _vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + length / 3.5f;
            temp_vector.Y = y - length / 1.0f;
            temp_vector.Z = z + length / 0.75f;
            _vertices.Add(temp_vector);


            _indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
        }

        public void creathandc(float x, float y, float z, float length, String m_color)//ttik pusat dari box dimana
        {//length panjang dari titik kubus

            _centerPosition.X = x; //jgn lupa selalu tambahkan ini
            _centerPosition.Y = y;
            _centerPosition.Z = z;

            this.color = m_color;

            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - length / 3.5f;
            temp_vector.Y = y + length / 0.75f;
            temp_vector.Z = z - length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / 3.5f;
            temp_vector.Y = y + length / 0.75f;
            temp_vector.Z = z - length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - length / 3.5f;
            temp_vector.Y = y - length / 0.75f;
            temp_vector.Z = z - length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + length / 3.5f;
            temp_vector.Y = y - length / 0.75f;
            temp_vector.Z = z - length / 3.5f;
            _vertices.Add(temp_vector);

            //TITIK 5
            temp_vector.X = x - length / 3.5f;
            temp_vector.Y = y + length / 0.75f;
            temp_vector.Z = z + length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / 3.5f;
            temp_vector.Y = y + length / 0.75f;
            temp_vector.Z = z + length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - length / 3.5f;
            temp_vector.Y = y - length / 0.75f;
            temp_vector.Z = z + length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + length / 3.5f;
            temp_vector.Y = y - length / 0.75f;
            temp_vector.Z = z + length / 3.5f;
            _vertices.Add(temp_vector);


            _indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
        }

        public void createback(float x, float y, float z, float length, String m_color)//ttik pusat dari box dimana
        {//length panjang dari titik kubus

            _centerPosition.X = x; //jgn lupa selalu tambahkan ini
            _centerPosition.Y = y;
            _centerPosition.Z = z;

            this.color = m_color;

            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - length / 0.75f;
            temp_vector.Y = y + length / 1.25f;
            temp_vector.Z = z - length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / 0.75f;
            temp_vector.Y = y + length / 1.25f;
            temp_vector.Z = z - length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - length / 0.75f;
            temp_vector.Y = y - length / 1.25f;
            temp_vector.Z = z - length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + length / 0.75f;
            temp_vector.Y = y - length / 1.25f;
            temp_vector.Z = z - length / 3.5f;
            _vertices.Add(temp_vector);

            //TITIK 5
            temp_vector.X = x - length / 0.75f;
            temp_vector.Y = y + length / 1.25f;
            temp_vector.Z = z + length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / 0.75f;
            temp_vector.Y = y + length / 1.25f;
            temp_vector.Z = z + length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - length / 0.75f;
            temp_vector.Y = y - length / 1.25f;
            temp_vector.Z = z + length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + length / 0.75f;
            temp_vector.Y = y - length / 1.25f;
            temp_vector.Z = z + length / 3.5f;
            _vertices.Add(temp_vector);


            _indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
        }

        public void createfrontuper(float x, float y, float z, float length, String m_color)//ttik pusat dari box dimana
        {//length panjang dari titik kubus

            _centerPosition.X = x; //jgn lupa selalu tambahkan ini
            _centerPosition.Y = y;
            _centerPosition.Z = z;

            this.color = m_color;

            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - length / 0.75f;
            temp_vector.Y = y + length / 3.5f;
            temp_vector.Z = z - length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / 0.75f;
            temp_vector.Y = y + length / 3.5f;
            temp_vector.Z = z - length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - length / 0.75f;
            temp_vector.Y = y - length / 3.5f;
            temp_vector.Z = z - length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + length / 0.75f;
            temp_vector.Y = y - length / 3.5f;
            temp_vector.Z = z - length / 3.5f;
            _vertices.Add(temp_vector);

            //TITIK 5
            temp_vector.X = x - length / 0.75f;
            temp_vector.Y = y + length / 3.5f;
            temp_vector.Z = z + length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / 0.75f;
            temp_vector.Y = y + length / 3.5f;
            temp_vector.Z = z + length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - length / 0.75f;
            temp_vector.Y = y - length / 3.5f;
            temp_vector.Z = z + length / 3.5f;
            _vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + length / 0.75f;
            temp_vector.Y = y - length / 3.5f;
            temp_vector.Z = z + length / 3.5f;
            _vertices.Add(temp_vector);


            _indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
        }

        public void createtelapak(float x, float y, float z, float length, String m_color)//ttik pusat dari box dimana
        {//length panjang dari titik kubus

            _centerPosition.X = x; //jgn lupa selalu tambahkan ini
            _centerPosition.Y = y;
            _centerPosition.Z = z;

            this.color = m_color;

            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 3.5f;
            temp_vector.Z = z - length / 1.0f;
            _vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 3.5f;
            temp_vector.Z = z - length / 1.0f;
            _vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 3.5f;
            temp_vector.Z = z - length / 1.0f;
            _vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 3.5f;
            temp_vector.Z = z - length / 1.0f;
            _vertices.Add(temp_vector);

            //TITIK 5
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 3.5f;
            temp_vector.Z = z + length / 1.0f;
            _vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 3.5f;
            temp_vector.Z = z + length / 1.0f;
            _vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 3.5f;
            temp_vector.Z = z + length / 1.0f;
            _vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 3.5f;
            temp_vector.Z = z + length / 1.0f;
            _vertices.Add(temp_vector);


            _indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
        }

        public void addChildfoot(float x, float y, float z, float length, String color)
        {
            robot newChild = new robot(color);
            newChild.createtelapak(x, y, z, length, color);
            Child.Add(newChild);
        }

        public void addChildfrontup(float x, float y, float z, float length, String color)
        {
            robot newChild = new robot(color);
            newChild.createfrontuper(x, y, z, length, color);
            Child.Add(newChild);
        }

        public void addChildbackplate(float x, float y, float z, float length, String color)
        {
            robot newChild = new robot(color);
            newChild.createback(x, y, z, length, color);
            Child.Add(newChild);
        }

        public void addChildbluehand(float x, float y, float z, float length, String color)
        {
            robot newChild = new robot(color);
            newChild.creathandb(x, y, z, length, color);
            Child.Add(newChild);
        }

        public void addChildHeadRobot(float x, float y, float z, float length, String color)
        {
            robot newChild = new robot(color);
            newChild.createBodyRobot(x, y, z, length, color);
            Child.Add(newChild);
        }

        public void addChildantena(float x, float y, float z, float length, String color)
        {
            robot newChild = new robot(color);
            newChild.creatlongbox(x, y, z, length, color);
            Child.Add(newChild);
        }

        public void addChildantena2(float x, float y, float z, float length, String color)
        {
            robot newChild = new robot(color);
            newChild.creattallbox(x, y, z, length, color);
            Child.Add(newChild);
        }

        public void addChildflat(float x, float y, float z, float length, String color)
        {
            robot newChild = new robot(color);
            newChild.creatflatlongbox(x, y, z, length, color);
            Child.Add(newChild);
        }

        public void addChildCubes(float x, float y, float z, float length, String color)
        {
            robot newChild = new robot(color);
            newChild.createBoxVertices(x, y, z, length);
            Child.Add(newChild);
        }

        public void addChildplate(float x, float y, float z, float length, String color)
        {
            robot newChild = new robot(color);
            newChild.creatslimbox(x, y, z, length, color);
            Child.Add(newChild);
        }

        public void addChildhandc(float x, float y, float z, float length, String color)
        {
            robot newChild = new robot(color);
            newChild.creathandc(x, y, z, length, color);
            Child.Add(newChild);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public void loadObjFile(string path, String m_color)
        {
            this.color = m_color;
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Unable to open");
            }

            using (StreamReader streamreader = new StreamReader(path))
            {
                while (!streamreader.EndOfStream)
                {//vt= verteks tekstur, vn=lighting/shading, f=face indices
                    List<string> words = new List<string>(streamreader.ReadLine().ToLower().Split(" "));
                    words.RemoveAll(s => s == string.Empty);
                    if (words.Count == 0)
                    {
                        continue;
                    }
                    string type = words[0];
                    words.RemoveAt(0);

                    switch (type)
                    {//ngecilin obj /10
                        case "v":
                            _vertices.Add(new Vector3(float.Parse(words[0]) / 5, float.Parse(words[1]) / 5, float.Parse(words[2]) / 5));
                            break;
                        case "vt":
                            _textureVertices.Add(new Vector3(float.Parse(words[0]), float.Parse(words[1]), words.Count < 3 ? 0 : float.Parse(words[2])));
                            break;
                        case "vn":
                            _normals.Add(new Vector3(float.Parse(words[0]), float.Parse(words[1]), float.Parse(words[2])));
                            break;
                        case "f":
                            foreach (string w in words)
                            {
                                if (w.Length == 0)
                                {
                                    continue;
                                }
                                string[] comps = w.Split("/");
                                _indices.Add(uint.Parse(comps[0]) - 1);
                                //f tgh
                                //f blkng
                            }
                            break;
                    }
                }
            }
        }

        public void rotate(Vector3 pivot, Vector3 vector, float angle)
        {
            //pivot -> mau rotate di titik mana
            //vector -> mau rotate di sumbu apa? (x,y,z)
            //angle -> rotatenya berapa derajat?
            var real_angle = angle;
            angle = MathHelper.DegreesToRadians(angle);

            //mulai ngerotasi
            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i] = getRotationResult(pivot, vector, angle, _vertices[i]);
            }
            //rotate the euler direction
            for (int i = 0; i < 3; i++)
            {
                _euler[i] = getRotationResult(pivot, vector, angle, _euler[i], true);

                //NORMALIZE
                //LANGKAH - LANGKAH
                //length = akar(x^2+y^2+z^2)
                float length = (float)Math.Pow(Math.Pow(_euler[i].X, 2.0f) + Math.Pow(_euler[i].Y, 2.0f) + Math.Pow(_euler[i].Z, 2.0f), 0.5f);
                Vector3 temporary = new Vector3(0, 0, 0);
                temporary.X = _euler[i].X / length;
                temporary.Y = _euler[i].Y / length;
                temporary.Z = _euler[i].Z / length;
                _euler[i] = temporary;
            }
            _centerPosition = getRotationResult(pivot, vector, angle, _centerPosition); //nyimpen titik tengah obj

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * Vector3.SizeInBytes,
                _vertices.ToArray(), BufferUsageHint.StaticDraw);

            foreach (var item in Child)
            {
                item.rotate(pivot, vector, real_angle);
            }
        }

        Vector3 getRotationResult(Vector3 pivot, Vector3 vector, float angle, Vector3 point, bool isEuler = false)
        {
            Vector3 temp, newPosition;
            if (isEuler)
            {
                temp = point;
            }
            else
            {
                temp = point - pivot;
            }

            newPosition.X =
                (float)temp.X * (float)(Math.Cos(angle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(angle))) +
                (float)temp.Y * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) - vector.Z * Math.Sin(angle)) +
                (float)temp.Z * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) + vector.Y * Math.Sin(angle));
            newPosition.Y =
                (float)temp.X * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) + vector.Z * Math.Sin(angle)) +
                (float)temp.Y * (float)(Math.Cos(angle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(angle))) +
                (float)temp.Z * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) - vector.X * Math.Sin(angle));
            newPosition.Z =
                (float)temp.X * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) - vector.Y * Math.Sin(angle)) +
                (float)temp.Y * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) + vector.X * Math.Sin(angle)) +
                (float)temp.Z * (float)(Math.Cos(angle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(angle)));

            if (isEuler)
            {
                temp = newPosition;
            }
            else
            {
                temp = newPosition + pivot;
            }
            return temp;
        }

        public void resetEuler()
        {
            _euler[0] = new Vector3(1, 0, 0);
            _euler[1] = new Vector3(0, 1, 0);
            _euler[2] = new Vector3(0, 0, 1);
        }

        public void trans(float x, float y, float z)
        {
            transform = transform * Matrix4.CreateTranslation(x, y, z);
        }

        //untuk mengatur ukuran objek
        public void scale(float x)
        {

            transform = transform * Matrix4.CreateTranslation(-1 * (_centerPosition)) * Matrix4.CreateScale(x) * Matrix4.CreateTranslation((_centerPosition));
        }

    }
}

