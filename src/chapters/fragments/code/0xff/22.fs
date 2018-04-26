precision mediump float;
uniform vec2 u_res;
#define COS_30 0.8660256249
#define PI 3.14159265834
#define TAU (2.*PI)
uniform float u_time;
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
float triSDF(vec2 p, float s, float sc){
  vec2 a = abs(p);
  float distToSide = a.x * COS_30;
  float u = p.y * sc;
  return max(distToSide + u, -u) - s;
}
float py(vec2 p){
  float tri = step(triSDF(p+vec2(0.,.2), .3, 0.5), 0.);
  tri *= step(p.y, 0.10);
  tri *= step(mod(p.y, 0.1), 0.05);
  return tri;
}
float fan(vec2 p){
  float t = u_time*13.;
  float theta =  (atan(p.y+1.5,p.x))/TAU;
  float stripes = step(sin(t + 20.*atan(p.y-.6,p.x)),0.);
  float tri = step(triSDF(p-vec2(0.,1.), .45, .5),0.);
  return tri * stripes;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a* (gl_FragCoord.xy/u_res*2.-1.);
  float i = 0.;

  i += py(p);
  float eye = cInter(p-vec2(0.,.6), .6, 0.3);

  i += fan(p) - (fan(p) *eye);
  i += eye - cInter(p-vec2(0.,.6), .55, 0.3);

  float wavesY = 1.2;
  i += cInterStroke(p+vec2(0.0, wavesY), .5, 0.3);
  i += cInterStroke(p+vec2(+.73, wavesY), .5, 0.3);
  i += cInterStroke(p+vec2(-.73, wavesY), .5, 0.3);


  i += step(cSDF(p-vec2(0.,0.7), 0.2), 0.);

  gl_FragColor = vec4(vec3(i), 1.);
}