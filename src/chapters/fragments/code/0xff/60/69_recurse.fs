precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p, float r){
  return length(p)-r;
}
float circle(vec2 p, float r){
  return step(sdCircle(p,r), .0);
}
float sdRing(vec2 p, float r, float w){
  return abs(length(p) - (r*.5)) - w;
}
float ring(vec2 p, float r, float w){
  return step(sdRing(p, r, w), 0.);
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float time = mod(u_time*.5, 1.);
  float i;
  float strokeWeight = 0.2;
  float r = 1.;

  // time = mod( time, 2.);
  // time = 1./time;
  // p.y += time/1.25;
  // r += time;
  p /= 1.+ time;
  // r += time;
  strokeWeight = 0.01;
  vec2 v = vec2(0., -1. + time);

  // i += ring(p+vec2(0., time), 2.*r, strokeWeight);
  i += ring(p+v, 1.*r, strokeWeight);

  vec2 v2 = vec2(0., -1.5 + time/2.);
  gl_FragColor = vec4(vec3(i),1.);
}


  // i += ring(p+v2, 1.5*r, strokeWeight);
  // i += ring(p+vec2(0., .8)*r, .25*r, strokeWeight);
  // i += ring(p+vec2(0., -.8)*r, .25*r, 0.01);