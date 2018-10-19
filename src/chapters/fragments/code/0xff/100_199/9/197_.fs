precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define C 4.



float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;
  return fract( sin(x+y) * 23454.);
}
float ringSDF(vec2 p, float r, float w){
  return abs(length(p) - (r*.5)) - w;
}
mat2 r2d(float a){
  return mat2(cos(a),sin(a),sin(a),-cos(a));
}
void main(){
  vec2 p = (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time*4.;

  vec2 idx = vec2(floor(p*C))/2.;
  p = mod(p, 1./C);

  p /= (1./C)*.5;

  p -= vec2(1.); //center
  p /= (cos(  t+ 5.*valueNoise(vec2(idx) ) )+1.5)/2.;

  float i;// = smoothstep(.1, .01, ringSDF(p, 1., .04));

  // p *= r2d(t + idx.x + idx.y);

  i += smoothstep(0.1, .01, ringSDF(p+vec2(.2), 1., .01));

  i = 1.-i;
  gl_FragColor = vec4(vec3(i),1.);
}