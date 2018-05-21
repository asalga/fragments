precision mediump float;
uniform vec2 u_res;
uniform float u_time;

float circleSDF(vec2 p, float r){
  return length(p)-r;
}

float e(in vec2 p, float s, vec2 o, float timeOffset){
  float t =(u_time+timeOffset) * 7.;
  o *= vec2(cos(t), sin(t));
  float test = 0.;
  test += smoothstep(.03, .005 ,circleSDF(p+o,s));
  test += (1.-circleSDF(p+o,.5))/10.;
  return test;
}

void main(){
  vec2 a = vec2(1., u_res.y/ u_res.x);
  vec2 p = a * (gl_FragCoord.xy/ u_res * 2. -1.);
  float s = 0.05;
  float i = e(p, s*2., vec2(0.), 0.) + 
  			e(p, s, -vec2(.5, 1.), 0.) + 
  			e(p, s, vec2(1.,.5), 4.) +
  			e(p, s, vec2(0.,-1.) + vec2(cos(3.14/3.), sin(3.14/1.)), 11.);
  gl_FragColor = vec4(vec3(i), 1.);
}