precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float PI = 3.141592658;

float sampleChecker(vec2 c) {
  float col;
  float sz = 0.333;

  float x = step(mod(c.x,sz), sz/2.);
  float y = step(mod(c.y,sz), sz/2.);

  if(x == y) return 1.;
  return x*y;
}

float sdCircle(in vec2 p, float r){
  return length(p)-r;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float sz = 1./4.;
  float i;
  vec2 id = floor(p*10.)/10.;

  vec2 c = vec2(sz);
  vec2 rp = mod(p, c)-0.5*c;
  i = sampleChecker(p);

  i += step(sdCircle(p, 0.8), 0.);


  // i = step(sdCircle(rp, sz*0.25), 0.);
  // i = id.x * id.y;
  // if(id.x == id.y){
  //   i = 1.;
  // }



  gl_FragColor = vec4(vec3(i),1.);
}