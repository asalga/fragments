// 63 - 
// rotate view
// rotate view smoothly
// investigate easing
// 
//
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define COS_30 0.8660256249
float sdTri(vec2 p, float s, float sc){
  vec2 a = abs(p);
  float distToSide = a.x * COS_30;
  float u = p.y * sc;
  return max(distToSide + u, -u) - s;
}
float sdCircle(vec2 p, float r){
  return length(p)-r;
}
vec2 skew(vec2 p, float x){
  vec2 retP = p;
  retP.x += retP.y*x;
  return retP;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;

  float circ = sdCircle(skew(p, .4), 0.25);
  float tri = sdTri(p, 0.25, 0.5);

  i = mix(tri, circ, fract(u_time));

  i = step(i, 0.);

  gl_FragColor = vec4(vec3(i),1.);
}