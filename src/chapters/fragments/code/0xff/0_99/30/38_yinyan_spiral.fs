precision mediump float;
uniform vec2 u_res;
uniform float u_time;
float easeCustom(float t) {
  return 2.*t*t;
}
float cSDF(vec2 p, float r){
  return length(p)-r;
}
float sc(vec2 p, vec2 v){
  return smoothstep(.01,.001,cSDF(p-v,.15));
}
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.); 
  float t = u_time;
  p *= easeCustom(sin(t))/1.8 * r2d(t*2.);
  float theta = atan(p.y,p.x)/3.141592658;
  float r = length(p)/50.;
  float s = mod(r+theta/10.,1.);
  vec2 v = vec2(0.,.8);
  float i = step(.1, s) + sc(p,v) - sc(p,-v);
  gl_FragColor = vec4(vec3(i),1.);
}