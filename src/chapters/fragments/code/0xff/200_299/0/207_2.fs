precision mediump float;

uniform vec2 u_res;
uniform float u_time;

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t =u_time;
  float s = 5.;

  vec2 o = vec2(1.);

  i  = cos(t + length(p) * s);
  i *= sin(t + length(p+o) * s);
  i *= sin(t + length(p-o) * s);
  i *= sin(t + length(p-vec2(-1., 1.)*o) * s);
  i *= sin(t + length(p-vec2(1., -1.)*o) * s);

  i = smoothstep(0.001, 0.0001, i);

  gl_FragColor = vec4(vec3(i),1.);
}