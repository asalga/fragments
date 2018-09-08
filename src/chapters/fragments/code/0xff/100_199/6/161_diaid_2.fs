// 161 - "Diaid 2"
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

mat2 r2d(float a){
  return mat2(cos(a),sin(a),-sin(a),cos(a));
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res;
  float i;
  float t = u_time*2.;

  float cntPerSide = 1. + mod( (1. + floor(t/PI*2.)), 10.);

  vec2 c = vec2(1./cntPerSide);
  vec2 rp = mod(p, c) - c * 0.5;

  // alternate the rotation each cell
  vec2 id = floor(p*cntPerSide);
  if(mod(id.x, 2.) > .0){
    t = -t;
  }
  if(mod(id.y, 2.) > .0){
    t = -t;
  }

  rp *= r2d(t);

  i += step(sdRect(rp+vec2(-c.x/2.,   c.y/2.), c*.5), 0.) * 0.1;
  i += step(sdRect(rp+vec2(-c.x/2.,  -c.y/2.), c*.5), 0.) * 0.25;
  i += step(sdRect(rp+vec2(c.x/2.,   -c.y/2.), c*.5), 0.) * 0.75;
  i += step(sdRect(rp+vec2(c.x/2.,    c.y/2.), c*.5), 0.) * 1.0;

  gl_FragColor = vec4(vec3(i),1.);
}