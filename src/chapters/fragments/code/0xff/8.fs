precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define TAU 3.141592658 * 2.


// We need to create a function that samples a
// procedural texture we created.
//
vec3 sample(vec2 p, float m){
  vec3 test = vec3(.0);
  if(p.x < .5 && p.y < .5) test = vec3(0.);
  else if(p.x < .5 && p.y > .5) test = vec3(1.);
  else if(p.x > .5 && p.y < .5) test = vec3(1.);
  return test;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/ u_res * 2. -1.);
  float l = length(p);
  float t = u_time * 1.;
  float r = mod(.25 / l + t, 1.);
  float th = mod(atan(p.y / p.x)/TAU, 1.);
  vec2 u = vec2(r, th); 
  vec3 i = sample(u, r);
  gl_FragColor = vec4(i, 1.);
}