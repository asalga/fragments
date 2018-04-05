precision mediump float;
uniform vec2 u_res;
uniform float u_time;

float square(vec2 p, float size){
  // Does this fragment fall inside the square?  
  // determine which side of the square this fragment is on
  // if x > y then we're on the x side
  // if y > x then w'ere on the y side
  vec2 absValue = abs(p);
  float side = max(absValue.x, absValue.y);

  // return 0 if a > b
  // return 1 if a < b
  return step(side, size);
}

float rectSDF(vec2 p, vec2 size){
  vec2 absValue = abs(p / size);
  float side = max(absValue.x, absValue.y);
  return step(side, size.x) + step(side, size.y);
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float h = 0.08;

  float t = u_time;
  p.y += sin(p.x* 5. + t) * 0.15;
  // float s = sin((p.x + t) * 2.) * 0.5;
  // float waveSize = 0.2;
  
  float c = rectSDF(vec2(p.x, p.y - .25), vec2(0.8, h)) +
  			rectSDF(vec2(p.x, p.y - .0), vec2(0.8, h)) + 
  			rectSDF(vec2(p.x, p.y + .25), vec2(0.8, h));

  gl_FragColor = vec4(vec3(c),1.);
}