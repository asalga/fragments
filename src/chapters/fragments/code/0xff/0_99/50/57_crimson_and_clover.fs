// 59 - "Crimson & Clover" 
// alternating movement idea from bookofshaders marching_dots
precision mediump float;
#define TAU 3.141592658*2.
#define PI TAU/2.
uniform vec2 u_res;
uniform float u_time;
float sdCircle(vec2 p, float r){
  return length(p) - r;
}
float circ(vec2 p, float r){
  return smoothstep(0.01, 0.001, sdCircle(p, r));
}
mat2 r2d(float a){
  return mat2(cos(a),sin(a),-sin(a),cos(a));
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time*2.;
  float sz = 0.25/2.;
  p *= r2d(u_time);
  float yTime = t * a.y * step(mod(t, 1.),.5);
  float xTime = t * step(.5, mod(t,1.));

  p.y -= step(mod(p.x, 1.), 0.5) * yTime;
  p.x -= step(mod(p.y, a.y), a.y/2.) * xTime;
  vec2 np = mod(p, vec2(.5,a.y/2.))-vec2(sz*2., sz*1.6*2.);
  
  float i;
  for(float it=0.;it<3.;it++){
  vec2 trans = vec2(0., 1.);
  i+=circ(np*r2d(-u_time)+(.1*trans*r2d(TAU/3.*it)),sz);
  }  
  vec3 col = i * vec3(.5, 0.07, 0.11);
  gl_FragColor = vec4(col,1.);
}