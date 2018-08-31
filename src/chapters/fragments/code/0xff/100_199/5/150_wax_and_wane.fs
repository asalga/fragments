// 150 - "wax & wane"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p,float r){
  return length(p)-r;
}

float sq(float x){
  return x*x;
}
float sdRing(vec2 p, float r, float w){
  return abs(length(p)- (r*.5)) - w;
}
void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  // vec2 op = p;
  // p = mod(p, vec2(1.))-vec2(.5);

  float t = u_time*.8;
  float sz = 0.8;
  vec3 l = vec3(cos(t), 0., sin(t));
  vec3 n = vec3(p.xy / sz, 0.);
  n.z = sqrt(1. - n.x*n.x - n.y*n.y);
  float i = dot(n,l);
  // float i = smoothstep(.0, .01, d);

  // make variations

  // vec2 id = floor(op);
  // if(id.x == -1.){
  //    i = d;
  // }

  // if(id.y == -1. && id.x > -1.){
  //   l = vec3(cos(t), 1., sin(t));
  //   i = smoothstep(.0, .01, dot(n,l));
  //   i += 1.- smoothstep(0., 0.01, sdRing(p, sz*2., .003));
  // }

  // if(id.x < 0. && id.y < 0.){
  //   l = vec3(cos(t), 1., sin(t));
  //   l = normalize(l);
  //   i = dot(n,l);
  //   i *= smoothstep(.1, 1., i);
  // }

  // smooth edges
  float len = length(p.xy) / sz;
  i *= 1.- smoothstep(0.99, 1., len);

  gl_FragColor = vec4(vec3(i),1);
}