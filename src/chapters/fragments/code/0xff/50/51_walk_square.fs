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
  float t = u_time*4.;
  float modt = mod(t, PI/2.);

  float ground = step(rectSDF(p+vec2(0., .1), vec2(2., 0.1)), 0.);
  p.x += modt * PI/10.;
  p *= -r2d( modt + PI/2.);// rotate first
  p -= vec2(0.25);

  


  float i = step(rectSDF(p, vec2(0.25)), 0.);
  vec4 cube = vec4(vec3(i), 1.);

  // GRID
  vec2 pp = gl_FragCoord.xy;
  vec2 lineWidthInPx = vec2(1.);
  vec2 cellSize = vec2(50.);
  vec2 g = step(mod(pp, cellSize), lineWidthInPx);
  vec4 grid = vec4(vec3(g.x+g.y), 1.);

  gl_FragColor = ground+cube;
}



// void main(){
//   vec2 p = gl_FragCoord.xy;
//   vec2 lineWidthInPx = vec2(1.);
//   vec2 cellSize = vec2(50.);
//   vec2 i = step(mod(p, cellSize), lineWidthInPx);
//   gl_FragColor = vec4(vec3(i.x+i.y), 1.);
// }