precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define TAU 2.*PI
#define COS_30 0.8660256249
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
float triSDF(vec2 p, float s, float sc){
  vec2 a = abs(p);
  float distToSide = a.x * COS_30;
  float u = p.y * sc * 0.6;
  return max(distToSide + u, -u) - s;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float count = 6.;  
  float sliceSize = TAU/count;
  float theta = atan(p.y,p.x);
  float idx = floor( ((theta+PI) / TAU) * count);
  float snapped = -PI + (idx * sliceSize) + sliceSize/2.;
  vec2 v = vec2(cos(snapped), sin(snapped)) * 0.8;

  // need to rotate triangle a certain angle
  // depending on what section they are in.
  p-=v;

  // i = step(udRoundBox(p, vec2(.01, .01), 0.05), 0.);
  i = step(triSDF(p,0.05, 0.4), 0.);
  // i = triSDF(p,0.15, 0.5);

  // float TEST = (snapped+PI)/TAU;
  gl_FragColor = vec4(vec3(i,i, i),1.);
}