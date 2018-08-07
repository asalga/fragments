precision mediump float;

uniform sampler2D u_t0;
uniform vec2 u_res;
uniform float u_time;

void main() {
  vec2 p = gl_FragCoord.xy / u_res;
  p.y = 1.0 - p.y;
  
  p.y += 0.5 * sin(u_time + p.x*1.);
  p.y = mod(p.y,1.);

  gl_FragColor = texture2D(u_t0, p);
}
