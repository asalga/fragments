precision mediump float;

#define PI 3.141592658

uniform vec2 u_res;
uniform float u_time;

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}
float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;
  return fract( sin(x+y) * 23454.);
}

void rot(inout vec2 p, in float a){
  float c = cos(a);
  float s = sin(a);
  p *=mat2(c,-s,s,c);
}

void main(){
  vec2 p = ((gl_FragCoord.xy/u_res) * 2. -1.);
  float t = u_time * 1.;

  float r = PI/2. * floor(mod(t, 4.));

  rot(p, -r);
  float i = step(sdRect(p-vec2(0.5), vec2(0.5, 0.5)), 0.);

  gl_FragColor = vec4(vec3(i), 1.);
}