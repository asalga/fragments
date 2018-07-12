precision mediump float;
uniform vec2 u_res;
#define COS_30 0.8660256249
float circleSDF(vec2 p, float r){
  return length(p)-r;
}
float semicircleSDF(vec2 p, float r){
  return max(length(p)-r,-p.y);
}
float horizLineSDF(vec2 p, float w){
  vec2 a = abs(p);
  return length(vec2(max(a.y-w,0.), a.x));
}
float triangleSDF(vec2 p, float s, float sc){
  vec2 a = abs(p);
  float distToSide = a.x * COS_30;
  float u = p.y * sc;
  float _1 = distToSide + u;
  float _2 = -u;
  float m = max(_1,_2);
  return m - s;
}

float rectSDF(vec2 p, vec2 size){
  vec2 absValue = abs(p / size);
  float side = max(absValue.x, absValue.y);
  return step(side, size.x);
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = 0.8 * a * (gl_FragCoord.xy/u_res*2.-1.);
  float w = 0.65;

  float top = step(semicircleSDF(p, .42), 0.) +
              rectSDF(p+vec2(0., 0.08), vec2(w, 0.12));
  float es = 0.12;
  float eyes = step(circleSDF(p+ vec2(+0.158, 0.04), es), 0.) +
  			   step(circleSDF(p+ vec2(-0.158, 0.04), es), 0.);
  float tri = step(triangleSDF(-p + vec2(0., -.03), .5, 1.2), .01);
  float r = rectSDF(p+vec2(0.,.227), vec2(w, .1));
  float nose = step(triangleSDF(p+vec2(0.0, 0.18), .03, 0.7), .001);
  float i = top - eyes + (tri*r) - nose;
  i += step(horizLineSDF(p+vec2(+.06, .3), .07), .025);
  i += step(horizLineSDF(p+vec2(-.06, .3), .07), .025);
  i += step(horizLineSDF(p+vec2(-.18, .28),.07), .043);
  i += step(horizLineSDF(p+vec2(+.18, .28),.07), .043);
  gl_FragColor = vec4(vec3(i), 1.);
}