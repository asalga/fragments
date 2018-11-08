precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res;
  float i;
  float sz = 1./4.;
  float t = u_time * 0.5;

  float ft = fract(t);

  vec2 c = vec2(0., -1.0 - sz/2.);
  vec2 fc = vec2(0., fract(t) * sz*3.);

  vec2 start = vec2(0., 0.25);
  vec2 displace = vec2(0., fract(t)*sz);

  float curr = step(sdRect(p + c + fc, vec2(1., sz/2.)), 0.);
  float total = step(sdRect(p - start + displace, vec2(1., sz*1.)), 0.);

  i = total + curr;
  gl_FragColor = vec4(vec3(i),1.);
}