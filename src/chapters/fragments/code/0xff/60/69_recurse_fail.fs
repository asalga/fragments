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
  float time = mod(u_time*1., 2.);
  float i;
  float strokeWeight = 10.;
  float r = 1.;
  float scale = 1.+time;
  p.y += 0.5;
  p /= scale;
  
  vec2 v0 = vec2(0.,  time);
  vec2 v1 = vec2(0., -1. + time);
  vec2 v2 = vec2(0., -2. + time);

  strokeWeight = (1./u_res.x / p.y) * strokeWeight / scale;

  i += ring(p+v0,  1.*r, strokeWeight);
  i += ring(p+v1, .85*r, strokeWeight);
  // i += ring(p+v2, .25*r, strokeWeight);

  gl_FragColor = vec4(vec3(i),1.);
}