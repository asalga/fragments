// thought of water reprise
precision mediump float;
uniform vec2 u_res;
uniform float u_time;

float cSDF(vec2 p, float r){
  return length(p)-r;
}

float circleIntersection(vec2 p, float r, float i){
  float a = step(cSDF(p+vec2(0.,-i), r), 0.);
  float b = step(cSDF(p+vec2(0.,i), r), 0.);
  return a*b;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  p.y -= .2;
  float t = u_time * 3.;
  
  vec2 _t1 = vec2(0.3, cos(t+p.x*10.)/10.);
  float _1 = circleIntersection(p+_t1 , 1.2, 1.1);

  vec2 _t2 = vec2(0.3, .28+cos(t+p.x*10.)/10.);
  float _2 = circleIntersection(p+_t2 , 1.2, 1.1);

  vec2 _t3 = vec2(-0.35, .15+cos(t+p.x*10.)/10.);
  float _3 = circleIntersection(p+_t3 , 1.2, 1.1);

  vec2 _t4 = vec2(-0.35, .42+cos(t+p.x*10.)/10.);
  float _4 = circleIntersection(p+_t4 , 1.2, 1.1);

  float i = _1 + _2 + _3 +_4;
  gl_FragColor = vec4(vec3(i), 1.);
}