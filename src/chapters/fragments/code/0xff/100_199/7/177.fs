precision mediump float;

uniform vec2 u_res;
uniform float u_time;
float sampleChecker(vec2 c) {
  float col;
  float sz = 0.25;

  float x = step(mod(c.x,sz), sz/2.);
  float y = step(mod(c.y,sz), sz/2.);

  if(x == y){return 1.;}
  return x*y;
}
void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float i;

  float x = p.x;
  float y = p.y;

  p *= (sin(u_time*.2)+1./2.) * length(p);

  p.x += sin(y + u_time);
  p.y += sin(x - u_time);

  i = sampleChecker(p);

  gl_FragColor = vec4(vec3(i),1.);
}