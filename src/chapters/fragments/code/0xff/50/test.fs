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
float line(vec2 p, vec2 a, vec2 b, float r) {
  vec2 pa = p - a, ba = b - a;
  float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
  return length( pa - ba*h ) - r;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;

  float div = 2.;
  float SZ = div/3.;

  mat2 rot = r2d(u_time*PI);
  mat2 rot2 = r2d((u_time+.4)*PI);
  vec2 orbiter = (p + vec2(0.5, 0.));  

  i += step(line(p, vec2(0.5,0.), p-(orbiter*rot), 0.01), 0.0);
  i += step(line(p, vec2(0.5,0.), p-(orbiter*rot2*2.), 0.01), 0.0);
  
  gl_FragColor = vec4(vec3(i),1.);
}