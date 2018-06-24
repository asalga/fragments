// 82 - "Sine Light"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;

  vec3 lightPos = vec3(0,0,2);
  i = (sin(p.x*PI*4.)+1.)/2.;

  float wavePos = sin(p.x*PI*4.);

  // 2 create normals
  vec3 n = normalize(vec3(cos(wavePos),0,sin(wavePos)));

  gl_FragColor = vec4(vec3(i),1.);
}