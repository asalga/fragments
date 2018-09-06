precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

mat2 r2d(float a){
  return mat2(cos(a),sin(a),-sin(a),cos(a));
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float i;
  float t = u_time;

  vec2 c = vec2(0.5);
  vec2 rp = mod(p, c)-c*0.5;

  vec2 fp = floor(p*4.)/4.;

  if(mod(fp.x, 1.) < .5){
    t= -t;
  }
  if(mod(fp.y, 1.) < .5){
    t= -t;
  }

  rp *= r2d(t);

  i += step(sdRect(rp+vec2(-c.x/2.,   c.y/2.), c*0.5), 0.) * 0.1;
  i += step(sdRect(rp+vec2(-c.x/2.,  -c.y/2.), c*0.5), 0.) * 0.25;
  i += step(sdRect(rp+vec2(c.x/2.,   -c.y/2.), c*0.5), 0.) * 0.75;
  i += step(sdRect(rp+vec2(c.x/2.,    c.y/2.), c*0.5), 0.) * 1.0;

  float r = rp.x;
  gl_FragColor = vec4(vec3(i),1.);
}