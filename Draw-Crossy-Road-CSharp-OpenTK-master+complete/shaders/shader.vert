#version 330 core

//inisialisasi variabel dari window.cs, in jadi inputan
layout(location=0) in vec3 aPosition;
//layout(location=1) in vec3 aColor; //RGB only

//mengganti value file shader.frag, data akan dilempar ke shader.frag
//out vec4 vertexColor2;

//RGB only
//out vec3 ourColor;

uniform mat4 transform;
uniform mat4 view;
uniform mat4 projection;

void main(void){
	//DEFAULT var static (x,y,z,w) karena gl_Position minta 4 dimensi maka dibuat vec4
	gl_Position = vec4(aPosition,1.0) * transform * view * projection;

	//mewarnai MERAH pada object,lalu data akan dilempar ke shader.frag
	//vertexColor2 = vec4(1.0,0.0,0.0,1.0);

	//ourColor = aColor;
}
