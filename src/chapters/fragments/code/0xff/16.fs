precision mediump float;
uniform vec2 u_res;
#define TAU 2.*3.141592658
float ringSDF(vec2 p, float r, float w){
  return abs(length(p)-r) - w;
}
float tri(vec2 p, float s){
  vec2 a = abs(p);
  float u = p.y * .283;
  float _1 = (a.x * 0.8660254034) + u;
  float m = max(_1,-u);
  if(p.y < -0.053) return 1.;
  if(p.x < 0.0 && p.y < 0.45)return 1.; 
  return m - (s*1.41);
}
float tSDF(vec2 p, float s){
  return tri(p, s) * (2.*tri(p, s*0.8));
}
mat2 r2d(float _a){
  return mat2(cos(_a),-sin(_a),sin(_a),cos(_a));
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = 1.2 * a*(gl_FragCoord.xy/u_res*2.-1.);
  float c = 0.;
  float circ = smoothstep(0.02, 0.0029, ringSDF(p, 1., .03));
  for(int i = 0; i < 5; i++){
  	mat2 rot = r2d(TAU/5.*float(i) + 3.1416/8.);
    c += smoothstep(.019, 0.016, tSDF(p * rot, .19));
    circ -= circ * smoothstep(0.01, 0.001,tSDF(p * rot, .3));
  }
  gl_FragColor = vec4(vec3(c+circ), 1.);
}