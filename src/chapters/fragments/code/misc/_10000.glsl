precision mediump float;
uniform vec2 u_res;

void main() {
  vec2 uv = gl_FragCoord.xy / u_res;
  float h = step(0.4, (mod((uv.x * -1.) * 20., 1.)));
  float v = step(0.4, (mod(uv.y * 20., 1.)));
  gl_FragColor = vec4(vec3(v*h), 1.0);
}