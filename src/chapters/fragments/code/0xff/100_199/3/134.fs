precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdRect(vec2 p, vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

void main(){
  vec2 p = gl_FragCoord.xy;///u_res;
  float i = 0.0;

  // i += step(0., sdRect(p, vec2(.10)));
  i += step(sdCircle(p, 10.0), 0.);

  gl_FragColor = vec4(vec3(i),1);
}