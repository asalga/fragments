precision highp float;

uniform vec2 u_res;
uniform float u_time;

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

mat2 r2d(float a){
  float c = cos(a);
  float s = sin(a);
  return mat2(c,s,-s,c);
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float t = u_time;
  vec2 op = p;

  float i;
  float r = 0.2;
  float sz = 0.25;

  // p += vec2(sz/2.);
  vec2 cellID = floor(p/sz);

  vec2 c = vec2(sz);
  p = mod(p, c) - (0.5*c);

  p *= r2d(t + (cellID.x/11. + cellID.y/11.) );

  i = smoothstep(0.02, 0.000, sdCircle(p, r*2.));
  i -= smoothstep(0.02, 0.023, sdRect(p - vec2(0., r), vec2(r, r)));

  vec2 gr = vec2(p)*1.;
  gl_FragColor = vec4(vec3(i),1);
}