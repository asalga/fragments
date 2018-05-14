precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define TAU 3.14159265834*2.

float ringSDF(vec2 p, float r, float w){
  return abs(length(p) - (r*.5)) - w;
}

float rectSDF(vec2 p, vec2 size){
  vec2 absValue = abs(p / size);
  float side = max(absValue.x, absValue.y);
  return step(size.y, side + abs(sin(2.*p.x)));
}


float s(vec2 p){
  vec2 a = vec2(step(p, vec2(.5)));
  return a.x == 0. && a.y == 0. ? 1. : a.x * a.y;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time * 1.;
  // float r = mod(.15 / length(p) + t, 1.);

  // float th = mod(atan(p.y / p.x)/TAU, 1.);
  float i =0.;// step(ringSDF(p*1.,.5, .01), 0.);

  i = rectSDF(p+ (1./length(p)), vec2(0.3, 0.5));

  p *= mod( .8/(length(p)*1.3) + t, 1.4);


  
	

  gl_FragColor = vec4(vec3(i), 1.);
}