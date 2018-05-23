precision mediump float;
uniform vec2 u_res;
#define COS_30 0.8660256249

float cSDF(vec2 p, float r){
  return length(p)-r;
}
//from book of shaders
float rectSDF(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y), .0) + length(max(d,.0));
}
float ringSDF(vec2 p, float r, float w){
  return abs(length(p) - (r*.5)) - w;
}
float triSDF(vec2 p, float s, float sc){
  vec2 a = abs(p);
  float distToSide = a.x * COS_30;
  float u = p.y * sc;
  return max(distToSide + u, -u) - s;
}

float arrow(vec2 p){
  float arrowLen = 0.2;
  float tri = step(triSDF(p, 0.05, 0.5), 0.);
  float rect = step(rectSDF(p+ vec2(0., arrowLen), vec2(0.01, 0.2)), 0.);
  return tri + rect;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float r = 0.5;

  float pupil = step(cSDF(p, 0.05), 0.);
  float outerCirc = step(ringSDF(p, r, 0.01), 0.);

  // subtract a bit so triangle touches
  vec2 arr1Pos = p + vec2(0., r-0.15);
  float arrow1 = arrow(arr1Pos);


  i += pupil + outerCirc + arrow1;

  gl_FragColor = vec4(vec3(i),1.);
}

