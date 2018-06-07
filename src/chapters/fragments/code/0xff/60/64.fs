// 64
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define TAU 3.141592658*2.
#define NUM_STRIPS 10.
vec2 swap(vec2 p){
  return vec2(p.y,p.x);
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  p = swap(p);
  float t = u_time*0.;
  // p *= .15;
  // p.x += 1.0;
  
  // we know the x len and y len we also 
  // know the hypotenuse so we can calc z
  float z = sqrt(1.-p.x*p.x-p.y*p.y);
  vec3 n = vec3(p, z);

  vec3 xzVec = normalize(vec3(0.,n.x, n.z));

  float anY = atan(xzVec.y/xzVec.z)/.4 + t;
  float div = 1.;

  // ew i know i know
  float horizSlices = .5;
  float h = .25 * step(mod(p.y, horizSlices), horizSlices/2.);
  float i = step(div/4., mod(anY + h,div/2.)) * step(length(p), 1.);
  gl_FragColor = vec4(vec3(i),1.);
}