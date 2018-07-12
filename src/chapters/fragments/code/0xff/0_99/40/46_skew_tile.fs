// 46
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define SZ 0.25
float squareSDF(vec2 p){
  return max(abs(p).x, abs(p).y);
}
vec3 tile(vec2 p, float debug){
  vec2 p0 = vec2(p.x-SZ, p.y);
  p0.y -= p0.x;//skew
  vec2 p1 = vec2(p.x+SZ, p.y);
  p1.y += p1.x;//skew
  vec2 fq = vec2(0.079);
  vec2 shade = vec2(step(mod(p+ u_time/3., fq), fq/2.));
  vec3 c = vec3(shade.x) * step(squareSDF(p0), .25) + 
  		     vec3(shade.y) * step(squareSDF(p1), .25);
  c.r += debug;
  return c;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  p = p * 2. + u_time;
  p.x += step(mod(p.y, 2.), 1.) * 0.5;//offset
  vec2 pt = mod(p, vec2(1.));
  vec3 c = tile(pt+vec2(-.5), 0.);
  gl_FragColor = vec4(vec3(c), 1.);
}