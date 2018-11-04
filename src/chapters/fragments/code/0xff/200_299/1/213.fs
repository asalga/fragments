precision mediump float;

uniform vec2 u_res;
uniform float u_time;
const float PI = 3.14159;

float sampleChecker(vec2 c) {
  float col;
  float sz = 0.25;

  float x = step(mod(c.x,sz), sz/2.);
  float y = step(mod(c.y,sz), sz/2.);

  if(x == y){return 1.;}
  return x*y;
}

float sdCircle(vec2 p, float r){
  return length(p) - r;
}

void rot(inout vec2 p, in float a){
  float c = cos(a);
  float s = sin(a);

  mat2 rot = mat2(c,-s,s,c);
  p*=rot;
}

float samplePolarChecker(vec2 c){
  float t = u_time*2.;

  float r = pow( length(c), 2.) + t;
  float sz = 3.;
  float rLen = step(mod(r, sz), sz/2.);
  float angle = step(mod(atan(c.x,c.y)+PI, PI/5.), .3);

  float fog = pow(length(c)/2., 4.);
  if(rLen == angle){return 0.8* fog;}
  return rLen*angle * fog;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float i;
  float v, h;
  float t = u_time * .15;
  // t = floor(t*10.)/10.;
  // float si = (sin(t)+1.)/2. + 1.;
  // float co = (cos(t)+1.)/2. + 1.;
  float si = 2.;
  float co = 1.;

  p/=1.5;
  vec2 s = vec2(si,co);
  vec2 shift = vec2(0., t);

  vec2 vert = p * s.xy+ shift.xy;
  vec2 hori = p * s.yx + shift.yx;


  v = sampleChecker(vert) - smoothstep(0.25, 0.252, sdCircle(p, 0.25));
  h = sampleChecker(hori) * smoothstep(0.25, 0.252, sdCircle(p, 0.25));


  float fadeCircle = 1.-smoothstep(0.005, 0.95, sdCircle(p, 0.25));

  i += h;

  // i *= fadeCircle;
  i += v * fadeCircle;
  // i *= v;

  gl_FragColor = vec4(vec3(i), 1);
}