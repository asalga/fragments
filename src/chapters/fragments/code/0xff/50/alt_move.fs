precision mediump float;
uniform vec2 u_res;
uniform float u_time;
float cSDF(vec2 p, float r){
  return length(p) - r;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time * 2.;
  float i;
  
  float yTime = t * a.y * step(mod(t, 1.),.5);
  float xTime = t * step(.5, mod(t,1.));

  p.y -= step(mod(p.x, 1.), 0.5) * yTime;
  p.x -= step(mod(p.y, a.y), a.y/2.) * xTime;

  vec2 np = mod(p, vec2(0.5, a.y/2.)) - 0.25;
  i += step(cSDF(np, 0.25/2.), 0.0);

  gl_FragColor = vec4(vec3(i),1.);
}