precision mediump float;

uniform sampler2D u_t0;
uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

void main() {
  vec2 p = gl_FragCoord.xy / u_res;

  p.y = 1.0 - p.y;

  // divide up into sections

  float cnt = 80.;
  vec2 cell = floor(p*cnt)/cnt;

  float intensity = (texture2D(u_t0, cell) ).x;
  float sz = (1./cnt)/2.;
  vec2 c = vec2(1./cnt);
  vec2 rp = mod(p, c)-c*0.5;

  float test = intensity*sz;

  float i = step(sdCircle(rp, test ),0.);
  gl_FragColor = vec4(vec3(i),1);
}
