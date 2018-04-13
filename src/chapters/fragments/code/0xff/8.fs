precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define TAU 3.141592658 * 2.

vec3 s(vec2 p, float m){
  vec3 test = vec3(.0);
  if(p.x < .5 && p.y < .5) test = vec3(0.);
  else if(p.x < .5 && p.y > .5) test = vec3(1.);
  else if(p.x > .5 && p.y < .5) test = vec3(1.);
  return test;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/ u_res * 2. -1.);
  float r = mod(.5 / length(p) + u_time, 1.);
  float th = mod(atan(p.y / p.x)/TAU, 1.);
  vec3 i = s(vec2(r, th), r);
  gl_FragColor = vec4(i, 1.);
}