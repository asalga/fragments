precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define TAU 3.141592659 *2.
#define NUM_STRIPS 6.

float cSDF(vec2 p, float r){
  return length(p)-r;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  // vec3 ldir = vec3(0.707, 0., .707);
  vec3 ldir = vec3(0., 0., 1.);
   
  // we know the x len and y len we also 
  // know the hypotenuse so we can calc z
  float z = sqrt(1.-p.x*p.x-p.y*p.y);
  vec3 n = normalize(vec3(p, z));

  // get angle between two vectors
  float theta = acos(dot(n,ldir));

  // how to distinguish between vertical and horiz

  // normalize theta
  float i = 1. - (theta / TAU);
  float r = 1./NUM_STRIPS;


  // normals with the same theta will create 'circles'


  vec3 xzVec = vec3(n.x, 0., n.z);
  xzVec = normalize(xzVec);

  vec3 yzVec = vec3(n.x, n.y, 0.);
  yzVec = normalize(yzVec);

  float xzVecTheta = acos(dot(xzVec,ldir))/TAU;
  float yzVecTheta = acos(dot(yzVec,ldir))/TAU;

  i = step(mod(xzVecTheta, r), r/2.);
  
  
  // create a vector from (0,0,1) and rotate it
  // all the normals running down vertically a long the
  // sphere will have the same theta



  // normals that create strips


  // step(cSDF(p, .5), 0.);


  gl_FragColor = vec4(vec3(i),1.);
}