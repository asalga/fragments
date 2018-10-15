precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float PI = 3.141592658;
const vec2 tileSz = vec2(1.);
const float sz = 140.;

float sampleChecker(vec2 c) {
  float col;
  float sz = 0.25;

  float x = step(mod(c.x,sz), sz/2.);
  float y = step(mod(c.y,sz), sz/2.);

  if(x == y){return 1.;}
  return x*y;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;

  float warp = sin(fract(p.x * 2. + u_time) * PI);
  p.y += warp/5.;
  p.y += sin(u_time*2.)/10.;

  float i = sampleChecker(p);

  gl_FragColor = vec4(vec3(i),1);
}
