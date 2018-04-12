precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
float cSDF(vec2 p, float r){
  return (length(p) - r);
}
void main(){
  vec2 a = vec2(1.,u_res.y/u_res.x);
  vec2 p = 1.2 * a * (gl_FragCoord.xy / u_res * 2. -1.);
  float t = mod(u_time, 2.*PI);
  vec3 s = vec3(cos(t), .0, sin(t));
  float z = sqrt(1. - pow(p.x,2.) - pow(p.y,2.));
  vec3 v = normalize(vec3(p.x, p.y, z));
  vec3 i = vec3(smoothstep(.1,.11,dot(v, s)) + 
  				smoothstep(.48, .49, 1.-cSDF(p, .5)) -
  				smoothstep(.38, .39, 1.-cSDF(p, .37)));
  gl_FragColor = vec4(i, 1.);
}