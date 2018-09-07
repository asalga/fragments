// 160 - "Warp"

precision highp float;

uniform vec2 u_res;
uniform float u_time;
uniform sampler2D u_t0;

float checker(vec2 c) {
  float sz = .5;
  float x = step(mod(c.x,sz), sz/2.);
  float y = step(mod(c.y,sz), sz/2.);
  // if(x == y){return 0.8;}
  return x*y;
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