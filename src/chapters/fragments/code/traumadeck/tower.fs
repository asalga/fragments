// Tower
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658


float random (vec2 st) {
  return fract(sin(dot(st.xy,vec2(12.9898,178.233)))*43758.5453123);
}
float cSDF(vec2 p, float r){
  return length(p) - r;
}
float cInter(vec2 p, float r, float i){
  float a = smoothstep(0.01, 0.001,cSDF(p+vec2(0.,-i), r));
  float b = smoothstep(0.01, 0.001,cSDF(p+vec2(0.,i), r));
  return a*b;
}
float cInterStroke(vec2 p, float r, float i){
  float a = cInter(p,r,i);
  return a - cInter(p,r-.06,i);
}
//from book of shaders
float rectSDF(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x, d.y), 0.0) + length(max(d,0.0));
}
float eye(vec2 p){
  p/= 0.5;
  float i = cInter(p-vec2(0.,.6), .6, 0.3);
  i -= cInter(p-vec2(0.,.6), .55, 0.3);
  i += step(cSDF(p-vec2(0.,0.7), 0.2), 0.);
  return i;
}
float moon(vec2 p){
  p += vec2(0.45, -1.);
  p/=0.3;
  float t = mod(u_time*0.05, 2.*PI);
  vec3 s = vec3(cos(t), .0, sin(t));
  float z = sqrt(1. - pow(p.x,2.) - pow(p.y,2.));
  vec3 v = normalize(vec3(p.x, p.y, z));
  vec3 i = vec3(smoothstep(.1,.11,dot(v, s)) + 
  				smoothstep(.48, .49, 1.-cSDF(p, .5)) -
  				smoothstep(.38, .39, 1.-cSDF(p, .37)));
return i.x;
}

float specks(vec2 p ){
  return smoothstep(0., .1, 2.*random(p));
}
float tower(vec2 p, vec2 towerPos){
  float i = 0.;
  // tower body
  vec2 towerBodySz = vec2(0.3, .7);
  i += step(rectSDF(towerPos+ vec2(0., 0.7), towerBodySz), 0.);

  // tower top bricks
  float topBrickSz = 0.15;
  i += step(mod(p.x-0.15, topBrickSz), topBrickSz/2.)*
  	   step(rectSDF(p-vec2(0., 0.14), vec2(0.5, 0.05)), 0.);

  // tower top
  vec2 towerTopSz = vec2(0.5, .12);
  i += step(rectSDF(towerPos-vec2(0.,.1), towerTopSz), 0.);
  return i;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  vec2 towerPos = p+ vec2(0.0, 0.1);
  float i = 0.;

  i += tower(p, towerPos);
  i += eye(p);
  i += moon(p);
  // i *= specks(p);

  gl_FragColor = vec4(vec3(i),1.);
}