// 165 - "Absolute Sin"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
const float PI = 3.141592658;

float sdLine(vec2 p, vec2 a, vec2 b, float r) {
  vec2 pa = p - a, ba = b - a;
  float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
  return length( pa - ba*h ) - r;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float i;
  float t = u_time;
  float thic = 0.03;
  float CNT = 10.;
  vec2 cell = floor(p*CNT);
  vec2 c = vec2(1./CNT, 0.);
  vec2 p1 = mod(p, c)-c*0.5;

  p1 *= 1.2;

  float h1 = abs(sin(t + cell.x/5.)) * 0.65;
  float h2 = abs(sin(t + cell.x/5. + PI/2.)) * 0.65;
  float h3 = abs(sin(t + cell.x/5.)) * 0.65;
  float h4 = abs(sin(t + cell.x/5. + PI/2.)) * 0.65;

  i += step(sdLine(p1+vec2(0., 0.5), vec2(0., -h1), vec2(0., h1), thic), 0.);
  i += step(sdLine(p1-vec2(0., 0.5), vec2(0., -h2), vec2(0., h2), thic), 0.);

  i += step(sdLine(p1-vec2(0., 1.5), vec2(0., -h3), vec2(0., h3), thic), 0.);
  i += step(sdLine(p1-vec2(0., -1.5), vec2(0., -h4), vec2(0., h4), thic), 0.);

  gl_FragColor = vec4(vec3(i),1.);
}