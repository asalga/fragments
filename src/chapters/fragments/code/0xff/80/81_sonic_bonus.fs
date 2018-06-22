// 17 - 
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define TAU 3.141592658 *2.
#define NUM_STRIPS 10.

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time/1.;
  vec3 ldir = normalize(vec3(0, 0, 0));
  // ldir = vec3(cos(t), .0, sin(t));

  // we know the x len and y len we also 
  // know the hypotenuse so we can calc z
  float z = sqrt(1.-p.x*p.x-p.y*p.y);
  vec3 n = normalize(vec3(p, z));

  // get angle between two vectors
  float theta = acos(dot(n,ldir));

  // how to distinguish between vertical and horiz
  float i = 1. - (theta / TAU);
  float r = 1./NUM_STRIPS;

  vec3 xzVec = vec3(1, n.x, n.z);
  xzVec = normalize(xzVec);
  // vec3 yzVec = vec3(n.y, 0., n.z);
  // yzVec = normalize(yzVec);
  float anY = atan(xzVec.x/xzVec.z)/TAU + (t*.9);
  // float anX = (atan(yzVec.x/yzVec.z) + PI/2.) / PI;
  float div = 1.;

  // ew i know i know
  float horizSlices = 0.4;
  float h = .25 * step(mod(p.y, horizSlices), horizSlices/2.);
  


  // contours
  vec3 l = normalize(vec3(0, 1, 1));
  // float z = sqrt(1. - p.x*p.x - p.y*p.y);
  
  vec3 v1 = normalize(vec3(p.x, p.y, z));
  vec3 v2 = normalize(vec3(p.x, p.y, -z));

  float c = smoothstep(0.1, 0.0001, sin(t*10. + dot(v1,l)*20.));
  // float c2 = smoothstep(0.1, 0.0001, sin(t*10. + dot(v2,l)*20.) )/2.;



  i = step(div/4., mod(anY+h,div/2.)) * step(length(p), 1.0);

  i += c;

  gl_FragColor = vec4(vec3(i),1);
}





