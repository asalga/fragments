precision mediump float;
uniform vec2 u_res;
uniform float u_time;

float cSDF(vec2 p, float r){
  return length(p)-r;
}
float vertLintSDF(vec2 p, float w){
  vec2 a = abs(p);
  return length(vec2(max(a.y-w,0.), a.x));
}
float circInter(vec2 p, float r, float n){
  float a = length(p+vec2(0., r*n))-r;
  float b = length(p-vec2(0., r*n))-r;
  return a * b;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  
  // i += step(cSDF(p, 0.15), 0.);
  // gl_FragColor = vec4(vec3(i), 1.);  
  
  float i = 0.;
  // float flen = 0.2;
  // float fdist = 0.21;

  // i += step(cSDF(p, .35), 0.);
  // i += step(cSDF(p+vec2(+.35, -.17),.14), 0.);
  // i += step(cSDF(p+vec2(-.35, -.17),.14), 0.);
  
  // i += step(vertLintSDF(p+vec2(+fdist, -.35), flen), 0.1);
  // i += step(vertLintSDF(p+vec2(0.,     -.5),  flen), 0.1);
  // i += step(vertLintSDF(p+vec2(-fdist, -.35), flen), 0.1);

  i += step(circInter(p, 0.1, .5),0.05);

  gl_FragColor = vec4(vec3(i), 1.);
}