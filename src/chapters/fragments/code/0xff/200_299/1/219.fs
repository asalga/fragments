// 219
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

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float i;
  float t = u_time;

  float a = step(sdRing(p, 0.5, 0.002), 0.);
  float b = step(sdRing(p, 0.8, 0.002), 0.);
  float c = step(sdRing(p, 1.5, 0.002), 0.);

  vec2 _ap = p;
  rot(_ap, 203. + t*1.5);
  float _a = 1.-smoothstep(0., 0.012, sdCircle(_ap + vec2(0., 0.25), 0.02));
  _a -= step(sdCircle(_ap+ vec2(0., 0.25), 0.024), 0.)*2.;

  vec2 _bp = p;
  rot(_bp, t*.58);
  float _b;
  _b = 1.-smoothstep(0., 0.012, sdCircle(_bp + vec2(0., 0.75), 0.027));
  _b -= step(sdCircle(_bp+ vec2(0., 0.75), 0.03), 0.)*2.;

  vec2 _cp = p;
  rot(_cp, PI * 8.4 + t);
  float _c = 1.-smoothstep(0., 0.012, sdCircle(_cp + vec2(0., 0.4), 0.025));
  _c -= step(sdCircle(_cp+ vec2(0., 0.4), 0.03), 0.)*2.;

  i = b + a + c;
  i += _a + _b + _c;


    vec2 pos = p;
    rot(pos, t*.5);
    float at = atan(pos.y,pos.x);
    float f = sin(at*13.);

    i += f * step(sdCircle(pos, 0.1), 0.);

  gl_FragColor = vec4(vec3(i),1.);
}