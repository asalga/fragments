precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.14159

float cSDF(vec2 p, float r){
  return length(p) - r;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);

  // vec2 pt = mod(p,);
  // vec2 _p = gl_FragCoord.xy;
  // vec2 _lineWidthInPx = vec2(1.);
  // vec2 _cellSize = vec2(50.);
  // vec2 _i = step(mod(_p, _cellSize), _lineWidthInPx);
  // float theta = ((atan(p.y,p.x)/PI)+1.)/2.;

  float theta = atan(p.y,p.x)*PI*2.;

  float m = step(mod(theta, 2.), .85);
  float r = .7;

  vec2 Pos = vec2(cos(m),sin(m))*r;

	p += vec2(cos(m),sin(m));

  // gl_FragColor = vec4(vec3(i.x+i.y), 1.);
  // p.x = p.x + (cos(u_time*2.) * p.y);
  // p.y = p.y + (sin(u_time*-4.) * p.x);

  float i = step(cSDF(p, .02), 0.4);


  // float i = cSDF(p * Pos, .5);

  // i += _i.x + _i.y;
  gl_FragColor = vec4(vec3(i), 1.);
}
