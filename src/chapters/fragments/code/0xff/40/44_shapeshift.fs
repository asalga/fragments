// Timeshifting
precision mediump float;
#define PI 3.141592658
#define SZ .35
uniform vec2 u_res;
uniform float u_time;
mat2 r2d(float a){
  return mat2(cos(a),sin(a),sin(a),-cos(a));
}
float ringSDF(vec2 p, float r, float w){
  return abs(length(p) - (r*.5)) - w;
}
float rSDF(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x, d.y), 0.) + length(max(d,0.));
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time*4.;
  float theta = sin(t)*PI/8.;
  vec2 bPos = (p+vec2(sin(-theta)*a.y,cos(theta)-1.))
  			  * r2d(theta);
  float c = ringSDF(bPos,SZ*2., .02);
  float r = rSDF(bPos,vec2(SZ));
  float i = smoothstep(0.01, 0.001,mix(c,r,(sin(t)+1.)/2.));
  p.y -= a.y;
  p *= r2d(theta);
  i += smoothstep(0.01, 0.001, rSDF(p, vec2(0.005, a.y-0.3)));
  gl_FragColor = vec4(vec3(i), 1.);
}