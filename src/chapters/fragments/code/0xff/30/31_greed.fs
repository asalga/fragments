precision mediump float;
uniform vec2 u_res;
uniform float u_time;

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  
  float i = 1.;
  gl_FragColor = vec4(vec3(i), 1.);
}