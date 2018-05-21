precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define TAU 3.14159265834*2.

float s(vec2 p){
  vec2 a = vec2(step(p, vec2(.5)));
  return a.x == 0. && a.y == 0. ? 1. : a.x * a.y;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/ u_res * 2. -1.);
  float r = mod(.5 / length(p) + u_time*.5, 1.);
  float th = mod(atan(p.y / p.x)/TAU, 1.);
  gl_FragColor = vec4(vec3(s(vec2(r, th))), 1.);
}