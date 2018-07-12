precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sampleChecker(vec2 p){
  vec2 a = vec2(step(fract(p), vec2(.5)));
  return a.x == 0. && a.y == 0. ? 1. : a.x * a.y;//mix?
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time*3.;

  float x = fract(p.x / abs(p.y*2.) + t/20.);
  float y = fract(abs(1./p.y) + t);
  i = sampleChecker(vec2(x,y));

  // fog to hide artefacts @ center
  i *= pow( abs(p.y), 2.4)*2.;

  gl_FragColor = vec4(vec3(i),1.);
}