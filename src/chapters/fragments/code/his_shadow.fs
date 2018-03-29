precision mediump float;
#define PI 3.141592658
uniform vec2 u_res;
uniform float u_time;

float c(vec2 p, float r){
  vec2 a = vec2(u_res.x/u_res.y, 1.);
  vec2 c = a * ((gl_FragCoord.xy  / u_res) * 2. -1.);
  
  vec2 nc = normalize(c);
  float theta = acos(dot(nc, vec2(0.0, 1.0))) * 40.0;
  // c.x += cos(theta)/50.;
  // c.y += sin(theta)/50.;
  return step(distance(p, c), a.x * r);
}

void main(){
  vec2 a = vec2(u_res.x/u_res.y, 1.);
  vec2 p = a * ((gl_FragCoord.xy/u_res) * 2. -1.);
  float i = c(vec2(0.), 1.) - c(vec2(0.), 0.98);

  // float d = length(p);
  // p = vec2(cos(0.)/2., sin(d)/2.) + p;
  // float t = distance(p, vec2(0.)); // * u_time * 10.;
  // p += normalize(p) * vec2(cos(t), sin(t));
  // vec2 nc = normalize(c);
  // float theta = acos(dot(nc, vec2(0.0, 1.0))) * 40.;
  // c.x -= cos(theta)/50.;
  // c.y -= sin(theta)/50.;

  float l = length(p) * 25.;
  vec2 pn = normalize(p);
  float theta = acos(dot(pn, vec2(1., 0.))) * l;
  // if(theta < 0.15){
  //   discard;
  // }

  // p.x += cos(theta)/1.1;
  // p.y += sin(theta)/1.1;

  // pn = normalize(p);
  float theta2 = atan(p.y, p.x);
  // acos(dot(pn, vec2(1., 0.)));
 
  //float theta2 = acos(dot(pn, vec2(1., 0.))) * 1.;
  //i += step(.5,p.x);
  // first step is for angle, second is for keeping it inside circle // * (1. - ss(0.625, 0.628, length(p)));
  float blade = (1. - step(.15, theta2));
  // float c1 = blade;

  gl_FragColor = vec4(vec3(blade), 1.);
}