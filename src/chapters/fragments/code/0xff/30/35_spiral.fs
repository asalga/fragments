precision mediump float;
uniform float u_time;
uniform vec2 u_res;
#define PI 3.14159

void main(){
  vec2 ar = vec2(1., u_res.y/u_res.x);
  vec2 p = ar * (gl_FragCoord.xy/u_res*2.-1.);
  float density = 2.5;
  float a = ((atan(p.y,-p.x)/PI)+1.)/2.;
  float r = length(p)*density + a;
  float i = step(mod(r, 1.), 0.5);
  gl_FragColor = vec4(vec3(i), 1.);
}