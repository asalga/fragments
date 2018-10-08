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

  gl_FragColor = vec4(vec3(i),1.);
}