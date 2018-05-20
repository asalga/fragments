precision mediump float;
uniform vec2 u_res;

float bricks(vec2 p, vec2 sz, float morterSz){
  float i;
  // morter lines for y
  // repeat SDF?

  // if y % 2 ==0
  float xOffset = step( mod(p.y, sz.y*2.), sz.y) * .25;

  float x = step(mod(p.x + xOffset, sz.x), sz.x-morterSz);
  float y = step(mod(p.y, sz.y), sz.y-morterSz);

  return x*y;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i = 0.;


  i = bricks(p, vec2(0.5, 0.1), 0.02);

  gl_FragColor = vec4(vec3(i),1.);
}