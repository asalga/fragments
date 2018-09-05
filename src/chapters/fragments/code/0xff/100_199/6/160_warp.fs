// 16x - spin

precision highp float;

uniform vec2 u_res;
uniform float u_time;

float checker(vec2 c) {
  float col;
  float sz = 1.;
  float x = step(mod(c.x,sz), sz/2.);
  float y = step(mod(c.y,sz), sz/2.);
  if(x == y){return 0.8;}
  return x*y;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;

  float _dot = dot(p,p);
  vec2 uv = p/_dot;
  uv += u_time;

  float i = checker(uv);
  gl_FragColor = vec4(vec3(i), 1);
}