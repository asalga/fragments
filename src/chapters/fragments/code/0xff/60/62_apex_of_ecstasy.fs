// 62
precision mediump float;
#define TAU 3.141592658*2.
uniform vec2 u_res;
uniform float u_time;
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
  float ti = u_time;
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  p.y *= 2.;
  vec2 np = p;
  p.x = (p.x+1.+ti)*.5;// adjust only x, not y  
  float i = step(p.y, sin((p.x)*TAU)) - 
  	  		step(p.y+0.05, sin((p.x)*TAU));
  for(int it=0;it<30;it++){
    float fit = float(it);
    ti -= pow(1.06, fit)/100.;
    float mti = mod(ti,1.);
    vec2 t = vec2(-mod(ti, 2.)+1., -sin(ti*TAU));
    float d = cos(mti * TAU)*1.4;//bit shitty right here
    vec2 _p = (np+t) * r2d(d);
    i +=step(capsule(_p,vec2(-.5,0.),vec2(.5,0.),.02),0.);
  }
  gl_FragColor = vec4(vec3(i),1.);
}