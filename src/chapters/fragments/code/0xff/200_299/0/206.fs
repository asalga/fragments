precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdCircle(in vec2 p, in float r){
  return length(p)-r;
}

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time;
  float sz = 0.125 + (sin(t*0.25)+1.)/13.;

  p += vec2(1.5+cos(t*0.25), sin(t*0.25)-1.5);

  float len = length(p-1.)*15.;

  vec2 c = vec2(sz);
  p = mod(p, c) - 0.5*c;

  float b = sdRect(p, vec2(sz/2.));
  float ci = sdCircle(p, sz/5.);

  float amt = (sin(t*4. + len)+1.)/2.;
  i = mix(ci, b, amt);
  i = step(i, 0.);

  gl_FragColor = vec4(vec3(i),1.);
}