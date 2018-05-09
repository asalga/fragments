precision mediump float;
uniform vec2 u_res;
uniform float u_time;
uniform vec3 u_mouse;
float cSDF(vec2 p, float r){
  return length(p)-r;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a* (gl_FragCoord.xy/u_res*2.-1.);
  vec2 m = vec2(-1., a.y) * ((u_mouse.xy/u_res)*2.-1.);
  
  float r1 = 0.35;
  vec2 c1p = m;

  float r2 = 0.6;
  vec2 c2p = vec2(cos(u_time), sin(u_time));
  
  float c1 = smoothstep(0.01, 0.001, cSDF(p+m, r1));
  float c2 = smoothstep(0.01, 0.001, cSDF(p+c2p, r2));

  float len = length(c1p-c2p);
  float i = abs(step(len,r1+r2)-(c1+c2));

  gl_FragColor = vec4(vec3(i), 1.);
}