precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float PI = 3.141592658;

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

float sdCircle(in vec2 p, in float r){
  return length(p)-r;
}

mat2 r2d(in float a){
  return mat2(cos(a),sin(a),-sin(a),cos(a));
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time * .5;

  vec2 ln = p;
  ln += vec2(t,0.);
  ln *= r2d(PI/4.);
  i = mod(ln.x, 0.2);

  vec2 ln2 = p;
  ln2 -= vec2(t*2.,0.);
  ln2 *= r2d(-PI/4.);
  // i += step(mod(ln2.x, 0.4), 0.2);

  float x = sdCircle(p, 0.9);
  x = pow(10., x);
  i += x;

  i = clamp(i, 0., 1.);

  x = sdCircle(vec2(fract(p.x+0.4+t),p.x), 0.9);
  x = pow(4., x);
  // i -= 1.-x;

  i -= fract(p.x*10. + t*2.);

  i = 0.;

  float test = sin(p.y*1.*3.14159+t*5.)/5.;
  float test2 = sin(p.x*1.*3.14159+t*5.)/5.;


  i = step(-.9, p.x) * step(p.y, 0.9);
  i *= step(p.x, 0.9) * step( -0.9, p.y);

  if(p.x > test){
    i= 1.-i;
  }
  if(p.y > test2){
    i= 1.-i;
  }

  if(p.y > p.x){
    i = 1.-i;
  }

  gl_FragColor = vec4(vec3(i),1.);
}









