precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define SZ .5
float cSDF(vec2 p, float r){
  return length(p) - r;
}
float rSDF(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x, d.y), 0.) + length(max(d,0.));
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time*4.;

  float c = cSDF(p, SZ);
  float r = rSDF(p, vec2(SZ, SZ));

  float s = (sin(t)+1.)/2.;
  i = smoothstep(0.01, 0.001, mix(c,r,s));

  gl_FragColor = vec4(vec3(i), 1.);
}