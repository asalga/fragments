// 47 brick shift
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define BRICK_H .2
#define BRICK_W BRICK_H * 2.
//from book of shaders
float rectSDF(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y), .0) + length(max(d,.0));
}
//from book of shaders
float random (vec2 st) {
  return fract(sin(dot(st.xy,vec2(12.9898,178.233)))*43758.5453123);
}
float bricks(vec2 p, vec2 sz, float morterSz){
  float halfXOffset = (step(mod(p.y, sz.y*2.), sz.y)*2. -1.) * u_time;
  float x = step(mod(p.x + halfXOffset, sz.x), sz.x-morterSz);
  float y = step(mod(p.y, sz.y), sz.y-morterSz);
  return x*y;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  p.y += u_time;
  p.x += p.y*2.;
  float i = bricks(p, vec2(BRICK_W, BRICK_H), 0.12 );
  gl_FragColor = vec4(vec3(i),1.);
}