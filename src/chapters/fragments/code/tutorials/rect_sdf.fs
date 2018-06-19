precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdRect1(vec2 p, vec2 sz){
  vec2 d = abs(p)-sz;
  return max(d.x,d.y);
}

// from book of shaders
float sdRect2(vec2 p, vec2 size){
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y), .0) + length(max(d,.0));
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;

  float b = sdRect2(p, vec2(0.5, 0.5));
  // float g = sdRect2(p, vec2(0.5, 0.5));

  float r = 0.;
  if(b < 0.){
  	b = 0.;
  	r = 1. - b;
  }

  gl_FragColor = vec4(vec3(r, 0., b),1.);
}