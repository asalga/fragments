precision mediump float;
uniform vec2 u_res;
uniform float u_time;
void main(){
  vec2 a = vec2(u_res.x / u_res.y, 1.);
  vec2 uv = a * (gl_FragCoord.xy / u_res * 2. - 1.);
  float x = sin(u_time) / 2.;
  float m = 0.85 / distance(uv, vec2(-x, 0.));	
  float f = 0.5 / distance(uv, vec2(x, .3));
  float c = 0.2 * (pow(m, 1.) + pow(f, 1.));
  gl_FragColor = vec4(vec3(smoothstep(0.7, 0.72,c)),1.);
}