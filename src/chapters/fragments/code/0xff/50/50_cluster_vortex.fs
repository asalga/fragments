precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define COS_30 .8660256249
#define DEG_TO_RAD PI/180.
#define COUNT 3.
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
float triSDF(vec2 p, float s, float sc){
  vec2 a = abs(p);
  float distToSide = a.x * COS_30;
  float u = p.y * sc * 1.;
  return max(distToSide + u, -u) - s;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float sliceSize = PI/COUNT;
  p *= r2d(u_time);
  float theta = atan(p.y,p.x);
  float lenp = length(p);
  float TH = theta - (1.-lenp);
  float idx = floor( ((TH+PI) / PI) * COUNT); 
  float snapped = -PI + (idx * sliceSize) + sliceSize/2.;
  vec2 v = vec2(cos(snapped), sin(snapped)) * -1.;
  float triRot = (idx-1.) * -(DEG_TO_RAD*(180./COUNT));
  p = (p-v) * r2d(triRot) * r2d(lenp);
  float i = triSDF(p, .3, .5);
  gl_FragColor = vec4(vec3(i),1.);
}