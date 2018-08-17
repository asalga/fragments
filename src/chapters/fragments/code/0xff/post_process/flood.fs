precision mediump float;

uniform sampler2D u_t0;
uniform float u_time;
uniform vec2 u_res;

void main(){
  float t = u_time;
  vec2 p = (gl_FragCoord.xy/u_res);
  p.y = 1.-p.y;

  float marker = (sin(t*1.5)+1.)/2.;
  if(marker > p.y){
    p.y = marker;
  }

  vec3 c = texture2D(u_t0, p).rgb;
  gl_FragColor = vec4(c, 1);
}