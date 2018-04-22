precision mediump float;
uniform vec2 u_res;
#define COS_30 0.866
#define PI 3.141592658
#define V2(x,y) vec2(x,y)
float cTri(vec2 p, float s) {
  vec2 a = abs(p);
  return max(a.x*COS_30+(p.y*.5),-p.y*.5)-s*.5;
}
float cSDF(vec2 p, float r){
  return length(p)-r;
}
float rectSDF(vec2 p, vec2 size){
  vec2 absValue = abs(p / size);
  float side = max(absValue.x, absValue.y);
  return step(size.y, side + abs(sin(2.*p.x)));
}
float moon(vec2 p, float r, float w){
  return abs(length(p)-r)-w;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float theta = step((atan(-(p.y- 1.1)*.0220, p.x*sin(p.x))), 0.7);
  float i = step(cSDF(p+V2(0.,2.3), 1.5), 0.);
  i += 1.-step(cos(theta*8.),0.);
  i += step(cTri(V2(p.x, p.y-1.),.15), 0.);
  i += step(moon(p+vec2(0., -1.),.35, .01), 0.);
  for(float y = -0.4; y < 0.4; y+=0.14){
    i -= step(cSDF(p+vec2(-.03, y), .01), 0.);
  }
  gl_FragColor = vec4(vec3(i), 1.);
}
