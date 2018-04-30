// afots
precision mediump float;
uniform vec2 u_res;
#define PI 3.141592658

float ring(vec2 p, float r){
  float l = length(p);
  float ss = 0.01;
  float thickness = 0.05;
  return smoothstep(r, r + ss, l) * 
  		 smoothstep(l, l + ss, r + thickness);
}

float rect(vec2 p, vec2 dims){
  vec2 scaledP = p / dims;
  vec2 a = abs(scaledP);
  float v = max(a.x, a.y);
  return step(v, dims.x) + step(v,  dims.y);
}

vec2 rot(vec2 p, float theta){
  return p * mat2( cos(theta), -sin(theta),
                   sin(theta), cos(theta));
}

void main(){
  vec2 a = vec2(1., u_res.y / u_res.x);
  vec2 p = a * (gl_FragCoord.xy / u_res * 2. - 1.);
  float s = 0.75;
  float theta = PI/4.;
  float c = ring(p, s) + 
  			rect(rot(p, theta), vec2(s, .1)) +
  			rect(rot(p, -theta), vec2(s, .1));
  gl_FragColor = vec4(vec3(c), 1.0);
}
