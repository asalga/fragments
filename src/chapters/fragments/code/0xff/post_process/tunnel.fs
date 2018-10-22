precision mediump float;

uniform sampler2D u_t0;
uniform vec2 u_res;
uniform float u_time;

const float PI = 3.141592659;
const float TAU = PI*2.;

void main() {
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  // p.y = 1.0 - p.y;
  float t = u_time * .25;
  float len = length(p);

  float r = .5/len + t*2.;
  float th = atan(p.y / p.x)/TAU + t/2.;
  th *= 2.;

  vec2 uv = vec2(r,th);

  uv = fract(uv);

  vec3 col = texture2D(u_t0, uv).rgb;

  float fog = .1- (1./(length(p) * 100.));

  fog = pow( len, 2.4) * 1.;
  col *= fog;

  gl_FragColor = vec4(col, 1);
}
