precision mediump float;

uniform vec2 u_res;
uniform float u_time;

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float i;

  i = step(mod(p.y, 0.2), 0.1);

  gl_FragColor = vec4(vec3(i),1.);
}