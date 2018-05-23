precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float cSDF(vec2 p, float r){
  return length(p)-r;
}
float opSub(float a, float b){
  return a-b;
}
float random (vec2 st) {
  return fract(sin(dot(st.xy,vec2(12.9898,178.233)))*43758.5453123);
}
// position, radius, phase
float moon(vec2 p, float r, float phase){
  float a = step(cSDF(p,r), 0.);
  float b = step(cSDF(p+vec2(phase*r*2., 0.),r), 0.);
  return 1.-smoothstep(0.1, 0.01, a-b);
}

float circInter(vec2 p, float r, float i){
  float a = smoothstep(0.01, 0.001,cSDF(p+vec2(0.,-i), r));
  float b = smoothstep(0.01, 0.001,cSDF(p+vec2(0.,i), r));
  return a*b;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time;
  float i;
  // t=0.;
  // t = mod(u_time, .5);

  float len = length(p);
  // position defines phase of wave
  float shfitwave =  (p.x* sin((p.x+t) * 4.)) /4.;
  
  vec2 sq_trans = vec2(0. , shfitwave);
  


  float squiggle = circInter(p + sq_trans , 1.1, 1.002);
  i += squiggle;
  gl_FragColor = vec4(vec3(i), 1.);
}


// float tr = (2.-1.)/3.;