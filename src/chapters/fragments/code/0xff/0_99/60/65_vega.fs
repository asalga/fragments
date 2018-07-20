// 65 - Vega
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

mat2 r2d(float a){
  return mat2(cos(a),sin(a),-sin(a),cos(a));
}
float sdCircle(vec2 p, float r){
  return length(p)-r;
}
float circle(vec2 p, float r){
  return smoothstep(0.01, 0.001, sdCircle(p, r));
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i, r = 0.85;
  for(int it = 10; it > 0; --it){
    float invert = mod(float(it), 2.)*2.-1.;// -1 .. 1
    i -= invert * circle(p, r);
    p *= r2d(u_time);
    p += vec2(0.1, 0.);
    r -= 0.1;
  }  
  gl_FragColor = vec4(vec3(i),1.);
}