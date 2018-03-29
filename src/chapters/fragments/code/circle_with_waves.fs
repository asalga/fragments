precision mediump float;
uniform float u_time;
uniform vec2 u_res;

/*
  Since we're going to overlap the teeth of the gear,
  when we specify the radius of the gear, the teeth
  are not taken into account
  r - radius of gear (not including teeth)
*/
float gear(vec2 p, float r, float numTeeth, float teethHeight, float d){
  float theta = atan(p.y, p.x);
  vec2 dir = normalize(p);
  vec2 teeth = dir * sin((d * u_time + theta) *  numTeeth) * teethHeight;
  float plen = length(p + teeth);
  float pl = length(p);

  float truncedTeeth = 1.0-step(r + teethHeight/2., length(p));
  float isInTeeth = step(plen, r);
  float isInCircle = step(pl, r);

  return (isInCircle + isInTeeth) * truncedTeeth * step(0.4, pl);
}

// same as above, but fewer lines
float gear2(vec2 p, float r, float numTeeth, float teethHeight){
  float theta = atan(p.y, p.x);
  // we go around the circle and create a wavy pattern.
  // we can get the length of each vector and scale it by
  // sin. but we scale the normalized vector, not the 
  // the original--that would result in a wave too large
  // normalize just to get the direction.
  float pl = length(p);
  float truncedTeeth = 1. - step(r + teethHeight/2., pl);
  vec2 dir = normalize(p);
  vec2 t = dir * min(0., sin(theta * numTeeth)) * teethHeight;
  return step(length(p + t), r) * truncedTeeth  * step(0.4, pl);//hollow
}

void main(){
  vec2 a = vec2(1.0, u_res.y/u_res.x);
  vec2 p = a * ((gl_FragCoord.xy / u_res) * 2. -1.);
  
  float g0 = gear2(vec2(0.55, 0.) + p, 0.5, 30.0, 0.2);
  float g1 = gear2(vec2(-.55, 0.) + p, 0.5, 30.0, 0.2);

  gl_FragColor = vec4(vec3(g0 + g1), 1.0);
}