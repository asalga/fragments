// TODO: 
//  - dynamically calculate size of circle to be 80%
//    the size of cell  
precision mediump float;
uniform vec2 u_res;
void main(){
  vec2 a = vec2(1.,u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float gridThickness = 0.01;
  float circleRad = .2;
  // number of circles we want per side of y axis
  float numCircles = 2.;

  // if we are on an odd row
  if(mod(floor(p.y), 2.) == 0.0){
    // shift so that they alternate
    p.x += 1./(numCircles*2.0);
    // p.x += numCircles/2.;
    // p.y += 0.05;//circleRad/4.0;
  }


  // snap to closest bottom corner
  vec2 BLCorner = floor(p*numCircles)/numCircles;
  

  

  vec2 pointToCorner = p-BLCorner;
  // keep the vertical and horizontal lines the same thicknes
  // by taking into account the aspect ratio
  float grid = step(pointToCorner.x*a.y, gridThickness) + 
               step(pointToCorner.y*a.y, gridThickness);

  // TODO: fix this, should be dynamic
  vec2 circleCenter = BLCorner + (0.25);

  // diff between center of closest circle and point
  float lenToCircle = length(p-circleCenter);

  // If it's greater than that, then we're outside of the circle
  float circles = step(lenToCircle, circleRad);
  
  float i = 0.0 +
            circles + 
            grid + 
            0.0;


  gl_FragColor = vec4(vec3(i), 1.);
}