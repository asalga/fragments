// sdfs

precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p, float r){
  return length(p)-r;
}
float sdSquare(vec2 p, float w){
  vec2 _p = abs(p);
  return max(_p.x,_p.y) - w;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;

  // i += step(sdCircle(p, 0.25), 0.);
  i = step(sdSquare(p, .25), 0.);

  gl_FragColor = vec4(vec3(i),1.);
}