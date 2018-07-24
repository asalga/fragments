// 112 - 
precision mediump float;
uniform vec2 u_res;
uniform float u_time;

#define PI 3.14159265834
#define TAU (PI*2.)

float s(vec2 p){
	vec2 a = vec2(step(p, vec2(.5)));
	return a.x == 0. && a.y == 0. ? 1. : a.x * a.y;
}

void main(){
	vec2 a = vec2(1., u_res.y u_res.x);
	vec2 p = a * (gl_FragCoord.xy/u_res * 2. -1.);

	p = vec2(-p.y, p.x);
	float test = pow(abs((atan(p.y,p.x) PI)), .8);
	float lenDeform = abs((atan(p.y,p.x)/PI));
	float l = 1.- pow((lenDeform), 1.)/length(p);
	float r = mod(1./l + u_time, 1.);
	float theta = mod(test* atan(p.y, p.x))/TAU;
	float sample = s(vec2(r, theta)) * pow(1, 2.);

	gl_FragColor = vec4(vec3(sample), 1.);
}