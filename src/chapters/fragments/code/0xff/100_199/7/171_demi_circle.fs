precision highp float;

uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float i;

  i = step(sdCircle(p, 0.25), 0.);

  gl_FragColor = vec4(vec3(i),1);
}