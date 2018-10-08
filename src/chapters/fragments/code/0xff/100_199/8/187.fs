precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdCircle(in vec2 p, in float r){
  return length(p)-r;
}

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float i;
  float t = u_time/2.;


  float circleMask = step(sdCircle(p, 1.), 0.);
  float linesDown = step(mod(p.y+t, 0.25), 0.125);
  linesDown *= circleMask;

  float sz = 0.17;
  vec2 pos = vec2(0.83);

  float linesUp = step(mod(p.y-t/2., 0.25/2.), 0.125/2.);

  float circleMinMask1 = linesUp * step(sdCircle(p + vec2( 1.,  1.) * pos, sz), 0.);
  float circleMinMask2 = linesUp * step(sdCircle(p + vec2(-1.,  1.) * pos, sz), 0.);
  float circleMinMask3 = linesUp * step(sdCircle(p + vec2(-1., -1.) * pos, sz), 0.);
  float circleMinMask4 = linesUp * step(sdCircle(p + vec2( 1., -1.) * pos, sz), 0.);

  i +=  linesDown +
        circleMinMask1 +
        circleMinMask2 +
        circleMinMask3 +
        circleMinMask4;

  gl_FragColor = vec4(vec3(i),1.);
}