precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
float random (vec2 st) {
  return fract(sin(dot(st.xy,vec2(12.9898,178.233)))*43758.5453123);
}
void main(){
  vec2 a = vec2(1.,u_res.y/u_res.x);
  vec2 p = 1.1*a*(gl_FragCoord.xy / u_res * 2. -1.);
  float t = u_time;
  vec3 l = vec3(cos(t), 0., sin(t));
  float z = sqrt(1. - p.x*p.x - p.y*p.y);
  vec3 v = normalize(vec3(p.x, p.y, z));
  float c = step(sin(u_time*50.+ dot(v,l)*50.), 0.);
  gl_FragColor = vec4(vec3(c), 1.);
}