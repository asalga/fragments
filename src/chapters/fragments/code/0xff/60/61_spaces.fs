precision mediump float;
#define PI 3.14159
uniform vec2 u_res;
uniform float u_time;
#define DEPTH 5
float sdCircle(vec2 p, float r){
  return length(p) - r;
}
float circle(vec2 p, float r){
  return 1.-step(0.,sdCircle(p, r));
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
  float i;
  float thickness = 0.005;
  i += ring(p,2.,thickness);

  float sz = 1.;
  for(int it = 0; it < DEPTH; it++){
    vec2 _p = mod(p + vec2(0.,a.y/2.), vec2(sz,0.));
    vec2 c = vec2(-sz/2., -a.y/2.);
    i += ring(_p+c,sz,thickness);
    sz *= .5;
  }

  gl_FragColor = vec4(vec3(i),1);
}