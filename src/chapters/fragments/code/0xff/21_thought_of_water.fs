// 21 - thought of water reprise
precision mediump float;
uniform vec2 u_tracking;
uniform vec2 u_res;
uniform float u_time;
float cSDF(vec2 p, float r){
  return length(p)-r;
}
float cInter(vec2 p, float r, float i){
  float a = smoothstep(0.01, 0.001,cSDF(p+vec2(0.,-i), r));
  float b = smoothstep(0.01, 0.001,cSDF(p+vec2(0.,i), r));
  return a*b;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);

  float t = u_time * 3.;
  float shfit = cos(t+p.x*(u_tracking.x+0.5)*10.)/10.;
  float tr = ((2.*u_tracking.y)-1.)/3.;

  vec2 _t1 = vec2(-tr, -.35 + shfit);
  float _1 = cInter(p+_t1 , 1.2, 1.1);
  vec2 _t2 = vec2(tr, .35 + shfit);
  float _2 = cInter(p+_t2 , 1.2, 1.1);
  vec2 _t3 = vec2(0., shfit);
  float _3 = cInter(p+_t3 , 1.2, 1.1);

  float i = _1 + _2 + _3;
  gl_FragColor = vec4(vec3(i), 1.);
}