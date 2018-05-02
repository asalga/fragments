precision mediump float;
uniform vec2 u_res;
uniform vec3 u_mouse;
uniform float u_time;
#define TAU 3.141592658 *2.
#define NUM_STRIPS 10.

void main(){
  vec2 a = vec2(1.,u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);


  float gridThickness = 0.01;
  // number of circles we want per side of y axis
  float numCircles = 0.9; 
  //1.0;
  // clamp(u_mouse.x/20.,.5,20.);
  float circleRad = 0.1;  //1./(numCircles*2.);
  // p -= circleRad;






  //NES
  // float t = u_time;
  vec3 ldir = vec3(0., 0., 1.);
  // ldir = vec3(cos(t), .0, sin(t));




  // snap to closest bottom corner
  vec2 BLCorner = floor(p*numCircles)/numCircles;

// p -= (BLCorner + circleRad);
p -= BLCorner  - circleRad*1.;

  vec2 pointToCorner = p;//-BLCorner;
  // float grid = step(pointToCorner.x*a.y, gridThickness) + 
               // step(pointToCorner.y*a.y, gridThickness);

  // diff between center of closest circle and point
  // float lenToCircle = length(p-(BLCorner));

  



  // we know the x len and y len we also 
  // know the hypotenuse so we can calc z
  float z = sqrt(1.-p.x*p.x-p.y*p.y);
  vec3 n = normalize(vec3(p, z));
  // get angle between two vectors
  float theta = acos(dot(n,ldir));
  // how to distinguish between vertical and horiz
  float i;// = 1. - (theta / TAU);
  float r = 1./NUM_STRIPS;
  vec3 xzVec = vec3(n.x, 0., n.z);
  xzVec = normalize(xzVec);
  // vec3 yzVec = vec3(n.y, 0., n.z);
  // yzVec = normalize(yzVec);
  float anY = atan(xzVec.x/xzVec.z)/TAU;//+ (t*.1);
  // float anX = (atan(yzVec.x/yzVec.z) + PI/2.) / PI;
  float div = 1.;
  float horizSlices = 0.4;
  float h = .25 * step(mod(p.y, horizSlices), horizSlices/2.);
  i = step(div/4., mod(anY + h,div/2.)) * step(length(p), 1.0);


  // If it's greater then rad, then we're outside the circle
  // float circles = step(lenToCircle, circleRad);
 // i += circles;

  gl_FragColor = vec4(vec3(i), 1.);
}





//   // ew i know i know
//   gl_FragColor = vec4(vec3(i),1.);
// }