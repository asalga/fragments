precision mediump float;
uniform float u_time;
uniform vec2 u_res;
#define EPSILON 0.02

float circle(vec2 p, float r){
  // len must be less than radius or radius must be > length  
  float plen = length(p);
  float fill = step(plen, r);
  fill *= step(r - EPSILON, plen);
  return fill;
}

float wavycircle(vec2 p, float r){
  float theta = atan(p.y, p.x);
  vec2 pn = normalize(p);
  pn *= sin(theta * 15.)/10.0;
  float plen = length(p + pn);
  return 1. - step(r, plen) * step(0.5, length(p));
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * ((gl_FragCoord.xy / u_res) * 2. -1.);
  // vec2 off = p + vec2( cos(u_time*speed),  sin(u_time*speed) );
  float i = wavycircle(p, 0.5);
  gl_FragColor = vec4(vec3(i), 1.0);
}

// float c(vec2 p, float r){
//   vec2 a = vec2(u_res.x/u_res.y, 1.);
//   vec2 c = ((gl_FragCoord.xy / u_res) * 2. -1.);
//   float theta = sin(atan(c.y, c.x));
  
//   // vec2 nc = normalize(c);
//   // float theta = acos(dot(nc, vec2(0.0, 1.0)));
  
  
//   // c.x = c.x * cos(theta * 10.)*.53;//30.;
//   // c.y = c.y * sin(theta * 10.)*.53;///30.;

//   float d = theta * 20.;
//   c += vec2(cos(d), sin(d)) * 0.2;

//   // return 1. - smoothstep(a.x * r, a.x * r + 0.01, distance(p, c));
//   return step(r, length(c));
// }

// void main(){
//   vec2 a = vec2(u_res.x/u_res.y, 1.);
//   //vec2 p = a * ((gl_FragCoord.xy/u_res) * 2. -1.);

//   float i = c(vec2(0.), .3);// - c(vec2(0.), 0.98);

//   // float d = length(p);
//   // p = vec2(cos(0.)/2., sin(d)/2.) + p;
//   // float t = distance(p, vec2(0.)); // * u_time * 10.;
//   // p += normalize(p) * vec2(cos(t), sin(t));

//   // vec2 p1 = vec2(p);
//   // vec2 pn = normalize(vec2(p));
//   // float theta = acos(dot(vec2(1., 0.), pn));

//   // first step is for angle, second is for keeping it inside circle 
//   // * (1. - ss(0.625, 0.628, length(p)));
  
//   //float blade = 1. - step(.5, theta); 
//   //i += blade;

//   gl_FragColor = vec4(vec3(i), 1.);
// }