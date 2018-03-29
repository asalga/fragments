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

float wavycircle(vec2 p, float r){
  float theta = atan(p.y, p.x);
  vec2 pn = normalize(p);
  pn *= sin(theta * 15.)/10.0;
  float plen = length(p + pn);
  return 1. - step(r, plen) * step(0.5, length(p));
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * ((gl_FragCoord.xy / u_res) * 2. -1.);
  // vec2 off = p + vec2( cos(u_time*speed),  sin(u_time*speed) );
  float i = wavycircle(p, 0.5);
  gl_FragColor = vec4(vec3(i), 1.0);
}
