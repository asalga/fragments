// 160 - "Warp"

precision highp float;

uniform vec2 u_res;
uniform float u_time;

float checker(vec2 c) {
  float sz = .5;
  vec2 s = step(mod(c,sz), vec2(sz/2.));
  return s.x*s.y;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float sqLen = dot(p,p);
  vec2 uv = vec2(p/sqLen);
  uv.x += u_time;
  float i = checker(uv) * sqLen;
  gl_FragColor = vec4(vec3(i), 1);

  // debug
  // uv = fract(uv);
  // vec4 col = texture2D(u_t0, uv) * sqLen;
  // gl_FragColor = vec4(col.rgb, 1);
}