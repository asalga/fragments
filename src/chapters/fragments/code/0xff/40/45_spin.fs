precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define C 4.
float ringSDF(vec2 p, float r, float w){
  return abs(length(p) - (r*.5)) - w;
}
mat2 r2d(float a){
  return mat2(cos(a),sin(a),sin(a),-cos(a));
}
void main(){
  vec2 a = vec2(1.,u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time*10.;
  vec2 idx = vec2(floor(p*C))/2.;
  p = mod(p, 1./C);
  p /= (1./C)*.5;
  p -= vec2(1.); //center
  p/= (sin(t/1. - idx.x*1.2 + idx.y*2.)+1.5)/2.;
  float i = smoothstep(.1, .01, ringSDF(p, 1., .04));
  p *= r2d(t*1.2 + idx.x + idx.y);
  i += smoothstep(0.01, 0.001, ringSDF(p+vec2(.33), .3, .2)); 
  gl_FragColor = vec4(vec3(i),1.);
}