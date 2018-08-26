precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p,float r){
  return length(p)-r;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float i;
  float t = u_time;

  p*=2.;
  vec2 pos = vec2(cos(t),sin(t))*0.8;
  p+= pos;

  vec3 s = vec3(cos(t), sin(t), 0.);
  float z = sqrt(1. - pow(p.x,2.) - pow(p.y,2.));
  vec3 v = normalize(vec3(p.x, p.y, z)) * 0.13;


  float sz = 1.0;//0.01 + 0.125*(1.-sin(t)+1.)/2.;
  //0..0.125
  i = step(step(sdCircle((p), sz), .018) * dot(v, s), 0.1);

  gl_FragColor = vec4(vec3(i),1);
}