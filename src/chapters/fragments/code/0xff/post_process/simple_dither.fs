/*
	Simple Dithering
*/
precision mediump float;

uniform sampler2D u_t0;
uniform vec2 u_res;


void main(){	
	vec2 p = gl_FragCoord.xy/u_res;

	float i = texture2D(u_t0, p).x;

	gl_FragColor = vec4(vec3(i),1);
}