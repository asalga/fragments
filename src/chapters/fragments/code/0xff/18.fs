precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define TAU 3.141592658 *2.
#define PI TAU/2.

float cSDF(vec2 p, float r){
  return length(p)-r;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time/1.;
  vec3 ldir = vec3(cos(t), .0, sin(t));
  // we know the x len and y len we also 
  // know the hypotenuse so we can calc z
  float z = sqrt(1.-p.x*p.x-p.y*p.y);
  vec3 n = normalize(vec3(p, z));
  float theta = acos(dot(n,ldir));

  // how to distinguish between vertical and horiz
  // float i = 1. - (theta / TAU);
  // float r = 1./NUM_STRIPS;

  vec3 xzVec = vec3(n.x, 0., n.z);
  xzVec = normalize(xzVec);
  // vec3 yzVec = vec3(n.y, 0., n.z);
  // yzVec = normalize(yzVec);
  float anY = atan(xzVec.x/xzVec.z)/TAU + (t*.1);
  // float anX = (atan(yzVec.x/yzVec.z) + PI/2.)/PI;
  float div = 1.;

  // ew i know i know
  float horizSlices = 0.5;
  float h = .25 * step(mod(p.y, horizSlices), horizSlices/2.);
  float i = step(div/4., mod(anY + h,div/2.)) * step(length(p), 1.0);
  gl_FragColor = vec4(vec3(i),1.);
}