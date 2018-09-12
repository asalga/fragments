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

  float r = .5/len + t;
  float th = atan(p.y / p.x)/TAU;
  th *= 4.;

  vec2 uv = vec2(r,th);

  uv = fract(uv);

  vec3 col = texture2D(u_t0, uv).rgb;

  float fog = .1- (1./(length(p) * 200.));

  fog = pow( len, 2.4) * 2.;
  col *= fog;

  gl_FragColor = vec4(col, 1);
}
