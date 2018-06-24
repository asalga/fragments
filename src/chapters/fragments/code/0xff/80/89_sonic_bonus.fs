// 17 - 
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
uniform vec3 u_mouse;

#define PI 3.14159
#define TAU 3.141592658 *2.
#define NUM_STRIPS 100.
#define DEG_TO_RAD PI/180.

mat2 r2d(float a){
 return mat2(cos(a),-sin(a),sin(a),cos(a));
}

/**
 * Return the normalized direction to march in from the 
 * eye point for a single pixel.
 * 
 * fieldOfView: vertical field of view in degrees
 * size: resolution of the output image
 * fragCoord: the x,y coordinate of the pixel in the output image
 */
vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
    vec2 xy = fragCoord - size / 2.0;
    float z = size.y / tan(fieldOfView / 2.0);
    return normalize(vec3(xy, -z));
}




void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time*.1;
  vec3 ldir = normalize(vec3(0, 0, 0));

  vec2 m2 = u_mouse.xy/u_res *2. -1.;




p.x += 1.4;
p *= .71;
// p *= r2d(PI/2.);


  // we know the x len and y len we also 
  // know the hypotenuse so we can calc z
  float z = sqrt(1.-p.x*p.x-p.y*p.y);
  vec3 n = normalize(vec3(p, z));

  

  // get angle between two vectors
  float theta = acos(dot(n,ldir));

  // how to distinguish between vertical and horiz
  float i = 1. - (theta / TAU);
  // float r = 1./NUM_STRIPS;

  vec3 xzVec = vec3(n.x, 0., n.z);
  xzVec = normalize(xzVec);
  // vec3 yzVec = vec3(n.y, 0., n.z);
  // yzVec = normalize(yzVec);
  float anY = atan(xzVec.x/xzVec.z)/TAU*1. + (t*.0);
  // float anX = (atan(yzVec.x/yzVec.z) + PI/2.) / PI;
  float div = .5;

  // ew i know i know
  // float horizSlices = 0.04;
  // float h = .25 * step(mod(p.y, horizSlices), horizSlices/2.);
  
  vec3 vtest = rayDirection( DEG_TO_RAD * 90., u_res, gl_FragCoord.xy);

  // contours
  vec3 l = normalize(vec3(0, 1, 0 ));
  
  vec3 v1 = normalize(vec3(p.x, p.y, z) - vtest);

  float c = smoothstep(0.1, 0.0001, sin(  dot(v1,l)*100.));
  // float c2 = smoothstep(0.1, 0.0001, sin(t*10. + dot(v2,l)*20.) )/2.;

  i = step(div/4., mod(anY,div/2.)) * step(length(p), 1.0);
  i += c;

  if(i == 2.){i = 0.;}



  gl_FragColor = vec4(vec3(i),1);
}





