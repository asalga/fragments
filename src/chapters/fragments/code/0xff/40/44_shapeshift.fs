// Shapeshifting & timetravelling
precision mediump float;
#define PI 3.141592658
#define E 0.001
#define SZ .35
uniform vec2 u_res;
uniform float u_time;
mat2 r2d(float a){
  return mat2(cos(a),sin(a),sin(a),-cos(a));
}
float cSDF(vec2 p, float r){
  return length(p) - r;
}
float rSDF(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x, d.y), 0.) + length(max(d,0.));
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time*4.;
  float theta = sin(t)*PI/8.;
  vec2 bPos = vec2(sin(-theta)*a.y,cos(theta)-1.);
  vec2 bob = (p + bPos) * r2d(theta);
  float c = cSDF(bob, SZ);
  float r = rSDF(bob, vec2(SZ, SZ));

  float state = (sin(t)+1.)/2.;
  i = smoothstep(0.01, 0.001, mix(c,r,state));

  p.y -= a.y;
  p *= r2d(theta);
  i += step(rSDF(p, vec2(0.01, a.y)), 0.);
  gl_FragColor = vec4(vec3(i), 1.);
}