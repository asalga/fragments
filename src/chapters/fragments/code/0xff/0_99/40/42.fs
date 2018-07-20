precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define TAU 2.*PI
#define COS_30 .8660256249
#define DEG_TO_RAD PI/180.

float cSDF(vec2 p, float r){
  return length(p)-r;
}
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
float triSDF(vec2 p, float s, float sc){
  vec2 a = abs(p);
  float distToSide = a.x * COS_30;
  float u = p.y * sc;
  return max(distToSide + u, -u) - s;
}
float t(vec2 p, vec2 v, float sz){
  return step(((triSDF(p+v, sz, .5))), 0.);
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float count = 1.5;
  float sliceSize = PI/count;
  float CC = 180./(count);
  float theta = atan(p.y,p.x);
  float lenp = length(p);
  float i;
  float T = abs(sin(1.8))*2.;

  float sz = 0.13;

  float t0 = t(p, vec2(0., -sz*2.), sz);
  float ct = t(p*vec2(1., -1.), vec2(0., -sz*2.), sz);
  
  vec2 rest = vec2(sz*2.3, sz*2.);
  vec2 pp = 2.*rest + -normalize(rest) *  
                     mod(u_time, 1.) *
                     step(mod(u_time, 1.), 0.5);

  float t1 = t(p, pp, sz);
  float t2 = t(p, vec2(-sz*2.3, sz*2.), sz);

  // vec p1 = p - (v*r2d(triRot) * .013); 
  i = t0 + t1 + t2;// + ct;  

  gl_FragColor = vec4(vec3(i,i,i),1.);
}