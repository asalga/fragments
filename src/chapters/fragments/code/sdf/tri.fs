precision mediump float;

uniform vec2 u_res;
uniform float u_time;

// From shadertoy.com/view/4sSSzG
float sdTriangle(vec2 p, vec2 p0, vec2 p1, vec2 p2){
  vec3 e0, e1, e2;

  vec2 t = vec2(1., -1.);

  e0.xy = normalize(p1 - p0).yx * t;
  e1.xy = normalize(p2 - p1).yx * t;
  e2.xy = normalize(p0 - p2).yx * t;

  e0.z = dot(e0.xy, p0);
  e1.z = dot(e1.xy, p1);
  e2.z = dot(e2.xy, p2);

  float a = max(0.0, dot(e0.xy, p) - e0.z);
  float b = max(0.0, dot(e1.xy, p) - e1.z);
  float c = max(0.0, dot(e2.xy, p) - e2.z);

  return length(vec3(a, b, c));
}


void main(){
  vec2 ar = vec2(1., u_res.y/u_res.x);
  vec2 p = ar * (gl_FragCoord.xy/u_res*2.-1.);
  float i;

  vec2 a = vec2(.5, -.5);
  vec2 b = vec2(.0, .5);
  vec2 c = vec2(-.5, -.5);

  i = sdTriangle(p,a,b,c);
  i = step(i, 0.);
  gl_FragColor = vec4(vec3(i),1.);
}
