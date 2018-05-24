precision mediump float;

#define PI 3.141592658
#define COUNT 2.

uniform vec2 u_res;
uniform float u_time;
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
float cSDF(vec2 p, float r){
  return length(p)-r;
}
float ringSDF(vec2 p, float r, float w){
  return abs(length(p) - (r*.5)) - w;
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
  float len = length(p);
  float spin = 1.;
  p *= r2d(t*.7);

  vec2 spiralPos = p;
  float th = atan(spiralPos.y,spiralPos.x)/PI + PI/8.7;
  float splen = length(spiralPos);  
  float spiral;

  th += step(splen,spin) * pow((spin-splen),1.);
  float ca = floor(th * COUNT)/COUNT;
  spiral += smoothstep(0.21, 0.215,th-ca);

  float circle = step(cSDF(p, 0.5), 0.);

  for(float it = 0.; it < 4.; it +=1.){
    vec2 sp0 = p;
    mat2 rot = r2d(it*PI/2.);
    sp0 = vec2(.45, 0.) + (p * rot);
    sp0.x *= 1.3;
    // position defines phase of wave
    float shfitwave = (sp0.x * 2. * sin(it+(sp0.x + (2.*t + it/5.) + it ) * 4.)) /4.;  
    float squiggle0 = circInter(sp0 + vec2(.0, shfitwave) , 1.2, 1.);
    i += clamp(squiggle0-circle, 0., 1.);
  }

  i += step(ringSDF(p, 1., 0.01), 0.0);
  i += spiral * circle;
  gl_FragColor = vec4(vec3(i), 1.);
}