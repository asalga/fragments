precision mediump float;
uniform float u_time;
uniform vec2 u_res;
#define R 0.25
// https://gist.github.com/girish3/11167208
float eob(float t) {
  if ((t/=1.0) < (1./2.75)) {return 1.*(7.5625*t*t);}
  else if (t < (2./2.75)) {return 1.*(7.5625*(t-=(1.5/2.75))*t + .75);}
  else if (t < (2.5/2.75)) {return 1.*(7.5625*(t-=(2.25/2.75))*t + .9375);}   
  return 1.*(7.5625*(t-=(2.625/2.75))*t + .984375);
}
float cSDF(vec2 p, float r){
  return length(p) - r;
}
//from book of shaders
float rectSDF(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x, d.y), 0.0) + length(max(d,0.0));
}
float box(vec2 p, float off){
  return step(rectSDF(p + vec2(0., R*off), vec2(0.24)), 0.);
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  p.y-= 0.28; //cheap way of offsetting dropped obj so it doesn't just appear
  float T = mod(u_time, .5);
  vec2 v = vec2(0., -a.y + T);
  v.y += eob(mod(u_time*2., a.y - .5));
  float i = step(cSDF(p + v, R), 0.);// dropped object
  p.y += T;
  i +=  box(p, 0.) + box(p, 2.) + box(p, 4.) + box(p, 6.);
  gl_FragColor = vec4(vec3(i),1.);
}
