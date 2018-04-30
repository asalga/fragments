precision mediump float;
uniform vec2 u_res;
uniform vec3 u_mouse;

float cSDF(vec2 p, float r){
  return length(p)-r;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  vec2 m = (gl_FragCoord.xy/u_mouse.xy*2.-1.);
  m.y*=-1.;

  float rad = 0.15;
  vec2 ct = floor(p/.1)*0.5 - (.5*rad);
  // float x = floor(p.x/10.)*.1 - (.5*rad);
  // vec2 ct = vec2(x, p.y);

  float i = step(length(m-ct), 0.3);

  
  // vec2 offset = vec2(.0, .4);
  // // p = p + offset;

  // float theta = atan(p.y, p.x);
  // float len = length(p-offset);
  // float x = cos(theta) * len;
  // float y = sin(theta) * len;

  // float s = step(mod(len, PI/8.), PI/16.);

  // i += s * step(cSDF(p, 1.), 0.);

  // i *= step(cSDF(p+offset, 0.5), 0.);

  // float st = step(mod(p.x, 0.1), 0.05);
  // i+= st;

  // i += step(cSDF(p- vec2(st, p.y), 0.3), 0.0);

// modding the position/translation
// why can't I mod the polar distance?


  // r = a(theta)
  // r = a(theta^1/n)

  // r = radial distance / length
  // a
  // theta = polar angle
  // n = how tightly wrapped

  // if the pixel exits in the
  // arm of the spiral.

  gl_FragColor = vec4(vec3(i), 1.);
}