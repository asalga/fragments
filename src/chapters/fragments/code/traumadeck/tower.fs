
precision mediump float;
uniform vec2 u_res;

//from book of shaders
float rectSDF(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x, d.y), 0.0) + length(max(d,0.0));
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  vec2 towerPos = p+ vec2(0.0, 0.1);
  float i = 0.;

  // tower body
  vec2 towerBodySz = vec2(0.3, .7);
  i += step(rectSDF(towerPos+ vec2(0., 0.7), towerBodySz), 0.);

  // tower top
  vec2 towerTopSz = vec2(0.45, .12);
  i += step(rectSDF(towerPos-vec2(0.,.1), towerTopSz), 0.);



  gl_FragColor = vec4(vec3(i),.4);
}