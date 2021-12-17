using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using System.Collections.Generic;
using LearnOpenTK.Common;

namespace ConsoleApp2
{
    class Window : GameWindow /*gamewindow=class windownya openTK*/
    {

        //List<Vector3> _Vertices = new List<Vector3>();
        Asset3d[] _object3d = new Asset3d[6];
        Asset2d[] _object2d = new Asset2d[3];
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        robot[] _robot = new robot[2];
        Panda[] _panda = new Panda[1];
        bool _firstMove = true;
        Vector2 _lastPos = new Vector2();
        Camera _camera; 
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        float[] _verticesbez = { };
        uint[] _indicesbez = { };
        Vector3 a;
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
           
            _object3d[0] = new Asset3d();
            _object3d[1] = new Asset3d();
            _object3d[2] = new Asset3d();
            _object3d[3] = new Asset3d();
            _object3d[4] = new Asset3d();
 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _robot[0] = new robot();
            _panda[0] = new Panda();
 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   

        }

        protected override void OnLoad()
        {
            base.OnLoad();

            //Ganti warna background
            GL.ClearColor(0.02f, 0.6f, 0.63f, 1);

            GL.Enable(EnableCap.DepthTest);
            CursorGrabbed = true;
    
            // Body Panda
            _panda[0].createBodyPanda(1.2f, 0.0f, 0.0f, 0.65f, "putih");
            _panda[0].addChildBox(1.0f, 0.37f, 0.5f, 0.09f, "hitam");//telinga kiri
            _panda[0].addChildBox(1.4f, 0.37f, 0.5f, 0.09f, "hitam");//telinga kanan
            _panda[0].addChildCubes(1.1f, 0.15f, 0.65f, 0.12f, "hitam");//Luaran mata
            _panda[0].addChildCubes(1.1f, 0.15f, 0.7f, 0.05f, "putih");//mata
            _panda[0].addChildCubes(1.3f, 0.15f, 0.65f, 0.12f, "hitam");//Luaran mata
            _panda[0].addChildCubes(1.3f, 0.15f, 0.7f, 0.05f, "putih");//mata
            _panda[0].addChildflat(1.2f, -0.1f, 0.65f, 0.095f, "abu");//mulut

            _panda[0].addChildplate(0.96f, 0.0f, 0.2f, 0.33f, "hitam");//lapisan badan hitam pertama
            _panda[0].addChildplate(1.09f, 0.0f, 0.2f, 0.33f, "hitam");//lapisan badan hitam kedua
            _panda[0].addChildplate(1.26f, 0.0f, 0.2f, 0.33f, "hitam");//lapisan badan hitam ketiga
            _panda[0].addChildplate(1.44f, 0.0f, 0.2f, 0.33f, "hitam");//lapisan badan hitam keempat    

            _panda[0].addChildBoxStand(1.39f, -0.3f, -0.1f, 0.25f, "hitam");//kaki belakang kanan
            _panda[0].addChildBoxStand(1.39f, -0.3f, 0.4f, 0.25f, "hitam");//kaki depan kanan
            _panda[0].addChildBoxStand(1.01f, -0.3f, -0.1f, 0.25f, "hitam");//kaki belakang kiri
            _panda[0].addChildBoxStand(1.01f, -0.3f, 0.4f, 0.25f, "hitam");//kaki depan kiri

            _panda[0].addChildBoxStand(1.39f, -0.42f, -0.1f, 0.25f, "hitam");//kaki belakang kanan
            _panda[0].addChildBoxStand(1.39f, -0.42f, 0.4f, 0.25f, "hitam");//kaki depan kanan
            _panda[0].addChildBoxStand(1.01f, -0.42f, -0.1f, 0.25f, "hitam");//kaki belakang kiri
            _panda[0].addChildBoxStand(1.01f, -0.42f, 0.4f, 0.25f, "hitam");//kaki depan kiri

            _panda[0].addChildBoxStand(1.39f, -0.46f, -0.1f, 0.25f, "putih");//kaki belakang kanan
            _panda[0].addChildBoxStand(1.39f, -0.46f, 0.4f, 0.25f, "putih");//kaki depan kanan
            _panda[0].addChildBoxStand(1.01f, -0.46f, -0.1f, 0.25f, "putih");//kaki belakang kiri
            _panda[0].addChildBoxStand(1.01f, -0.46f, 0.4f, 0.25f, "putih");//kaki depan kiri

            

            _panda[0].addChildTabung(1.202f, 0.13f, -0.37f, 0.01f, 0.08f, 0.02f);//buntut
            //_panda[0].addChildBox(0.0f, 0.13f, -0.37f, 0.05f, "");//buntut

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Body Robot
            _robot[0].createBodyRobot(-1.2f, 0.0f, 0.0f, 0.65f,"birubody");// badan                     main
            _robot[0].addChildplate(-0.8f, 0.2f, 0.0f, 0.185f, "birubody2");//plate kanan               child 0
            _robot[0].addChildplate(-1.6f, 0.2f, 0.0f, 0.185f, "birubody2");//plate kiri                child 1
            _robot[0].addChildbackplate(-1.2f, 0.2f, -0.35f, 0.225f, "birubody2");//plate back          child 2
            _robot[0].addChildbackplate(-1.2f, 0.0f, 0.35f, 0.225f, "birubody2");//plate front          child 3
            _robot[0].addChildCubes(-1.455f, 0.22f, 0.37f, 0.09f, "putihrobot");// plate prt a kiri          child 4
            _robot[0].addChildCubes(-0.945f, 0.22f, 0.37f, 0.09f, "putihrobot");// plate prt a kanan         child 5
            _robot[0].addChildfrontup(-1.2f, 0.3f, 0.35f, 0.225f, "birubody2");//plate front uper       child 6
            _robot[0].addChildfrontup(-1.2f, 0.225f, 0.3f, 0.175f, "merah");//plate front uper          child 7
            _robot[0].addChildCubes(-1.345f, 0.0f, 0.4f, 0.08f, "putihrobot");// plate prt b kiri       child 8
            _robot[0].addChildCubes(-1.2f, 0.0f, 0.4f, 0.08f, "putihrobot");// plate prt b tngh         child 9
            _robot[0].addChildCubes(-1.045f, 0.0f, 0.4f, 0.08f, "putihrobot");// plate prt b kanan      child 10

            //kaki
            _robot[0].addChildantena(-1.05f, -0.5f, 0.0f, 0.2f, "birutangan");//kaki atas kanan         child 11
            _robot[0].addChildantena(-1.35f, -0.3f, 0.0f, 0.2f, "birutangan");//kaki atas kiri          child 12
            _robot[0].addChildantena(-1.05f, -0.6f, 0.0f, 0.1999f, "merah");//kaki bawah kanan          child 13
            _robot[0].addChildantena(-1.35f, -0.4f, 0.0f, 0.1999f, "merah");//kaki bawah kiri           child 14
            _robot[0].addChildfoot(-1.05f, -0.775f, 0.05f, 0.15f, "putihrobot");//kaki telapak kanan    child 15
            _robot[0].addChildfoot(-1.35f, -0.575f, 0.05f, 0.15f, "putihrobot");//kaki telapak kiri     child 16

            // hand
            _robot[0].addChildplate(-0.725f, 0.2f, 0.0f, 0.1f, "merah");//plate kanan                   child 17
            _robot[0].addChildplate(-1.675f, 0.2f, 0.0f, 0.1f, "merah");//plate kiri                    child 18
            _robot[0].addChildbluehand(-1.675f, 0.2f, 0.2375f, 0.1f, "birutangan");//lengan kiri        child 19
            _robot[0].addChildbluehand(-0.725f, 0.2f, 0.2375f, 0.1f, "birutangan");//lengan kanan       child 20
            _robot[0].addChildhandc(-0.725f, 0.2f, 0.4f, 0.1f, "putihrobot");//tgn kanan                child 21
            _robot[0].addChildhandc(-1.675f, 0.2f, 0.4f, 0.1f, "putihrobot");//tgn kiri                 child 22
            _robot[0].addChildCubes(-0.725f, 0.3f, 0.45f, 0.07f, "putihrobot");// tgn a kanan           child 23
            _robot[0].addChildCubes(-0.725f, 0.1f, 0.45f, 0.07f, "putihrobot");// tgn b kanan           child 24
            _robot[0].addChildCubes(-1.675f, 0.3f, 0.45f, 0.07f, "putihrobot");// tgn a kiri            child 25
            _robot[0].addChildCubes(-1.675f, 0.1f, 0.45f, 0.07f, "putihrobot");// tgn b kiri            child 26


            // kepala robot
            _robot[0].addChildHeadRobot(-1.2f, 0.5f, 0.0f, 0.55f, "birubody");// kepala                 child 27
            _robot[0].addChildCubes(-1.05f, 0.75f, 0.3f, 0.09f, "merah");// mata kanan                  child 28
            _robot[0].addChildCubes(-1.35f, 0.75f, 0.3f, 0.09f, "merah");//mata kiri                    child 29
            _robot[0].addChildCubes(-1.2f, 0.6f, 0.3f, 0.09f, "birubody2");//hidung                     child 30
            _robot[0].addChildantena(-1.2f, 0.9f, 0.1f, 0.095f, "putihrobot");//antena bawah            child 31
            _robot[0].addChildflat(-1.2f, 1.0f, 0.1f, 0.095f, "merah");//antena merah bawah             child 32
            _robot[0].addChildantena2(-1.125f, 1.06f, 0.1f, 0.095f, "merah");//antena merah kanan       child 33
            _robot[0].addChildantena2(-1.275f, 1.06f, 0.1f, 0.095f, "merah");//antena merah kiri        child 34
            _robot[0].addChildflat(-1.2f, 1.115f, 0.1f, 0.095f, "merah");//antena bawah bawah           child 35

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //kubus
            _object3d[0].createBoxBody(0.0f, 0.0f, 0.0f, 0.65f);
            _object3d[0].addChildBuletanTopi(0.0f, 0.35f, 0.3f, 0.45f,"biru");//topi1
            _object3d[0].addChildBuletanTopi(0.0f, 0.4f, 0.3f, 0.35f, "birumuda");//topi2
            _object3d[0].addChildBox(-0.2f, 0.43f, 0.3f, 0.05f, "");//telinga kiri
            _object3d[0].addChildBox(0.2f, 0.43f, 0.3f, 0.05f, "");//telinga kanan
            _object3d[0].addChildCubes(0.0f, 0.6f, 0.3f, 0.33f, "biru");//topi3
            _object3d[0].addChildCubes(0.0f, -0.1f, 0.65f, 0.25f,"pinkmuda");//hidung
            _object3d[0].addChildHidung(0.0f, -0.1f, 0.8f, 0.25f, "pinkavg");//hidung
            _object3d[0].addChildCubes(-0.05f, -0.06f, 0.81f, 0.05f, "");//hid
            _object3d[0].addChildCubes(0.05f, -0.06f, 0.81f, 0.05f, "");//hid
            _object3d[0].addChildCubes(0.1f, -0.2f, 0.79f, 0.04f, "putih");//taring
            _object3d[0].addChildCubes(-0.1f, -0.2f, 0.79f, 0.04f, "putih");//taring
            _object3d[0].addChildCubes(-0.1f, 0.15f, 0.65f, 0.12f, "putih");//mata
            _object3d[0].addChildCubes(-0.1f, 0.15f, 0.7f, 0.05f, "pinktua");//bolamata
            _object3d[0].addChildCubes(0.1f, 0.15f, 0.65f, 0.12f, "putih");//mata
            _object3d[0].addChildCubes(0.1f, 0.15f, 0.7f, 0.05f, "pinktua");//bolamata
            _object3d[0].addChildlidah(0.0f, -0.2f, 0.81f, 0.04f, "pinklidah");//lidah


            _object3d[0].addChildBoxStand(0.19f, -0.3f, -0.1f, 0.25f,"");//kaki belakang kanan
            _object3d[0].addChildBoxStand(0.19f, -0.3f, 0.4f, 0.25f, "");//kaki depan kanan
            _object3d[0].addChildBoxStand(-0.19f, -0.3f, -0.1f, 0.25f, "");//kaki belakang kiri
            _object3d[0].addChildBoxStand(-0.19f, -0.3f, 0.4f, 0.25f, "");//kaki depan kiri

            _object3d[0].addChildBoxStand(0.19f, -0.42f, -0.1f, 0.25f,"pinkavg");//kaki belakang kanan
            _object3d[0].addChildBoxStand(0.19f, -0.42f, 0.4f, 0.25f, "pinkavg");//kaki depan kanan
            _object3d[0].addChildBoxStand(-0.19f, -0.42f, -0.1f, 0.25f, "pinkavg");//kaki belakang kiri
            _object3d[0].addChildBoxStand(-0.19f, -0.42f, 0.4f, 0.25f, "pinkavg");//kaki depan kiri

            _object3d[0].addChildBoxStand(0.19f, -0.46f, -0.1f, 0.25f,"pinktua");//kaki belakang kanan
            _object3d[0].addChildBoxStand(0.19f, -0.46f, 0.4f, 0.25f, "pinktua");//kaki depan kanan
            _object3d[0].addChildBoxStand(-0.19f, -0.46f, -0.1f, 0.25f, "pinktua");//kaki belakang kiri
            _object3d[0].addChildBoxStand(-0.19f, -0.46f, 0.4f, 0.25f, "pinktua");//kaki depan kiri

            _object3d[0].addChildTabung(0.002f, 0.13f, -0.37f, 0.01f, 0.08f, 0.02f);//buntut
            //_object3d[0].addChildBox(0.0f, 0.13f, -0.37f, 0.05f, "");//buntut

            _object3d[1].createPersegi(0.0f, 0.3f, 0.0f, 0.65f,"hitam");
            _object3d[1].addChildpersegi(0.0f, 0.7f, 0.0f, 0.15f, "putih");

            //pohon
            _object3d[2].createBoxStandThin(1.7f, -1.0f,-0.5f, 0.5f,"coklat");
            _object3d[2].addChildBall(0.25f, 0.45f, 0.15f ,1.7f,-0.2f,-0.5f,"hijau");

            _object3d[3].createBoxStandThin(-1.7f, -1.0f, -0.5f, 0.5f, "coklat");
            _object3d[3].addChildBall(0.25f, 0.45f, 0.15f, -1.7f, -0.2f, -0.5f, "hijau");
            // _object3d[0].createElipsoidV2(0.25f, 0.25f, 0.25f ,0,0,0,72,24);

            //_object3d[0].createHyperboloidSatuSisi(0.25f, 0.25f, 0.25f, 0, 0, 0);

            //_object3d[0].createHyperboloidDuaSisi(0.1f, 0.1f, 0.1f, 0, 0, 0);

            //_object3d[0].createCone(0.01f, 0.01f, 0.01f, 0, 0, 0);

            //_object3d[0].createEllipticParaboloid(0.1f, 0.1f, 0.1f, 0, 0, 0);

            //_object3d[0].createHyperboloidParaboloid(0.1f, 0.1f, 0.1f, 0, 0, 0);
            //_object3d[0].loadObjFile("D:/grafkom/C#/ConsoleApp2/assets/building.obj");


            //_object3d[1].createBoxVertices(0.3f, -0.5f, 0.1f, 0.025f);
            //_object3d[1].load(Size.X, Size.Y);
            ////_object3d[0].addChild(0.3f,0.3f,0.0f,0.25f);

            //awan
            _object3d[4].createElipsoid(0.45f, 0.25f, 0.25f, 0, 0.8f, -1.0f,"putih");
            _object3d[4].addChildBall(0.15f, 0.15f, 0.15f, -0.5f, 0.8f, -1.0f, "putih");
            _object3d[4].addChildBall(0.15f, 0.15f, 0.15f, 0.5f, 0.8f, -1.0f, "putih");

            //_robot[0].createEllipsoidVertices(0.002f, 0.001f, 0.002f, 0.01f, 0.05f, 0.02f);
            
            //bezier
            //_object2d[0] = new Asset2d(_verticesbez, _indicesbez);
            //_object2d[0].load();

            _object3d[0].load(Size.X,Size.Y);
            _object3d[1].load(Size.X, Size.Y);
            _object3d[2].load(Size.X, Size.Y);
            _object3d[3].load(Size.X, Size.Y);
            _object3d[4].load(Size.X, Size.Y);

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _robot[0].load(Size.X, Size.Y);
            _panda[0].load(Size.X, Size.Y);
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //_robot[0].load(Size.X, Size.Y);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); //agar frame tidak bertumpuk saat ganti frame

            _object3d[0].render(2);
            _object3d[1].render(2);
            _object3d[2].render(2);
            _object3d[3].render(2);
            _object3d[4].render(2);
            _robot[0].render(2);
            _panda[0].render(2);

            //_robot[0].rotate(_robot[0]._centerPosition, _robot[0]._euler[1], 1); //euler[0] = x, [1]=y, [2]=z
            //_object3d[0].resetEuler();
            SwapBuffers();
        }
        int counter = 0;
        //KEYBOARD FUNCTION
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            var input = KeyboardState;

