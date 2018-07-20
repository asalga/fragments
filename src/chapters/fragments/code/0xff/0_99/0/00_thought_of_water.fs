// 0 -thought of water
precision mediump float;
uniform vec2 u_res;
uniform float u_time;

float rectSDF(vec2 p, vec2 size){
  vec2 absValue = abs(p / size);
  float side = max(absValue.x, absValue.y);
  return step(side, size.x) + step(side, size.y);
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float h = 0.08;
  float t = u_time * 4.;
  
  float y1 = p.y + sin(p.x * 10.0 + t) * h;

  float c = rectSDF(vec2(p.x, y1 - .25), vec2(0.8, h)) + 
  			rectSDF(vec2(p.x, y1), vec2(0.8, h)) + 
  			rectSDF(vec2(p.x, y1 + .25), vec2(0.8, h));
  gl_FragColor = vec4(vec3(c),1.);
}