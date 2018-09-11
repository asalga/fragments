precision highp float;

uniform vec2 u_res;
uniform float u_time;

const float PI = 3.141592658;

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

float pac(vec2 pos, float sz, float m){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  p.x = 1.-p.x;

  float i;
  vec2 newP = p+pos;
  float t = u_time*0.5;

  float body = step(sdCircle(newP, sz), 0.);

  float theta = abs(atan(newP.y, newP.x))/PI;

  // sin -1..1
  // +1 => 0..2
  // /2 => 0..1
  float s = (sin(t * 3. * PI)+1.)/2. * .25;

  float mouth = step(.25, theta + s);

  return body * mouth;
}


void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float t = u_time * 0.25;
  float i=0.;

  // 1 .. 0
  // vec2 pac1_pos = -vec2(2. - fract(t+2.5) * 2.5, 0.);
  // float pac1_sz = 0.5;
  // i += step(sdCircle(p - pac1_pos, pac1_sz), 0.);

  vec2 pac2_pos = vec2(-fract(t*PI) * 2.5, 0.);
  float pac2_sz = 0.8;
  //1.-abs(pac2_pos.x) + 0.25;
  i = pac(pac2_pos, pac2_sz, 0.);

  gl_FragColor = vec4(vec3(i),1);
}