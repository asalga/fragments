precision mediump float;
uniform vec2 u_res;
void main() {
  vec2 p = gl_FragCoord.xy;
  float d = u_res.x/(8./2.);

  float horiz = p.x + step(d/2., mod(p.y, d)) * d/2.;
  float i = step(d/2., mod(horiz, d));
  // float i = horiz;
  gl_FragColor = vec4(i,i,i,1.);
}