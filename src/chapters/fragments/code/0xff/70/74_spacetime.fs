precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658

float sdSquare(vec2 p, float halfW){
  vec2 _p = abs(p);
  return max(_p.x,_p.y)-halfW;
}

float every(float v, float i, float c, float stay){
	float alternate = step(mod(v, c), i);
	float x = mod(v, i) * alternate;
	// we reached the 'end'
	if(stay == 1. && alternate==0.){x = i;}
	return x;
}

mat2 r2d(float a){
	return mat2(cos(a),-sin(a),sin(a), cos(a));
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time*1.;
  
  float mvX = every(t   , 1., 2., 1.);
  float mvY = every(t+1., 1., 2., 0.);

  vec2 pos = vec2(.5-mvX, -.5+mvY);

  float flip = (step(mod(t, 4.),2.)*2.)-1.;
  p.x *= flip;
  p.y *= flip;

  i = step(sdSquare(p+pos,0.25),0.);

  gl_FragColor = vec4(vec3(i),1.);
}