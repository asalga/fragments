precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p, float r){
  return length(p)-r;
}
float circle(vec2 p, float r){
  return smoothstep(0.01, 0.001, sdCircle(p, r));
}

void main(){
  vec2 ar = vec2(1., u_res.y/u_res.x);
  vec2 p = ar * (gl_FragCoord.xy/u_res*2.-1.);
  float NumSpaces = 8.;
  vec2 origP = p;
  float r = .4;
  float time = u_time*2.;

  float idx = (((p.x+1.)/2.)/NumSpaces)*10.;

  float dir =step(p.y,0.)*2.-1.;
  p.y = mod(p.y, 1.);

  p = mod(p, vec2(2./NumSpaces, 1.));
  p.y -= .5;  // center Y
  p.x -= idx/2.;

  vec2 p0 = p;
  // move across canvas and account for radius
  p.x  -= mod(time*dir,2.+r*2.)-1.-r;
  p0.x -= mod((time+dir)*dir,2.+r*2.)-1.-r;

  float i =  circle(p,r) + circle(p0,r);
  if(gl_FragCoord.y/u_res.y > 0.5){i = 1. - i;}
  gl_FragColor = vec4(vec3(i),1);
}
