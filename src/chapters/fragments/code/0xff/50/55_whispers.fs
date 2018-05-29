// 55 - "whispers"
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define LINE(x) smoothstep(.01,.001,line(p,vec2(.5,0.),p-(trans*rot(x)),.05))
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
// book of shaders
float line(vec2 p, vec2 a, vec2 b, float r) {
  vec2 pa = p - a, ba = b - a;
  float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
  return length( pa - ba*h ) - r;
}
mat2 rot(float r){
  return r2d((u_time+r)*PI);
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  vec2 trans = (p + vec2(0.5, 0.));  
  float i = LINE(0.0) + LINE(0.5) + LINE(1.0) + LINE(1.5);
  gl_FragColor = vec4(vec3(i),1.);
}