precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float i;
  float t = u_time;
  float w = .125;

  vec2 _p0 = mod(p - vec2(t, 0.), vec2(8.0, 0.));
  vec2 _p1 = mod(p + vec2(0., t+w), vec2(0.0, 8.));

  // top
  i += step(sdRect(_p0 - vec2(0., 1.-w), vec2(2.-w*2., w)), 0.);
  // * step(sdRect(p, vec2(1.-w*2.,1. )), 0.);

  // - step(sdRect(p -vec2(1.-w, 1.-w), vec2(w,w)), 0.);

  // right
  i += step(sdRect(_p1 - vec2(1.-w, 0.), vec2(0.125, 2.)), 0.)
  * step(sdRect(p, vec2(1., 1.-w*2.)), 0.);

  // - step(sdRect(p -vec2(1.-w, 1.-w), vec2(w,w)), 0.);

  gl_FragColor = vec4(vec3(i),1.);
}