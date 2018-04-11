precision highp float;
uniform vec2 u_res;
uniform float u_time;

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = gl_FragCoord.xy / u_res;
  float i = 0.;
  float t = u_time*5.;
  float y = 1.15+t-((p.y+t)*2.);
  float gx = .8/(y+t);
  float z = mod(gx+t,1.);
  i = step(.5,z) + step(.5, p.y);
  float w = .02;
  vec2 sp = p * vec2(1.+.2,1.-.2);
  i *=  step(sp.x, p.y) * step(p.y, sp.x + w) +
  		step(sp.y, 1.-p.x) * step(1.-p.x-w, sp.y);
  float wh = 1.-step(p.y,0.5);
  gl_FragColor = vec4(vec3(i+wh,wh,wh), 1.);
}