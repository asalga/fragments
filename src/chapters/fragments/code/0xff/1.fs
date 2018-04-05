precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.14159
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
  float t = u_time * 3.25;
  p.y += sin(p.x * 7.0 + t) * 0.1;
  
  float c = rectSDF(vec2(p.x, p.y - .25), vec2(0.8, h)) + 
  			rectSDF(vec2(p.x, p.y - .0), vec2(0.8, h)) + 
  			rectSDF(vec2(p.x, p.y + .25), vec2(0.8, h));

  gl_FragColor = vec4(vec3(c),1.);
}