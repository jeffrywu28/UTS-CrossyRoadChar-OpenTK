#version 330 //FILE INI UNTUK DUNIA PERWARNAAN

//default (BIARKAN ENABLE)
out vec4 outputColor; //output ke window.cs masuk ke alur draw obj

//terima data dari shader.vert,nama var harus sama
//in vec4 vertexColor2;
//in vec3 ourColor;

//terima data dari WINDOW.CS,agar WARNA bisa disetting di window.cs
//uniform vec4 ourColor;

void main(){ //parameter = r,g,b,alpha (max hanya sampai 1.0)

	//default
	outputColor = vec4(0.0,1.0,0.0,1.0);
	
	//terima data dari shad.vert
	//outputColor = vertexColor2;
	
	//terima data dari WINDOW.CS
	//outputColor = ourColor;

	//RGB only
	//outputColor = vec4(ourColor,1.0);
}