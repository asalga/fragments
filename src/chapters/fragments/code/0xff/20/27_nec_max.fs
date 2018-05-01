precision mediump float;
uniform vec2 u_res;
uniform vec3 u_mouse;
void main(){
  vec2 a = vec2(1.,u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float gridThickness = 0.01;
  // number of circles we want per side of y axis
  float numCircles = 1.0;
  //clamp(u_mouse.x/20.,.5,20.);
  float circleRad = 1./(numCircles*2.);
  p += circleRad;

  // snap to closest bottom corner
  vec2 BLCorner = floor(p*numCircles)/numCircles;
  
  vec2 pointToCorner = p-BLCorner;
  float grid = step(pointToCorner.x*a.y, gridThickness) + 
               step(pointToCorner.y*a.y, gridThickness);

  // diff between center of closest circle and point
  float lenToCircle = length(p-(BLCorner + circleRad));

  // If it's greater then rad, then we're outside the circle
  float circles = 1.-smoothstep(circleRad*0.98,circleRad,lenToCircle);
  gl_FragColor = vec4(vec3(circles), 1.);
}