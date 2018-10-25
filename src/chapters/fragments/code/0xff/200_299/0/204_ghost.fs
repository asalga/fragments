precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

float sdCircle(in vec2 p, in float r){
  return length(p) - r;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float i = 0.;

  i = sin(p.x*10.);
  if(i > 0.){
    i = 1.;
  }

  gl_FragColor = vec4(vec3(i),1.);
}