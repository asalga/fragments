precision mediump float;
#define TAU 3.141592658*2.
uniform vec2 u_res;
uniform float u_time;
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  p.x = (p.x+1.)*.5;// adjust x

  float i = step(p.y, sin(p.x*TAU));
  
  gl_FragColor = vec4(vec3(i),1.);
}