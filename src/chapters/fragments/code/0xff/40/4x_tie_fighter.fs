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
  

  float count = 2.;
  float CC = 180./count;
  float sliceSize = PI/count;
  float theta = atan(p.y,p.x);
  float i;

  //                   0..TAU   ->   0..1  ->   [0,1,..c-1]
  float idx = floor(( (theta+PI) / PI) * count);

  float snapped = -PI + (idx * sliceSize) + sliceSize/2.;

  // 
  float triRot = -snapped + PI/2.;

  // move away from center
  vec2 v = vec2(cos(snapped), sin(snapped));
  p -= v;

  // rotate the points away from the center
  p *= r2d(triRot);
  
  // p = (p + vec2(0, 1.));
  // p *= r2d(triRot + PI + 0.8);

  // i = triSDF(p, .13, .5);
  i += step(((triSDF(p, .1, .2))), 0.);
  gl_FragColor = vec4(vec3(i,i,i),1.);
}