precision lowp float;
uniform vec2 u_res;
uniform float u_time;
#define COS_30 0.866025

float triangle(vec2 p, float s) {
  vec2 a = abs(p);
  return max(a.x*COS_30+(p.y*.5),-p.y*.5)-s*.5;
}
float otri(vec2 p, float s){

  float ss = smoothstep(0.4, 0.2,triangle(p,s));
  return ss;


  //step(ss, 0.)
  		 //smoothstep(0.1, 0.0001+ (1.1* (1.+cos(-1.*0.) ))    ,triangle(p+vec2(.0,.01),s-.03));
}
void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.; 
  float sz = .35;
  float o = otri(p,sz)+otri(-p-vec2(0.,sz*.75),sz);
  gl_FragColor = vec4(vec3(o),1.);
}