            if (counter < 100)
            {
                _object3d[0].Child[16].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], 0.1f);
                _object3d[0].Child[16 + 4].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], 0.1f);
                _object3d[0].Child[16 + 8].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], 0.1f);

                _object3d[0].Child[17].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], 0.1f);
                _object3d[0].Child[17 + 4].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], 0.1f);
                _object3d[0].Child[17 + 8].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], 0.1f);

                _object3d[0].Child[18].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], 0.1f);
                _object3d[0].Child[18 + 4].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], 0.1f);
                _object3d[0].Child[18 + 8].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], 0.1f);

                _object3d[0].Child[19].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], 0.1f);
                _object3d[0].Child[19 + 4].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], 0.1f);
                _object3d[0].Child[19 + 8].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], 0.1f);
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                _robot[0].Child[11].trans(0.0f, 0.0025f, 0.0f);
                _robot[0].Child[13].trans(0.0f, 0.0025f, 0.0f);
                _robot[0].Child[15].trans(0.0f, 0.0025f, 0.0f);


                _robot[0].Child[12].trans(0.0f, -0.0025f, 0.0f);
                _robot[0].Child[14].trans(0.0f, -0.0025f, 0.0f);
                _robot[0].Child[16].trans(0.0f, -0.0025f, 0.0f);
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////           

                _panda[0].Child[11].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], 0.1f);
                _panda[0].Child[11 + 4].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], 0.1f);
                _panda[0].Child[11 + 8].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], 0.1f);

                _panda[0].Child[12].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], 0.1f);
                _panda[0].Child[12 + 4].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], 0.1f);
                _panda[0].Child[12 + 8].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], 0.1f);

                _panda[0].Child[13].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], 0.1f);
                _panda[0].Child[13 + 4].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], 0.1f);
                _panda[0].Child[13 + 8].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], 0.1f);

                _panda[0].Child[14].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], 0.1f);
                _panda[0].Child[14 + 4].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], 0.1f);
                _panda[0].Child[14 + 8].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], 0.1f);

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                _object3d[2].trans(0.0f, 0.00f, 0.25f);
                _object3d[2].Child[0].trans(0.0f, 0.00f, 0.25f);
                _object3d[3].trans(0.0f, 0.00f, 0.25f);
                _object3d[3].Child[0].trans(0.0f, 0.00f, 0.25f);

                _object3d[4].trans(0.0f, 0.00f, 0.125f);
                _object3d[4].Child[0].trans(0.0f, 0.00f, 0.125f);
                _object3d[4].Child[1].trans(0.0f, 0.00f, 0.125f);



            }
            else if (counter < 200)
            {
                _object3d[0].Child[16].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], -0.1f);
                _object3d[0].Child[16 + 4].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], -0.1f);
                _object3d[0].Child[16 + 8].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], -0.1f);

                _object3d[0].Child[17].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], -0.1f);
                _object3d[0].Child[17 + 4].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], -0.1f);
                _object3d[0].Child[17 + 8].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], -0.1f);

                _object3d[0].Child[18].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], -0.1f);
                _object3d[0].Child[18 + 4].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], -0.1f);
                _object3d[0].Child[18 + 8].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], -0.1f);

                _object3d[0].Child[19].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], -0.1f);
                _object3d[0].Child[19 + 4].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], -0.1f);
                _object3d[0].Child[19 + 8].rotate(_object3d[0].Child[16]._centerPosition, _object3d[0]._euler[0], -0.1f);
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                _robot[0].Child[11].trans(0.0f, -0.0025f, 0.0f);
                _robot[0].Child[13].trans(0.0f, -0.0025f, 0.0f);
                _robot[0].Child[15].trans(0.0f, -0.0025f, 0.0f);

                _robot[0].Child[12].trans(0.0f, 0.0025f, 0.0f);
                _robot[0].Child[14].trans(0.0f, 0.0025f, 0.0f);
                _robot[0].Child[16].trans(0.0f, 0.0025f, 0.0f);
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                _panda[0].Child[11].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], -0.1f);
                _panda[0].Child[11 + 4].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], -0.1f);
                _panda[0].Child[11 + 8].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], -0.1f);

                _panda[0].Child[12].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], -0.1f);
                _panda[0].Child[12 + 4].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], -0.1f);
                _panda[0].Child[12 + 8].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], -0.1f);

                _panda[0].Child[13].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], -0.1f);
                _panda[0].Child[13 + 4].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], -0.1f);
                _panda[0].Child[13 + 8].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], -0.1f);

                _panda[0].Child[14].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], -0.1f);
                _panda[0].Child[14 + 4].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], -0.1f);
                _panda[0].Child[14 + 8].rotate(_panda[0].Child[11]._centerPosition, _panda[0]._euler[0], -0.1f);

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                _object3d[2].trans(0.0f, 0.00f, -0.25f);
                _object3d[2].Child[0].trans(0.0f, 0.00f, -0.25f);
                _object3d[3].trans(0.0f, 0.00f, -0.25f);
                _object3d[3].Child[0].trans(0.0f, 0.00f, -0.25f);
                _object3d[4].trans(0.0f, 0.00f, -0.125f);
                _object3d[4].Child[0].trans(0.0f, 0.00f, -0.125f);
                _object3d[4].Child[1].trans(0.0f, 0.00f, -0.125f);


            }
            else
            {
                counter = -1;
            }
            counter++;
            Console.WriteLine(counter);

            //exit with escape keyboard
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            };

       
        }

        //Untuk Resize Window
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                float _x = (MousePosition.X - Size.X / 2) / (Size.X / 2);
                float _y = -(MousePosition.Y - Size.Y / 2) / (Size.Y / 2);
                
            }
            base.OnMouseDown(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e){
            base.OnKeyDown(e);
            if(e.Key == Keys.A){
                _object3d[0].rotate(_object3d[0]._centerPosition, _object3d[0]._euler[1],1);
            }
            if (e.Key == Keys.D)
            {
                _object3d[0].rotate(_object3d[0]._centerPosition, _object3d[0]._euler[1], -1);
            }
            if (e.Key == Keys.W){
                //_object3d[0].Child[0].rotate(_object3d[0].Child[0]._centerPosition,_object3d[0].Child[0]._euler[0],1);
                _object3d[0].rotate(_object3d[0]._centerPosition, _object3d[0]._euler[0], 1);
            }
            if (e.Key == Keys.S)
            {
                //_object3d[0].Child[0].rotate(_object3d[0].Child[0]._centerPosition,_object3d[0].Child[0]._euler[0],1);
                _object3d[0].rotate(_object3d[0]._centerPosition, _object3d[0]._euler[0], -1);
            }
            if (e.Key == Keys.Left)
            {
                _object3d[4].trans(-0.005f, 0.0f, 0.0f);
                _object3d[4].Child[0].trans(-0.005f, 0.0f, 0.0f);
                _object3d[4].Child[1].trans(-0.005f, 0.0f, 0.0f);
            }
            if (e.Key == Keys.Right)
            {
                _object3d[4].trans(0.005f, 0.0f, 0.0f);
                _object3d[4].Child[0].trans(0.005f, 0.0f, 0.0f);
                _object3d[4].Child[1].trans(0.005f, 0.0f, 0.0f);
            }
            if (e.Key == Keys.Up)
            {
                //_object3d[1].trans(0.0f, 0.005f, 0.0f);
                _object3d[4].scale(1.005f);
                _object3d[4].Child[0].scale(1.005f);
                _object3d[4].Child[1].scale(1.005f);
            }
            if (e.Key == Keys.Down)
            {
                _object3d[4].scale(-1.005f);
                _object3d[4].Child[0].scale(-1.005f);
                _object3d[4].Child[1].scale(-1.005f);
            }
            //if (e.Key == Keys.KeyPad1)
            //{
            //    Console.WriteLine(_object3d[0] + " " + (box1.getPosY() + box1.getLengthX()));
            //}

            if (e.Key == Keys.H)
            {
                _panda[0].rotate(_panda[0]._centerPosition, _panda[0]._euler[1], 5);
            }
            if (e.Key == Keys.F)
            {
                _panda[0].rotate(_panda[0]._centerPosition, _panda[0]._euler[1], -5);
            }
            if (e.Key == Keys.T)
            {
                _panda[0].rotate(_panda[0]._centerPosition, _panda[0]._euler[0], 5);
            }
            if (e.Key == Keys.G)
            {
                _panda[0].rotate(_panda[0]._centerPosition, _panda[0]._euler[0], -5);
            }



            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (e.Key == Keys.L)
            {
                _robot[0].rotate(_robot[0]._centerPosition, _robot[0]._euler[1], 5);
            }
            if (e.Key == Keys.J)
            {
                _robot[0].rotate(_robot[0]._centerPosition, _robot[0]._euler[1], -5);
            }
            if (e.Key == Keys.I)
            {
                _robot[0].rotate(_robot[0]._centerPosition, _robot[0]._euler[0], 5);
            }
            if (e.Key == Keys.K)
            {
                _robot[0].rotate(_robot[0]._centerPosition, _robot[0]._euler[0], -5);
            }
            if (e.Key == Keys.U) // tgn kiri up
            {
                _robot[0].Child[18].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], -0.75f);
                _robot[0].Child[19].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], -0.75f);
                _robot[0].Child[22].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], -0.75f);
                _robot[0].Child[25].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], -0.75f);
                _robot[0].Child[26].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], -0.75f);
            }
            if (e.Key == Keys.M) // tgn kiri down
            {
                _robot[0].Child[18].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], 0.75f);
                _robot[0].Child[19].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], 0.75f);
                _robot[0].Child[22].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], 0.75f);
                _robot[0].Child[25].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], 0.75f);
                _robot[0].Child[26].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], 0.75f);
            }
            if (e.Key == Keys.Comma) // tgn kanan up
            {
                _robot[0].Child[17].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], 0.75f);
                _robot[0].Child[20].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], 0.75f);
                _robot[0].Child[21].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], 0.75f);
                _robot[0].Child[23].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], 0.75f);
                _robot[0].Child[24].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], 0.75f);
            }
            if (e.Key == Keys.O) // tgn kanan down
            {
                _robot[0].Child[17].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], -0.75f);
                _robot[0].Child[20].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], -0.75f);
                _robot[0].Child[21].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], -0.75f);
                _robot[0].Child[23].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], -0.75f);
                _robot[0].Child[24].rotate(_robot[0].Child[17]._centerPosition, _robot[0]._euler[0], -0.75f);
            }
            if (e.Key == Keys.KeyPadDivide)
            {
                _robot[0].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[0].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[1].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[2].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[3].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[4].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[5].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[6].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[7].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[8].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[9].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[10].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[11].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[12].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[13].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[14].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[15].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[16].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[17].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[18].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[19].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[20].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[21].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[22].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[23].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[24].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[25].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[26].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[27].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[28].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[29].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[30].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[31].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[32].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[33].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[34].trans(0.0f, 0.0f, 0.025f);
                _robot[0].Child[35].trans(0.0f, 0.0f, 0.025f);
            } // maju robot
            if (e.Key == Keys.KeyPad8)
            {
                _robot[0].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[0].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[1].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[2].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[3].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[4].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[5].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[6].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[7].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[8].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[9].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[10].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[11].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[12].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[13].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[14].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[15].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[16].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[17].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[18].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[19].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[20].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[21].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[22].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[23].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[24].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[25].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[26].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[27].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[28].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[29].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[30].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[31].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[32].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[33].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[34].trans(0.0f, 0.0f, -0.025f);
                _robot[0].Child[35].trans(0.0f, 0.0f, -0.025f);
            } // mundur robot
            if (e.Key == Keys.KeyPad9)
            {
                _robot[0].trans(-0.025f, -0f, 0f);
                _robot[0].Child[0].trans(-0.025f, -0f, 0f);
                _robot[0].Child[1].trans(-0.025f, -0f, 0f);
                _robot[0].Child[2].trans(-0.025f, -0f, 0f);
                _robot[0].Child[3].trans(-0.025f, -0f, 0f);
                _robot[0].Child[4].trans(-0.025f, -0f, 0f);
                _robot[0].Child[5].trans(-0.025f, -0f, 0f);
                _robot[0].Child[6].trans(-0.025f, -0f, 0f);
                _robot[0].Child[7].trans(-0.025f, -0f, 0f);
                _robot[0].Child[8].trans(-0.025f, -0f, 0f);
                _robot[0].Child[9].trans(-0.025f, -0f, 0f);
                _robot[0].Child[10].trans(-0.025f, -0f, 0f);
                _robot[0].Child[11].trans(-0.025f, -0f, 0f);
                _robot[0].Child[12].trans(-0.025f, -0f, 0f);
                _robot[0].Child[13].trans(-0.025f, -0f, 0f);
                _robot[0].Child[14].trans(-0.025f, -0f, 0f);
                _robot[0].Child[15].trans(-0.025f, -0f, 0f);
                _robot[0].Child[16].trans(-0.025f, -0f, 0f);
                _robot[0].Child[17].trans(-0.025f, -0f, 0f);
                _robot[0].Child[18].trans(-0.025f, -0f, 0f);
                _robot[0].Child[19].trans(-0.025f, -0f, 0f);
                _robot[0].Child[20].trans(-0.025f, -0f, 0f);
                _robot[0].Child[21].trans(-0.025f, -0f, 0f);
                _robot[0].Child[22].trans(-0.025f, -0f, 0f);
                _robot[0].Child[23].trans(-0.025f, -0f, 0f);
                _robot[0].Child[24].trans(-0.025f, -0f, 0f);
                _robot[0].Child[25].trans(-0.025f, -0f, 0f);
                _robot[0].Child[26].trans(-0.025f, -0f, 0f);
                _robot[0].Child[27].trans(-0.025f, -0f, 0f);
                _robot[0].Child[28].trans(-0.025f, -0f, 0f);
                _robot[0].Child[29].trans(-0.025f, -0f, 0f);
                _robot[0].Child[30].trans(-0.025f, -0f, 0f);
                _robot[0].Child[31].trans(-0.025f, -0f, 0f);
                _robot[0].Child[32].trans(-0.025f, -0f, 0f);
                _robot[0].Child[33].trans(-0.025f, -0f, 0f);
                _robot[0].Child[34].trans(-0.025f, -0f, 0f);
                _robot[0].Child[35].trans(-0.025f, -0f, 0f);
            } // kanan robot
            if (e.Key == Keys.KeyPad7)
            {
                _robot[0].trans(0.025f, 0f, 0f);
                _robot[0].Child[0].trans(0.025f, 0f, 0f);
                _robot[0].Child[1].trans(0.025f, 0f, 0f);
                _robot[0].Child[2].trans(0.025f, 0f, 0f);
                _robot[0].Child[3].trans(0.025f, 0f, 0f);
                _robot[0].Child[4].trans(0.025f, 0f, 0f);
                _robot[0].Child[5].trans(0.025f, 0f, 0f);
                _robot[0].Child[6].trans(0.025f, 0f, 0f);
                _robot[0].Child[7].trans(0.025f, 0f, 0f);
                _robot[0].Child[8].trans(0.025f, 0f, 0f);
                _robot[0].Child[9].trans(0.025f, 0f, 0f);
                _robot[0].Child[10].trans(0.025f, 0f, 0f);
                _robot[0].Child[11].trans(0.025f, 0f, 0f);
                _robot[0].Child[12].trans(0.025f, 0f, 0f);
                _robot[0].Child[13].trans(0.025f, 0f, 0f);
                _robot[0].Child[14].trans(0.025f, 0f, 0f);
                _robot[0].Child[15].trans(0.025f, 0f, 0f);
                _robot[0].Child[16].trans(0.025f, 0f, 0f);
                _robot[0].Child[17].trans(0.025f, 0f, 0f);
                _robot[0].Child[18].trans(0.025f, 0f, 0f);
                _robot[0].Child[19].trans(0.025f, 0f, 0f);
                _robot[0].Child[20].trans(0.025f, 0f, 0f);
                _robot[0].Child[21].trans(0.025f, 0f, 0f);
                _robot[0].Child[22].trans(0.025f, 0f, 0f);
                _robot[0].Child[23].trans(0.025f, 0f, 0f);
                _robot[0].Child[24].trans(0.025f, 0f, 0f);
                _robot[0].Child[25].trans(0.025f, 0f, 0f);
                _robot[0].Child[26].trans(0.025f, 0f, 0f);
                _robot[0].Child[27].trans(0.025f, 0f, 0f);
                _robot[0].Child[28].trans(0.025f, 0f, 0f);
                _robot[0].Child[29].trans(0.025f, 0f, 0f);
                _robot[0].Child[30].trans(0.025f, 0f, 0f);
                _robot[0].Child[31].trans(0.025f, 0f, 0f);
                _robot[0].Child[32].trans(0.025f, 0f, 0f);
                _robot[0].Child[33].trans(0.025f, 0f, 0f);
                _robot[0].Child[34].trans(0.025f, 0f, 0f);
                _robot[0].Child[35].trans(0.025f, 0f, 0f);
            } // kiri robot
              ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
              float cameraspeed = 0.5f;
            //if (e.Key == Keys.Up)
            //{
            //    _camera.Position += _camera.Front * cameraspeed * (float)args.Time
            //}
            //if (e.Key == Keys.Down)
            //{
            //    _camera.Position -= _camera.Front * cameraspeed * (float)args.Time
            //}
            //if (e.Key == Keys.Left)
            //{
            //    _camera.Position -= _camera.Right * cameraspeed * (float)args.Time
            //}
            //if (e.Key == Keys.Right)
            //{
            //    _camera.Position += _camera.Right * cameraspeed * (float)args.Time
            //}



        }
    }

}
