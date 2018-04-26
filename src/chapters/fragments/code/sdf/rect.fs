// roundRectSDF Test

precision mediump float;
uniform vec2 u_res;

float rrectSDF(vec2 p, vec2 d, float r){
  vec2 diff = abs(p) - d;
  float test = length(p)-r;
  return min(max(diff.x, diff.y), test);
}

float roundRect(vec2 p, vec2 size, float radius) {
  vec2 d = abs(p) - size;
  return min(max(d.x, d.y), 0.0) + length(max(d,0.0))- radius;
}


float ellipse(vec2 p, vec2 r, float s) {
    return (length(p / r) - s);
}

 // return length(vec2(max(a.x-w,0.), a.y));

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float i = 0.;

i += step(rrectSDF(p+vec2(.5,-1.4),vec2(.25,.01),0.),0.);
i += step(rrectSDF(p+vec2(.5,0.),  vec2(.45,.05),0.),0.);
i += step(rrectSDF(p+vec2(.5,1.4), vec2(.25,.01),0.),0.);

i += step(rrectSDF(p+vec2(-.5,-1.),vec2(.3,.3),0.1),0.);
i += step(rrectSDF(p+vec2(-.5,0.), vec2(.3,.3),0.34),0.);
i += step(rrectSDF(p+vec2(-.5,1.), vec2(.3,.3),0.7),0.);

  gl_FragColor = vec4(vec3(i),1.);
}