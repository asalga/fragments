// 71 - "Sync"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

// float sdRect(vec2 p, vec2 )
float sdCircle(vec2 p, float r){
  return length(p)-r;
}
float circle(vec2 p, float r){
  return step(sdCircle(p, r),0.);
}
// from book of shaders
float sdRect(vec2 p, vec2 size){
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y), .0) + length(max(d,.0));
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time*2.;
  float _t = t;
  i += step(mod(p.y+ fract(t), .5), 0.25) * step(p.x, 0.0);
  i += step(mod(p.y+ fract(t*1.), .5), 0.25) * step(0.0, p.x);

  // if( mod( step(_t, 2.) == 0.){
    // i = abs(1. - i);
  // }
  float test = u_time;
  if( mod(test, 1.1) < .25){
     i = abs(1. - i);
  }

  gl_FragColor = vec4(vec3(i),1.);
}