precision highp float;

uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)-0.5;

  float cellSz = 1./4.;
  float r = cellSz/2.;
  p = mod(p, vec2(cellSz))-vec2(0.5*cellSz);
  float i = step(sdCircle(p, r), 0.);

  gl_FragColor = vec4(vec3(i),1);
}

