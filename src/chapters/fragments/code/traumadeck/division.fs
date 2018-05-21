// 12 - Division
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float cSDF(vec2 p, float r){
  return length(p)-r;
}
float cInter(vec2 p, float r, float i){
  float a = smoothstep(0.01, 0.001,cSDF(p+vec2(0.,-i), r));
  float b = smoothstep(0.01, 0.001,cSDF(p+vec2(0.,i), r));
  return a*b;
}

// position, radius, phase
float moon(vec2 p, float r, float phase){
  float a = step(cSDF(p,r), 0.);
  float b = step(cSDF(p+vec2(phase*r*2., 0.),r), 0.);
  return 1.-smoothstep(0.1, 0.01, a-b);
}

float ringSDF(vec2 p, float r, float w){
  return step(abs(length(p) - (r*.5)) - w, 0.);
}

float eye(vec2 p, vec2 pos){
  p/= 0.5;
  float i = cInter(p-vec2(0.,.6), .6, 0.3);
  i -= cInter(p-vec2(0.,.6), .55, 0.3);
  i += step(cSDF(p-vec2(0.,0.7), 0.2), 0.);
  return i;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  
  vec2 sunPos = p - vec2(-1., 1.6);
  vec2 moonPos = p - vec2(.5, -1.0);

  // SUN
  // float i = 1.-smoothstep(.23, .24,cSDF(p,.3));
  float onLeftSide = step(p.x, p.y);
  // i += step(cSDF(sunPos,.5), 0.);
  i = onLeftSide * smoothstep(0.,.1,sin(u_time*2.-atan(sunPos.y,sunPos.x) * 20.));
  i += eye(p+ vec2(0.7, -1.0),  vec2(0.));


  // MOON
  i += moon(moonPos, 0.4, .3);
  i += ringSDF(moonPos, 0.8, 0.01);
  i += step(cSDF(moonPos, 0.1), 0.);

  gl_FragColor = vec4(vec3(i), 1.);
}