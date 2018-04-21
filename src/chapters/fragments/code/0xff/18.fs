precision mediump float;
uniform vec2 u_res;
uniform float u_time;

float cSDF(vec2 p, float r){
  return length(p)-r;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);

  float i = step(cSDF(p, .5), 0.);
  gl_FragColor = vec4(vec3(i),1.);
}