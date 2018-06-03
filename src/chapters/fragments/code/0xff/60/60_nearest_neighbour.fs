// 55 - "nearest neighbour"
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
float cSDF(vec2 p, float r){
  return length(p)-r;
}
float ringSDF(vec2 p, float r, float w){
  return abs(length(p)- (r*.5)) - w;
}
// book of shaders
float capsule(vec2 p, vec2 a, vec2 b, float r) {
  vec2 pa = p - a, ba = b - a;
  float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
  return length( pa - ba*h ) - r;
}
void main(){
  vec2 ar = vec2(1., u_res.y/u_res.x);
  vec2 p = ar * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time*4.;
  float yIdx = 1.+floor(((p.y+ar.y)/(ar.y*2.))*3.);
  float xIdx = (t*yIdx) + 4.-floor((p.x+1.)*2.);
  float div = .5;
  float SZ = div/1.;
  p = mod(p,div);
  p -= div/2.;
  
  mat2 rot = r2d(t);

  mat2 orbCenterRot = r2d(xIdx);
  mat2 orbRightRot = r2d(xIdx+1.  );
  mat2 orbLeftRot =  r2d(xIdx-1. );

  mat2 rotLeft = r2d(t);

  // vec2 off = vec2(div/6., 0.);

  vec2 orbCenterPt = (p * r2d(-xIdx)) - vec2(SZ/2., 0.);
  vec2 orbRightPt = (p * orbRightRot) + vec2(SZ/2., 0.);

  vec2 orbCenterLine = vec2(SZ/2., 0.) * r2d(xIdx);
  vec2 orbRightPtLine = (vec2(SZ/2., 0.) * orbRightRot) - vec2(SZ, 0.);
  vec2 orbLeftPtLine = (vec2(SZ/2., 0.) * orbLeftRot) + vec2(SZ, 0.);

  vec2 circPos = (vec2(SZ/2., 0.) * rot);// + vec2(SZ/10., 0.);
  
  // vec2 rightPt = (vec2(SZ/2., 0.) * rotRight) - vec2(SZ, 0.);
  // rightPt = vec2(SZ*2., 0.);
  vec2 leftPt = (vec2(-SZ/2., 0.)*rotLeft) + vec2(SZ, 0.);
  
  float ring = step(ringSDF(p, SZ, 0.01), 0.);

  float red = step(capsule(p, orbCenterLine, orbRightPtLine, 0.01), 0.);
  float green;
  float blue= step(capsule(p, orbCenterLine, orbLeftPtLine, 0.01), 0.);
  i = red + green + blue;
  gl_FragColor = vec4(vec3(i),1.);
}