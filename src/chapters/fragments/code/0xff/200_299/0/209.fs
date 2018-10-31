precision mediump float;
uniform vec2 u_res;
uniform float u_time;
const float PI = 3.141592658;

float cSDF(vec2 p, float r){
  return length(p)-r;
}
void main(){
  // vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time;
  p.x += step(p.y,0.)*.05 * sin(5.*u_time+p.y*99.) * 2. * p.y;// + sin(t)*1.;

  float i = 1.-smoothstep(.0,.01, cSDF(p,.8));
  i = abs(step(0., p.y)-i);


  float len = .5 - length(p);


  float a = (atan(p.y, p.x)+PI)/(PI*2.);

  i = sin(len * 20.) * sin(a*PI*10.);
  i = step(i,0.15);

  if(p.y < .0){
    i *= pow(p.y,.91);
  }

  gl_FragColor = vec4(vec3(i), 1.);
}