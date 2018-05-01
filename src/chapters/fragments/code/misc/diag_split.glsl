precision mediump float;

uniform vec2 u_res;

void main(){
  vec2 uv = gl_FragCoord.xy / u_res;
  float c = smoothstep(.0, .01, uv.x - uv.y);
  gl_FragColor = vec4(vec3(c),1.0);
}