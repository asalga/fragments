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

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)-0.5;
  float i;

  float cellSz = 1./4.;
  float r = cellSz/2.;
  p = mod(p, vec2(cellSz))-vec2(0.5*cellSz);

  i = step(sdCircle(p, r), 0.);
  // i -= step(sdRect(p-vec2(0.,r/2.),r),0.);


  gl_FragColor = vec4(vec3(i),1);
}

