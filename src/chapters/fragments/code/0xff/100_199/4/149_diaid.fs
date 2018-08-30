// 148 - "diaid"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
const float PI = 3.141592658;

mat2 r2d(float a){
  return mat2(cos(a),sin(a),-sin(a),cos(a));
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  p = mod(p, vec2(0.5))-vec2(.25);
  p *= r2d(PI/4.);
  vec2 shape = mod(abs(p*4.) - u_time * 0.5, vec2(0.5));
  shape = step(shape, vec2(0.25));
  shape *= step(abs(p.yx), abs(p.xy));
  gl_FragColor = vec4(vec3(shape.x + shape.y),1);
}

// working code:
// float vert1 = mod(abs(p.x) - t, c);
// vert1 = step(vert1, 0.125);
// vert1 *= step(abs(p.y), abs(p.x));

// float horiz1 = mod(abs(p.y) - t, c);
// horiz1 = step(horiz1, 0.125);
// horiz1 *= step(abs(p.x), abs(p.y));
