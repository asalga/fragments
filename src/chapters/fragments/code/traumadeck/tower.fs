
precision mediump float;
uniform vec2 u_res;
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
  // i += moon

  gl_FragColor = vec4(vec3(i),1.);
}