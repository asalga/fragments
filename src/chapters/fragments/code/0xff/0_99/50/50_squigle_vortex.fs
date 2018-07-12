precision mediump float;
uniform vec2 u_res;
uniform vec2 u_tracking;
uniform float u_time;

#define PI 3.141592658
#define COUNT 2.
#define DEG_TO_RAD PI/180.

float cSDF(vec2 p, float r){
  return length(p)-r;
}
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
float circInter(vec2 p, float r, float i){
  float a = smoothstep(0.01, 0.001,cSDF(p+vec2(0.,-i), r));
  float b = smoothstep(0.01, 0.001,cSDF(p+vec2(0.,i), r));
  return a*b;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);  
  float tt = u_time;
  vec2 spiralPos = p;
  spiralPos *= r2d(-PI/8.);
  // spiralPos *= r2d(0.4);
  float th = atan(spiralPos.y/spiralPos.x)/PI;
  float len = length(spiralPos);
  
  float spin, spiral, squiggle;
  vec2 squigglyTrans = (p+vec2(-0.4, 0.));// * r2d(-0.);  
  
  float shfit = cos(tt + p.x*(+0.5)*10.)/10. * (.1-length(p));
  float tr = ((2.)-1.)/3.;
  vec2 _t1 = vec2(-0.3, shfit);
  squiggle += circInter(squigglyTrans + _t1, 1.0, 0.85);

  th += step(len,spin) * pow((spin-len),1.);
  float ca = floor(th * COUNT)/COUNT;
  spiral += smoothstep(0.21, 0.215,th-ca);
  spiral *= step(cSDF(p, 0.5), 0.);

  float i = squiggle + spiral;
  gl_FragColor = vec4(vec3(i), 1.);
}