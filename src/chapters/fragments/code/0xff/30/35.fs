precision mediump float;
#define PI 3.141592658
#define TAU PI * 2.

uniform float u_time;
uniform vec2 u_res;

// float cSDF(vec2 p, float r){
//   return length(p) - r;
// }

void main(){
  vec2 ar = vec2(1., u_res.y/u_res.x);
  vec2 p = ar * (gl_FragCoord.xy/u_res*2.-1.);
  
  // i = step(0.51, mod(i, len/0.89));
  // circle for debugging
  // float i = step(cSDF(p, 0.5), 0.);
  // move spiral off to the side of the stem
  // p.x -= 0.4;
  // float theta = -atan(p.y,p.x);
  // float r = (length(p)*thickness)+(theta/TAU)/20.;

  float density = 2.;
  float a = atan(p.y,p.x)/PI;
  float r = length(p)*density + a/5.;
  float i = mod(r, .4);

  i = step(0.2, i);
  
  gl_FragColor = vec4(vec3(i,i,i), 1.);
}



  // limit the spiral size
  // r = step(length(p), sz) * r;

  // float sp = step(0.1, mod(r, 0.2));
  
  // gl_FragColor = vec4(vec3(sp), 1.);
