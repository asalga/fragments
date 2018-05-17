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
  float u = p.y * sc * 1.;
  return max(distToSide + u, -u) - s;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float count = 3.;
  float sliceSize = PI/count;
  float CC = 180./(count);
  float theta = atan(p.y,p.x);
  float lenp = length(p);
  float i;
  float T = abs(sin(1.8))*2.;
  // i = step(cSDF(p, .8), 0.);

  float TH = theta;// + ((1.-lenp));

  //                   0..TAU      0..1   [0,1,..c-1]
  float idx = floor( ((TH+PI) / PI) * count);

  float snapped = -PI + (idx * sliceSize) + sliceSize/2.;

  // Test
  // snapped += (1.-lenp);

  vec2 v = vec2(cos(snapped), sin(snapped)) * .8;

  // remap 0..2 to 1..-1
  float triRot = (idx-1.) * -(DEG_TO_RAD*CC);
  
  p = (p-v);// * r2d(triRot);
  // p *= -r2d(1.-lenp);
  p *= r2d(triRot);

  // move outwards
  // p = p - (v*r2d(triRot) * T); 
  // p += (1.-lenp) * (op*r2d(0.4));
  // i = (triSDF(p, .13, .5));
  i = step(((triSDF(p, .3, .5))), 0.);
  gl_FragColor = vec4(vec3(i,i,i),1.);
}