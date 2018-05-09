precision mediump float;
uniform vec2 u_res;
uniform float u_time;

float cSDF(vec2 p, float r){
  return length(p)-r;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  
  float c1 = cSDF(p, 0.5);

  // circle circle intersection

  float i = 1.;
  gl_FragColor = vec4(vec3(c1), 1.);
}