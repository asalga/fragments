precision mediump float;

uniform vec2 u_res;

void main() {
  vec2 uv = gl_FragCoord.xy / u_res;
  vec3 col = vec3(step(.5, uv.x));
  gl_FragColor = vec4(col, 1.);
}