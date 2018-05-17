precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define SZ 0.25
float squareSDF(vec2 p){
  return max(abs(p).x, abs(p).y);
}
vec3 tile(vec2 p){
  vec2 p0 = vec2(p.x-SZ, p.y);
  			// p0.y -= p0.x;
  vec2 p1 = vec2(p.x+SZ, p.y);
  			// p1.y += p1.x;

  return vec3(0.25) * step(squareSDF(p0), 0.25) + 
  		 vec3(1.) * step(squareSDF(p1), 0.25);
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  

  p *= 2.5;
  p.y += mod(u_time, 2.);

  if(p.y > 3. ){discard;}

  vec2 pt = vec2(0.);
  float X = step(mod(p.y,2.),1.)*.5;
  pt.x = mod(p.x + X, 1.);
  pt.y = mod(p.y, 1.);

  vec3 c = tile(pt+vec2(-.5));
  gl_FragColor = vec4(vec3(c), 1.);
}