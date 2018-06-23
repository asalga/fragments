// 82 - "Sine Light"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  vec3 light = vec3(0,0,2);

  // 1 cylinder waves - DONE
  i = (sin(p.x*PI*4.)+1.)/2.;



  // 2 create normals

  gl_FragColor = vec4(vec3(i),1.);
}