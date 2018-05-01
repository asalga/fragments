#ifdef GL_ES
  precision mediump float;
#endif

uniform vec2 u_res;

void main() {
  vec2 aspect = vec2(u_res.x / u_res.y, 1.);
  vec2 uv = gl_FragCoord.xy / u_res * 2. - 1.;
  uv *= aspect;
  vec3 col = vec3(step(distance(vec2(.0), uv), .5));
  gl_FragColor = vec4(col, 1.);
}