precision mediump float;
uniform vec2 u_res;
uniform float u_time;

float horizLineSDF(vec2 p, float y, float thick){
  return step(abs(p.y - y), thick);
}




void main(){
  vec2 a = vec2(u_res.y / u_res.x, 1.);
  vec2 p = a * ((gl_FragCoord.xy / u_res) *2. -1.);
 
  float pos = .8 * sin(u_time * 1.5);

  float i = horizLineSDF(p, pos, .005);

  gl_FragColor = vec4(vec3(i), 1.);
}