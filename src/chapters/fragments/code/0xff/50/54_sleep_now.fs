// 54 - "sleep now"
precision mediump float;
#define PI 3.141592658
uniform vec2 u_res;
uniform float u_time;
float squareSDF(vec2 p, float l){
  vec2 a = abs(p);
  return max( abs(a.x-l), abs(a.y-l));
}
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
float rectSDF(vec2 p, vec2 size) {//book of shaders
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y),.0)+length(max(d,.0));
}
float cSDF(vec2 p, float r){
  return length(p) - r;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.); 
  float div = .25;
  float SZ = div/2.;
  vec2 op = p;
  float plen = length(p);
  float rings = (sin(length(p*5.)+u_time*4.)+1.)/2.;
  p += div/2.;// place a circle in the middle
  p = mod(p, div) - SZ;
  float c = cSDF(p, SZ); 
  float r = rectSDF(p*r2d(u_time*PI.+plen*5.),vec2(SZ*.707));
  float i = smoothstep(.01,.001,mix(c,r,rings));
  gl_FragColor = vec4(vec3(i),1.);
}