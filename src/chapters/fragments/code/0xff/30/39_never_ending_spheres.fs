precision mediump float;
uniform vec2 u_res;
uniform float u_time;

#define NUM_STRIPS 8.
#define PI 3.141592658
#define TAU PI*2.

float cSDF(vec2 p, float r){
  return length(p) - r;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  
  vec2 lineWidthInPx = vec2(0.01);
  vec2 cellSize = vec2(.5); 

  vec2 modp = mod(p, cellSize);
  modp -= cellSize/2.;

  float t = u_time;
  float numCircles = .5; 
  vec3 ldir = vec3(0., 0., 1.);

  ///////////
  p/=.1;

  // snap to closest bottom corner
  vec2 BLCorner = floor(p*numCircles)/numCircles;
  p -= (BLCorner)+ 1.0;

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
  float anY = atan(xzVec.x/xzVec.z)/TAU + (t*1.);
  // float anX = (atan(yzVec.x/yzVec.z) + PI/2.) / PI;
  float div = 1.;
  float horizSlices = 0.4;
  float h = .25 * step(mod(p.y, horizSlices), horizSlices/2.);
  float col = step(div/4., mod(anY + h,div/2.)) * step(length(p)/1., 1.);

  // float c = step(cSDF(modp, circleRad), 0.);
  vec2 grid = step(mod(p, cellSize), lineWidthInPx);
  // col += (grid.x + grid.y);
  gl_FragColor = vec4(vec3(col), 1.);
}