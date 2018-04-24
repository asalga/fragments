precision mediump float;
uniform vec2 u_res;
uniform float u_time;
uniform vec3 u_mouse;
uniform vec2 u_tracking;

float cSDF(vec2 p, float r){
  return length(p)-r;
}
float ringSDF(vec2 p, float r, float w){
  return abs(length(p) - (r*.5)) - w;
}
float lens(vec2 p, float r, float n){
  float a = step(length(p+vec2(0.,r))-r, r);
  float b = step(length(p-vec2(0.,r))-r, r);
  return a*b;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  vec2 m = vec2(-(u_tracking.x*2.-1.)*.4, (u_tracking.y*2.-1.)*.1);

  float i = lens(p, .5, .5)
  - step(ringSDF(p + m, .7, .15), 0.)
  + step(cSDF(p+vec2(.25, -.25) + m, .05), 0.);
  gl_FragColor = vec4(vec3(i), 1.);
}