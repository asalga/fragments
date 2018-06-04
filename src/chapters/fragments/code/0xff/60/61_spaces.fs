precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define DEPTH 7
float circle(vec2 p, float r){
  return smoothstep(0.01, 0.001, length(p)-r);
}
float sdRing(vec2 p, float r, float w){
  return abs(length(p)- r*.5) - w;
}
float ring(vec2 p, float r, float w){
  return smoothstep(0.01, 0.001, sdRing(p,r,w));
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float thickness = 0.0015;
  float i = 1.-circle(p, 1.);
  float sz = 1.;
  float dir = 1.;
  for(int it = 0; it < DEPTH; it++){
    vec2 _p = mod(p + vec2(0.,a.y/2.), vec2(sz,0.));
    vec2 c = vec2(-sz/2., -a.y/2.);
    i += ring(_p+c,sz,thickness);
    sz *= .5;
    dir *= -1.;
  }
  gl_FragColor = vec4(vec3(i),1);
}