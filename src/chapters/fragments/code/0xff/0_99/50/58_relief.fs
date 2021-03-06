// 58 - "Relief" Based on shadertoy.com/view/4sXGRn
// CC-A-NC-ShareAlike 3.0 Unported License
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define TAU 3.141592658*2.
float sampleChecker(vec2 p){
  vec2 a = vec2(step(mod(p, 1.), vec2(.5)));
  return a.x == 0. && a.y == 0. ? 1. : a.x * a.y;//mix?
}
void main(){
  vec2 ar = vec2(1., u_res.y/u_res.x);
  vec2 p = ar*(gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time*.5;
  float angle = atan(p.y,p.x);
  float len = length(p);
  float num_reliefs = 4.;
  float fractT = fract(t/2.);
  float sharp = smoothstep(0.,.2,fractT)-
                smoothstep(.7,1.,fractT);
  // angle += .2 * len;//slight twirl?maybe?
  float h = sharp * cos(num_reliefs * angle);
  float s = smoothstep(.0, .5, h);
  float perspective = 1.5/(len + .1*s);
  float textureSwim = t*2.*step(mod(t,1.),.5);
  vec2 uv = vec2(t + perspective, angle/TAU + textureSwim);
  float col = sampleChecker(uv);
  // Still figurging out this part..
  float ao = smoothstep(0.,.3,h)-smoothstep(.15,1.,h);
  float aoDarkness = 1.;
  // darker closer to edges
  col *= 1.- (aoDarkness * ao * (pow(len,.7)));
  col *= pow(len, 2.5);// hide moire
  gl_FragColor = vec4(vec3(col), 1.0 );
}