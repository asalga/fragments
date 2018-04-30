precision mediump float;
uniform vec2 u_res;
uniform float u_time;
uniform vec3 u_mouse;
uniform vec2 u_tracking;
#define PI 3.141592658
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
  vec3 ldir = normalize(vec3(0., 0.52, 1.));
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  vec2 m = vec2(-.4,.1) * (u_tracking*2.-1.);
  vec2 pcopy = vec2(p);
  ldir = vec3(cos(m.x), .0, sin(m.x));
  pcopy = (pcopy+m) * 2.15;
  float z = (sqrt(1.-pcopy.x*pcopy.x-pcopy.y*pcopy.y));
  vec3 n = vec3(p, z);
  float theta = acos(dot(ldir, normalize(vec3(n.x, n.y, n.z))));
  theta = step(0.3, 1.-((theta*2.) /PI));
  float i = step(1., lens(p, .48, .5));
  i -= smoothstep(.01,.001,ringSDF(p + m, .7, .15));
  i += smoothstep(.01,.001,cSDF(p+vec2(.25, -.25) + m, .05))
  + theta;
  i += 2.* (step(1., lens(p, .52, .5)) - step(1., lens(p, .5, .5))); // fix
  gl_FragColor = vec4(vec3(i), 1.);
}