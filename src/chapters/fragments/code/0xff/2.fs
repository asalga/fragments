precision mediump float;
uniform vec2 u_res;

float squareSDF(vec2 p, float size){
  return 1.;
}

void main(){
  vec2 a = vec2(u_res.x / u_res.y, 1.);
  vec2 p = a * (gl_FragCoord.xy / u_res * 2. - 1.);

  float c = squareSDF(p);
  gl_FragColor = vec4(vec3(c), 1.);
}