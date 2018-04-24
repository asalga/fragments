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
  vec3 ldir = vec3(0., 0., 1.);
  ldir = normalize(ldir);
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  vec2 m = vec2(-.4,.1) * (u_tracking*2.-1.);
  float i = cSDF(p, 0.5);

  float b = 0.1;
  // ldir = vec3(cos((b+m.x)*.1), .0, sin((b+m.x)*.1));
  ldir = vec3(0., 0., 1.);

  float sz = 2.;
  float x=(p.x)*sz;//+m.x
  float y=(p.y)*sz;//+m.y
  float z = sqrt(1.-x*x-y*y);

  // do we need to normalize??
  vec3 n = normalize(vec3(p, z));

  float theta = acos(dot(ldir, n));
  theta = 1.-((theta*2.) /PI);
  
  // theta *= 30.;
  theta = step(theta, 0.1);
  

  // i=
   i = step(1., lens(p, .5, .5))
  - smoothstep(.01,.001,ringSDF(p + m, .7, .15))
  + smoothstep(.01,.001,cSDF(p+vec2(.25, -.25) + m, .05))
  + theta;

  gl_FragColor = vec4(vec3(i), 1.);
}