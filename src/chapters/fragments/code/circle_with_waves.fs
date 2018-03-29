precision mediump float;
uniform float u_time;
uniform vec2 u_res;
#define EPSILON 0.02

float circle(vec2 p, float r){
  // len must be less than radius or radius must be > length  
  float plen = length(p);
  float fill = step(plen, r);
  fill *= step(r - EPSILON, plen);
  return fill;
}

/*
  Since we're going to overlap the teeth of the gear,
  when we specify the radius of the gear, the teeth
  are not taken into account

  p - current point
  r - radius of gear (not including teeth)
  numTeeth
  toothHeight
*/
float gear(vec2 p, float r, float numTeeth, float teethHeight){
  float theta = atan(p.y, p.x);
  float isInCircle = step(length(p), r);

  // we go around the circle and create a wavy pattern.
  // we can get the length of each vector and scale it by
  // sin. but we scale the normalized vector, not the 
  // the original--that would result in a wave too large
  // normalize just to get the direction.
  vec2 dir = normalize(p);
  vec2 t = (dir * sin(theta * numTeeth)) * 0.5;
  vec2 p2 = p + t;
  // p2 *= teethHeight;
  float plen = length(p2);

  // float teeth = step(plen, 1.);// * truncedStar;
  // float isInTeeth = * step(0.5, length(p));

  // return the points that are less than teethHeight
  float teeth = step(plen, 1.0);

  // return test * 1.0 - step(0.6, length(p));
  return teeth;
  return isInCircle;
  // return isInCircle + isInTeeth;
}

void main(){
  vec2 a = vec2(1.0, u_res.y/u_res.x);
  vec2 p = a * ((gl_FragCoord.xy / u_res) * 2. -1.);
  // Does this pixel have a part of the wavy circle?
  gl_FragColor = vec4(vec3(gear(p, 1.0, 10.0, 0.5)), 1.0);
}
