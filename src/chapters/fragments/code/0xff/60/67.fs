precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p, float r){
  return length(p)-r;
}
float circle(vec2 p, float r){
  return step(sdCircle(p, r), 0.);
}

void main(){
  vec2 ar = vec2(1., u_res.y/u_res.x);
  vec2 p = ar * (gl_FragCoord.xy/u_res*2.-1.);
  float NumSpaces = 8.;
  vec2 origP = p;
  float r = .25;
  float time = u_time*.5;

  // float idx = (((p.x+1.)/2.)/NumSpaces)*10.;
  float yIdx = (((p.y+1.)/2.)/2.)*10.;

  float dir = step(p.y,0.)*2.-1.;
  p.y = mod(p.y, 1.);

  // p = mod(p, vec2(2./NumSpaces, 1.));
  
  
  // center Y
  p.y -= .5;
  
  // p.x -= r/2.;
  p.x -= mod(time*dir,2.)-1.;
  
  // offset X to create staggerig pattern
  // p.x += idx/1.;

  float i = circle(p,r);

  // if(gl_FragCoord.y/u_res.y > 0.5){
  //   i = 1. - i;
  // }
  gl_FragColor = vec4(vec3(i),1);
}
