precision mediump float;
uniform vec2 u_res;

float rrectSDF(vec2 p, vec2 d, float r){
  vec2 diff = abs(p) - d;
  return max(diff.x, diff.y);
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float i = 0.;

i += step(rrectSDF(p+vec2(.5,-1.4),vec2(.25,.01),0.),0.);
i += step(rrectSDF(p+vec2(.5,0.),  vec2(.45,.05),0.),0.);
i += step(rrectSDF(p+vec2(.5,1.4), vec2(.25,.01),0.),0.);

i += step(rrectSDF(p+vec2(-.5,-1.),vec2(.3,.3),0.1),0.);
i += step(rrectSDF(p+vec2(-.5,0.), vec2(.3,.3),0.5),0.);
i += step(rrectSDF(p+vec2(-.5,1.), vec2(.3,.3),1.),0.);
// i += step(rrectSDF(p+vec2(.5,1.4), vec2(.25,.01),0.),0.);
// i += step(rrectSDF(p+vec2(.5,1.4), vec2(.25,.01),0.),0.);
  // i += step(rrectSDF(p, vec2(0.5, 0.1), 0.), 0.);


  gl_FragColor = vec4(vec3(i),1.);
}