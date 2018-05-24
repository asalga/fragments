precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
//from book of shaders
float rectSDF(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y), .0) + length(max(d,.0));
}
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time*8.;
  float modt = mod(t, PI/2.);
  float i;

  float groundCol = step(mod( (p.x+p.y*2.) + t/3., 0.25), .25/2.);
  float ground = groundCol * step(rectSDF(p+vec2(0.,.1), vec2(2.,.1)), 0.);
  float topBar = step(rectSDF(p, vec2(2.,0.01)), 0.);
  float bottomBar = step(rectSDF(p+vec2(0., 0.2), vec2(2.,0.01)), 0.);
  ground +=topBar + bottomBar;

  p.x += modt * PI/10.;
  p *= -r2d( modt + PI/2.);
  p -= vec2(0.25);
  
  float box = step(rectSDF(p, vec2(0.25)), 0.);
  i += ground + box;
  gl_FragColor = vec4(vec3(i), 1.);
}