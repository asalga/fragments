precision mediump float;
uniform float u_time;
uniform vec2 u_res;
void main(){
  vec2 p = gl_FragCoord.xy / u_res + vec2(u_time/5.);
  // 
  float c = step(5. * p.y, floor(5. * p.x));
  gl_FragColor = vec4(vec3(c), 1.);
}