// 115 - "Withering Hope"

/*
d1 $ stut 4 0.5 0.5
   $ sound "hit:3"
   # gain 0.9

d2 $ n "1 ~ 2 ~ 3 0"
   # s "superchip"
   # gain 0.5
   # legato 2.0
*/


precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.14159265834
#define TAU (PI*2.)

float s(vec2 p){
	vec2 a = vec2(step(p, vec2(.5)));
	return a.x == 0. && a.y == 0. ? 1. : a.x * a.y;
}

void perp(inout vec2 p){
  p = vec2(-p.y, p.x);
}

void main(){
	vec2 a = vec2(1., u_res.y/u_res.x);
	vec2 p = a * (gl_FragCoord.xy/u_res)*2.-1.;
	perp(p);
	float test = pow(abs((atan(p.y,p.x)/PI)), .18);
	
	float lenDeform = abs((atan(p.y,p.x)/PI));
	
	float l = 1.- pow((lenDeform), 1.)/length(p);
	
	float r = fract((1.5/l) - u_time);
	float theta = mod((test * atan(p.y, p.x))/TAU*8., 1.);
	
	float i = s(vec2(r, theta)) * pow(l*1.8, 8.);
	gl_FragColor = vec4(vec3(i), 1.);
}