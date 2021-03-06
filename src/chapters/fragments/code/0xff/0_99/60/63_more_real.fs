// 63
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define COS_30 0.8660256249
#define PI 3.141592658
float sdTri(vec2 p, float s, float sc){
  vec2 a = abs(p);
  float distToSide = a.x * COS_30;
  float u = p.y * sc;
  return max(distToSide + u, -u) - s;
}
//gsgd.co.uk/sandbox/jquery/easing/jquery.easing.1.3.js
float easeInOutBack(float t, float b, float c, float d){
  float s = .0;//70158;
  if ((t /= d/2.) < 1.){
  	return c/2.*(t*t*(((s*=(1.525))+1.)*t - s)) + b;
  }
  return c/2.*((t-=2.)*t*(((s*=(1.525))+1.)*t + s) + 2.) + b;
}
float sdCircle(vec2 p, float r){
  return length(p)-r;
}
vec2 skew(vec2 p, float x){
  p.x+= p.y*x;
  return p;
}
float remap(float x ){
  return (x +1.)*.5;
}
float sdRect(vec2 p, vec2 size){// from book of shaders
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y), .0) + length(max(d,.0));
}
mat2 r2d(float a){
  return mat2(cos(a),sin(a),-sin(a),cos(a));
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float sz = .15;
  float t = u_time * 3.;
  vec2 colorInv = p;
  float modt = step(1., mod(t, 2.));
  float e = easeInOutBack( modt * fract(t), 0., 1., 1.);
  float offset = step(mod(t, 4.), 2.);
  colorInv *= r2d(e*PI + offset*PI);
  p *= r2d(e*PI + offset*PI);//rotate 'camera'
  e = easeInOutBack( modt * fract(t), 0., 1., 1.);
  p.y += .6*sin(t*PI/2.);// move up/down
  p *= r2d(t*4.);//spin in-place

  float circ = sdCircle(skew(p, .7), 0.25);
  float tri = sdTri(p-vec2(0.,sz/2.), sz, 0.5);
  float blendShape = (sin(t*PI/2.)+1.)/2.;
  i = smoothstep(0.01, 0.001, mix(tri, circ, blendShape));

  if(colorInv.y < 0.){i = 1. - i;}
  gl_FragColor = vec4(vec3(i, i, i),1.);
}