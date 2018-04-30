precision mediump float;
uniform vec2 u_res;
uniform float u_time;
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float cnt = 2.;// countperside
  float r = .25;
  // p.x += u_time/100.0;
  // p.y *=1.2;

  // even rows
  if(mod(floor(2.*p.y),2.) == 0.){
    // gl_FragColor.x = 0.8;
    p.x += r;
    // p.y += .;
  }
  // p.y += 0.08;

  vec2 cp = p;
  // cp.y += 0.1;
  // p.y -= 0.1;
  vec2 closestCorner = (floor(cp*cnt)/cnt)+r;
  float len = length(p-closestCorner);
  gl_FragColor = vec4(vec3(smoothstep(r/1., r/1.1,len)),1.);
}