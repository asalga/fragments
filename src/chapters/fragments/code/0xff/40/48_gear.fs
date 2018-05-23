// 47 - gears 
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define TAU 2.*PI
#define COS_30 .8660256249
#define DEG_TO_RAD (PI/180.)
#define COUNT 4.
#define SPACING .7
#define SLICE_SIZE (PI/COUNT)
float expo (float t, float b, float c, float d) {
  t /= d/2.;
  if (t < 1.){
   return c/2. * pow( 2., 10. * (t - 1.) ) + b;
 }
  t--;
  return c/2. * ( -pow( 2., -10. * t) + 2. ) + b;
}
float cSDF(vec2 p, float r){
  return length(p)-r;
}
float ringSDF(vec2 p, float r, float w){
  return abs(length(p) - r*.5) - w;
}
//from book of shaders
float rectSDF(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y), .0) + length(max(d,.0));
}
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
float tooth(vec2 p, float s, float sc){
  vec2 a = abs(p);
  float distToSide = a.x * COS_30;
  float u = p.y * sc * 1.;
  float tri = max(distToSide + u, -u) - s;
  float rect = rectSDF(p-vec2(0., -.15), vec2(s*2.5,s*1.3));
  return step(tri,0.) * step(rect, 0.);
}
float gear(vec2 p, float rad){
  // 2 would give us no overlap, but we need the teeth to 
  // cross over so use a smaller number
  p *= rad * 1.8;
  float theta = atan(p.y,p.x);
  float i;
  //                   0..TAU   ->   0..1  ->   [0,1,..c-1]
  float idx = floor(( (theta+PI) / PI) * COUNT);
  float snapped = -PI + (idx * SLICE_SIZE) + SLICE_SIZE/2.;  
  
  // ring is not worth the trouble?
  float circ = step(cSDF(p, .8), 0.);
  circ -= step(cSDF(p, 0.6), 0.);

  float lines = step(rectSDF(p * r2d(-snapped ), vec2(0.7, 0.04)), 0.);
  i = lines + circ;

  p -= vec2(cos(snapped), sin(snapped));// away from center
  p *= r2d(-snapped + PI/2.);// rotate the points away from the center 
  i += tooth(p, .1, .25);

  return i;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);  
  float i;
  float mt = PI/2.;
  float div = 1.5;// divisions per side
  float t = mod(u_time*5., mt);
  t = expo(t,.0, mt/2., mt);
  p.x += u_time/2.;
  p.x = mod(p.x, div);
  vec2 c = vec2(-div/2., 0.);
  float halfSlice = SLICE_SIZE/2.;

  i = step(cSDF(p, div/2.), 0.);
  float scaleRad = 1./div * 2.;

  float r = gear(p * r2d(t + halfSlice), scaleRad);  // center
  float g = gear((p + (c*2.)) * r2d(t+halfSlice), scaleRad); // right
  float b = gear((p + c) * r2d(-t), scaleRad); // right
  i = r + g+ b;
  gl_FragColor = vec4(vec3(i),1.);
}