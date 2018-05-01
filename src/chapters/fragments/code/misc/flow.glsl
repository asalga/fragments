precision mediump float;
#define PI 3.141592658
uniform vec2 u_res;
uniform float u_time;
void main(){
  vec2 uv = gl_FragCoord.xy / u_res * 2. - 1.;
  
  // float v = (sin(u_time) + 2.) + u_time/5.;
  // float s = u_time * (sin(PI * mod(1.*u_time, PI/2.) / PI/2.) + 1.);
  // float phaseShift = u_time +  min(1.6,abs( sin(u_time)));

  float t =  sin( uv.x + 2. * u_time + sin(PI/2.*u_time + PI)) ;
  float c = step(.5 * t, uv.y);
  gl_FragColor = vec4(vec3(c), 1.);
}