precision mediump float;
uniform float u_time;
uniform vec2 u_res;
#define R 0.25

float cSDF(vec2 p, float r){
  return length(p) - r;
}
float rectSDF(vec2 p, vec2 size){
  vec2 absValue = abs(p / size);
  float side = max(absValue.x, absValue.y);
  return 1.-step(size.y, side);
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i = .0;
  float t = u_time;// * .5;

  float T =  mod(t, R*2.);
  vec2 SZ = vec2(0.5, 0.5);

  vec2 v = vec2(0., 0.);
  v.y = -a.y + T;
  v.y += mod(u_time*2., a.y-R*2.);

  i += step(cSDF(p + v, R), 0.);
  
  p.y += T;
  // i += step(cSDF(p + vec2(-0.4 ,   0.),   R), 0.) +
  // 	   step(cSDF(p + vec2(-0.4 , R*2.), R), 0.) + 
  // 	   step(cSDF(p + vec2(-0.4 , R*4.), R), 0.) + 
  // 	   step(cSDF(p + vec2(-0.4 , R*6.), R), 0.);


  i += rectSDF(p, SZ)+
  	   rectSDF(p + vec2(0., R*2.), SZ) +
  	   rectSDF(p + vec2(0., R*4.), SZ) + 
  	   rectSDF(p + vec2(0., R*6.), SZ);

  gl_FragColor = vec4(vec3(i),1.);
}