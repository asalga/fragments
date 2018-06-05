// 62 - kitchen tiles
precision mediump float;
#define PI 3.141592658
#define TAU PI*2.
uniform vec2 u_res;
uniform float u_time;
float circ(vec2 p, float r){
  return step( length(p)-r, 0.);
}
mat2 r2d(float a){
  return mat2(cos(a),sin(a),-sin(a),cos(a));
}
//from bookofshaders
float capsule(vec2 p, vec2 a, vec2 b, float r) {
  vec2 pa = p - a, ba = b - a;
  float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
  return length( pa - ba*h ) - r;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float ti = u_time;
  float lineLen = 0.5;
  p.x = (p.x+1.)*.5;// adjust only x, not y
  float i = step(p.y, sin(p.x*TAU)) - 
  	  		step(p.y+0.05, sin(p.x*TAU));

  for(int it=0;it<15;it++){
    float fit = float(it);
    ti += pow(fit,.011);
    float mti = mod(ti,1.);
    float tx = mti * TAU;
    vec2 t = vec2(-mti, -sin(ti*TAU));
    float d = cos( mti * TAU)*1.4;//bit shitty right here
    vec2 pos = (p+t) * r2d(d);
    i += step(capsule(pos, vec2(-lineLen, 0.),vec2(lineLen,0.), 0.009), 0.);
  }
  gl_FragColor = vec4(vec3(i),1.);
}