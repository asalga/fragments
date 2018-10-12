//Tmu Masn
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
uniform vec2 u_mouse;
uniform sampler2D u_t0;
const float PI = 3.141592658;


const float Tau   = 3.2832;
const float speed = -.02;
const float density = .04;
const float shape = .04;

float random( vec2 seed ) {
  return fract(sin(seed.x+seed.y*1e3)*1e5);
}
float valueNoise(float i, float scale){
  return fract(sin(i*scale));
}
float Cell(vec2 coord) {
  vec2 cell = fract(coord) * vec2(1.5,2.) - vec2(.0,.25);
  return (1.-length(cell*2.-1.))*step(random(floor(coord)),density)*2.;
}

void main( void ) {
  float t = u_time;
  vec2 p = (gl_FragCoord.xy / u_res)*2.-1.;//  - u_mouse.xy;

  float a = fract(atan(p.x, p.y+t) / Tau);
  float d = length(a);

  vec2 coord = vec2(pow(d, shape), a)*26.;
  vec2 delta = vec2(u_time*speed*15., .5);

  float c = 0.;
  // for(int i=0; i<14; i++) {
  //  coord += delta;
  //  c = max(c, Cell(coord));
  // }
  vec4 final;// =  vec4(c*d);
  // final.xyz = ((vec3(1.)-final.xyz));

  // final.yz = vec2(0.);
   // final.x = abs(1. / (c*p.x * 10000. ));

  float vn = random(p /sin(t));//+vec2(t/100000., 1.));
  float co = abs(1./(( fract( (p.y+vn)/2.+t )) * 10. * vn) );

  co *= 1.2 + fract(abs(sin(u_time)*100.));
  // co -= pow(length(p)+0.3, 30.);
  co = texture2D(u_t0, gl_FragCoord.xy/u_res ).r;// + co;

  co = 1.-co;

  // if()

  gl_FragColor = vec4(vec3(co), 1.);
}
