// 220
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
const float PI = 3.141592658;

float sdRing(vec2 p, float r, float w){
  return abs(length(p)- (r*.5)) - w;
}

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

void rot(inout vec2 p, float a){
  mat2 m = mat2(cos(a), sin(a),
                -sin(a), cos(a));
  p *= m;
}

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

void main(){
  vec2 uv = (gl_FragCoord.xy/u_res) * 2. -1.;
  float i;
  vec2 sz = vec2(0.25);
  float t = u_time * .125;

  float f = fract(t) * .5;
  vec2 scale = vec2(f) * 0.;

  // uv *= f;
  // uv -= 0.5;
  // uv *= f;
  sz = vec2(pow(f, 1./.707));

  float main = step(sdRect(uv, sz), 0.);
  // uv /= f;

  float _0 = step(sdRect(uv - vec2(0.5, 0.), vec2(sz.x/2., sz*1.)), 0.);

  i += main;
  // i += _0;

  gl_FragColor = vec4(vec3(i),1.);
}