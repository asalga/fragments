precision mediump float;
uniform vec2 u_res;
#define PI 3.141592658
#define e 0.011

float rectSDF(vec2 p, vec2 dims){
  vec2 a = abs(p / dims);
  float v = max(a.x, a.y);
  float test = smoothstep(v, v + e,  dims.x) + step(v,  dims.y);
  //ew ew ew, need to learn a better way
  return ceil(test/2.);
}

float circleSDF(vec2 p, float rad){
  float len = length(p);
  return 1. - smoothstep(rad, rad + e, len);
}

float loopSDF(vec2 p, vec2 dims){  
  float d = dims.x+dims.y;
  float l = length(p);
  return (1. - smoothstep(d, d + e, length(p))) * (1. - smoothstep(l, l + e, dims.x));
}

float halfLoopSDF(vec2 p, vec2 dims){   
  return loopSDF(p,dims) * smoothstep(p.x, p.x + e, 0.);
}

vec2 rot(vec2 p, float theta){
  return p * mat2( cos(theta), -sin(theta), sin(theta), cos(theta));
}

float pill(vec2 p, float pos,  float rad, vec2 d, vec2 loopSpec){
  vec2 a = vec2(1.0, u_res.y / u_res.x);
  vec2 toCut = a * vec2(.03);
  float r0 = rectSDF(p + vec2(0.,  rad + 0.0141), vec2(0.8, 0.028));
  float r1 = rectSDF(p + vec2(0., -rad - 0.0141), vec2(0.8, 0.028));
  float c1 = halfLoopSDF(p + vec2(0.6, 0.), loopSpec);
  float c2 = halfLoopSDF(rot(p, -PI) + vec2(0.6, 0.) , loopSpec);
  return c1 + r0 + r1 + c2;
}

void main(){
  vec2 a = vec2(1.0, u_res.y / u_res.x);
  vec2 p = a * (gl_FragCoord.xy / u_res * 2. - 1.);
  float t = -PI/4.;
  mat2 rot = mat2( cos(t), -sin(t), sin(t), cos(t));
  p *= rot;
  float thick = 0.03;
  float rad = 0.24;
  float pos = .6;
  vec2 toCut = a * vec2(.03);
  vec2 dims = vec2(0.8, 0.3);
  vec2 loopSpec = vec2(0.24, thick);
  float total = pill(p, pos, rad, dims, loopSpec);
  float highlight = circleSDF(p - vec2(0.6, -0.1), 0.07);
  float leftCircle = circleSDF(p + vec2(0.6, 0.), rad - 0.05);
  float rect = rectSDF(p + vec2(0.35, 0.), vec2(0.5, 0.39));
  float rectBlackHighlight = 2.*rectSDF(p + vec2(0.38, 0.13), vec2(0.48, 0.05));
  total += highlight + leftCircle + rect - rectBlackHighlight;
  gl_FragColor = vec4(vec3(total), 1.);
}