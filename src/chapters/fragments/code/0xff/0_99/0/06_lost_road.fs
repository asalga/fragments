precision highp float;
uniform vec2 u_res;
uniform float u_time;

void main(){
  vec2 p = gl_FragCoord.xy / u_res;
  float t = u_time*5.;
  float y = 1.15+t-((p.y+t)*2.);
  float gx = .8/(y+t);
  float z = mod(gx+t,1.);
  float w = .02;
  vec2 sp = p * vec2(1.+.2,1.-.2);
  float i = step(.5,z) + step(.5, p.y);
  i *=  step(sp.x, p.y) * step(p.y, sp.x + w) +
  		step(sp.y, 1.-p.x) * step(1.-p.x-w, sp.y);
  gl_FragColor = vec4(vec3(i+ step(0.5, p.y)), 1.);
}