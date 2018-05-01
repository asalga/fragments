precision mediump float;
uniform vec2 u_res;

float squareSDF(float s){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * ((gl_FragCoord.xy / u_res.xy) * 2. - 1.);
  vec2 absVal = abs(p.xy);
  return step(s, max(absVal.x, absVal.y));
}

void main(){
  float i = squareSDF(0.8);
  gl_FragColor = vec4(vec3(i), 1.0);
}