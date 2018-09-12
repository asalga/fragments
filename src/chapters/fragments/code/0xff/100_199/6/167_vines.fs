// 167 - "Vines"
// w/ pixelate
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
const float CNT = 10.;

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

mat2 r2d(float a){
  return mat2(cos(a), sin(a), -sin(a), cos(a));
}

void main(){
  // vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float t = u_time;
  float i;
  float sz = 0.125;// * 1.5;

  // i += step(sdCircle(p, sz), 0.);

  i = (1.-p.y)/2.;

  p -= vec2(0., 1.125);
  p.y = -p.y;

  float x = p.x - 1.;
  float y = p.y;
  float off;

  for(float j = 0.; j < 5.; j++){
    p.x = x + j * .5;
    p.y = y;
    off = j/2.;

    for(float it = CNT; it > 1.; it--){
      float sc = (it/CNT) * 2.;

      p *= r2d( sin( off*2. + t + it/8.) * .125 );
      p += vec2(0., -sz*1.);

      // p += vec2(0., -sz * 2. * it * sc);
      // i += step(sdCircle(p + vec2(0., -0.1), sz/2.), 0.);

      // if(it > 3.){
        // i -= step(sdRect(p, vec2(.025, sz/1.125)), 0.);

        i += step(sdRect(p, vec2(.025, sz*.5)), 0.) * (.1-y);
      // }
    }
  }


  gl_FragColor = vec4(vec3(i),1.);
}
