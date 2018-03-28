precision mediump float;
#define PI 3.141592658
#define ss smoothstep
uniform vec2 u_res;

float c(vec2 p, float r){
  vec2 a = vec2(u_res.x/u_res.y, 1.);
  vec2 c = a * ((gl_FragCoord.xy  / u_res) * 2. -1.);
  return 1. - smoothstep(a.x * r, a.x * r + 0.01, distance(p, c));
}

void main(){
  vec2 a = vec2(u_res.x/u_res.y, 1.);
  vec2 p = a * ((gl_FragCoord.xy/u_res) * 2. -1.);

  float i = c(vec2(0.), 1.) - c(vec2(0.), 0.98);

  // float d = length(p);
  // p = vec2(cos(0.)/2., sin(d)/2.) + p;
  float t = PI * length(p);
  p = vec2(cos(t) * p.x, sin(t) * p.y);
  

  // vec2 p1 = vec2(p);
  vec2 pn = normalize(vec2(p));
  float theta = acos(dot(vec2(1., 0.), pn));

  // first step is for angle, second is for keeping it inside circle // * (1. - ss(0.625, 0.628, length(p)));
  float blade = 1. - step(.5, theta); 
  i += blade;

  gl_FragColor = vec4(vec3(i), 1.);
}