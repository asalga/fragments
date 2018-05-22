// 21 - thought of water reprise
precision mediump float;

uniform vec2 ;
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
  float t = u_time;

  // p = mod(p, vec2(0.2, 0.3));

  float shfit = cos( t + p.x * 2.)/10.;
  float tr = ((2.)-1.)/3.;

  // vec2 _t1 = vec2(-tr, shfit);
  float _1 = cInter(p , 1.2, 1.01);

  float i = _1;
  gl_FragColor = vec4(vec3(i), 1.);
}