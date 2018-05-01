// TODO: 
//    - dynamically calculate size of circle
//

precision mediump float;
uniform vec2 u_res;
void main(){
  vec2 a = vec2(1.,u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float gridThickness = 0.01;
  float circleRad = .48;
  // number of circles we want per side of y axis
  float numCircles = 1.;
  // snap to closest bottom corner
  vec2 BLCorner = floor(p*numCircles)/numCircles;
  
  vec2 pointToCorner = p-BLCorner;
  // keep the vertical and horizontal lines the same thicknes
  // by taking into account the aspect ratio
  float grid = step(pointToCorner.x*a.y, gridThickness) + 
               step(pointToCorner.y*a.y, gridThickness);

  vec2 circleCenter = BLCorner + (numCircles/2.);

  // diff between center of closest circle and point
  float lenToCircle = length(p-circleCenter);

  // If it's greater than that, then we're outside of the circle
  float circles = step(lenToCircle, circleRad);
  
  float i = 0.0 +
            circles + 
            grid;

  gl_FragColor = vec4(vec3(i), 1.);
}