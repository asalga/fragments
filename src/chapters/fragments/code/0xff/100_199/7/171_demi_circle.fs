precision highp float;

uniform vec2 u_res;
uniform float u_time;

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

mat2 r2d(float a){
  float c = cos(a);
  float s = sin(a);
  return mat2(c,s,-s,c);
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float i;
  float t = u_time;
  float r = .45;

  vec2 c = vec2(.25);
  p = mod(p, c) - 0.5*c;

  p *= r2d(t);

  i = step(sdCircle(p, r), 0.);
  i -= step(sdRect(p - vec2(0., r), vec2(r)), 0.);

  gl_FragColor = vec4(vec3(i),1);
}