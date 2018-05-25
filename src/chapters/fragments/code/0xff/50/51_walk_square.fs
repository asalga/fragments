precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define HALF_PI PI/2.
#define BOLT(pos) smoothstep(0.01, 0.0004,rectSDF(p + pos, vec2(0.02)))
float rectSDF(vec2 p, vec2 size) {  //from book of shaders
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y), .0) + length(max(d,.0));
}
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
float box(vec2 p){
  float mainBox = smoothstep(0.01, 0.0004, rectSDF(p, vec2(0.25)));
  float bolt0 = BOLT(vec2(.2));
  float bolt1 = BOLT(vec2(-.2));
  float bolt2 = BOLT(vec2(-.2,.2));
  float bolt3 = BOLT(vec2(.2,-.2));
  return mainBox - bolt0 - bolt1 - bolt2 - bolt3;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time * 2.;
  float modT = mod(t, 0.5);
  float groundCol = 1.- smoothstep(0.1, 0.08, mod((p.x+ p.y*2.) + modT, 0.25));
  float ground = groundCol * step(rectSDF(p+vec2(modT,.1), vec2(2.,.1)), 0.);
  float topBar = step(rectSDF(p, vec2(2.,0.01)), 0.);
  float bottomBar = step(rectSDF(p+vec2(0., 0.2), vec2(2.,0.01)), 0.);
  p.x += modT -.25;
  p *= -r2d(modT*PI + HALF_PI);
  p -= vec2(0.25);
  float i = ground + topBar + bottomBar + box(p);
  gl_FragColor = vec4(vec3(i), 1.);
}