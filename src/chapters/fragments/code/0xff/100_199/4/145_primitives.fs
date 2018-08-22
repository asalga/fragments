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
  float t = u_time* .5;
  vec2 p = (gl_FragCoord.xy/u_res*2.-1.);
  float lines,i;

  vec2 ln4 = p;

  // add rect
  float rect = step(sdRect(p, vec2(0.85)),0.);

  vec2 ln2 = p;
  ln2 += vec2(t+0.142,0.);
  ln2 *= r2d(-PI/4.);
  float lines2 = step(mod(ln2.x, 0.2), 0.1);
  i += rect * lines2;

  // cut out circle
  float circ = step(sdCircle(p, .75), 0.);
  i -= circ;
  i = clamp(i, 0., 1.0);

  // add circle
  vec2 ln = p;
  ln += vec2(t,0.);
  ln *= r2d(PI/4.);
  lines = step(mod(ln.x, 0.2), 0.1);

  i += circ * lines;
  i = clamp(i, 0., 1.0);

  // cut out triangle
  vec2 rp = p;
  rp += vec2(0.0,0.3);
  rp *= r2d(PI/4.);
  rect = step(sdRect(rp, vec2(0.5)),0.) * step(-0.15, p.y);

  i -= rect;
  i = clamp(i, 0., 1.0);

  // add it back with stripes
  i += rect * step(mod(ln4.y+t, 0.2), 0.1);
  step(p.y, p.x*10.);


  gl_FragColor = vec4(vec3(i),1.);
}