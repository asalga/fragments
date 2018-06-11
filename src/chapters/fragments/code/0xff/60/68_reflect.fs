// 68 - "Reflect"
precision mediump float;

#define PI 3.141592658
uniform vec2 u_res;
uniform float u_time;

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;

  float pLen = length(p);
  i = sin(pLen * PI);

  gl_FragColor = vec4(vec3(i),1.);
}