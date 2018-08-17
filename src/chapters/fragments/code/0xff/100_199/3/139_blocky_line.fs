// 139
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
uniform vec2 u_mouse;

float cross2d(vec2 a, vec2 b){
  return a.x*b.y-a.y*b.x;
}

bool line_line_intersection(vec2 a, vec2 b, vec2 c, vec2 d, out vec2 p){
  vec2 r = b-a;
  vec2 s = d-c;

  float d_ = cross2d(r,s);
  float u = (c.x - a.x * r.y - (c.y-a.y)*r.x)/d_;
  float t = (c.x - a.x * s.y - (c.y-a.y)*s.x)/d_;

  if( u >= 0. && u <= 1. && t >= 0. && t <= 1.){
    p = a + t*r;
    return true;
  }

  return false;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res;
  float i;
  vec2 inter;
  vec2 m = u_mouse.xy;
  m.y = 1.-m.y;
  float t = u_time;

  vec2 a = vec2(0,0);
  vec2 b = m;

  vec2 c = vec2(sin(t),0);
  vec2 d = vec2(0,1);
  vec2 ip;

  vec2 left_0;
  vec2 left_1;

  vec2 cell = floor(p*10.)/10.;

  left_0 = cell;
  left_1 = cell + vec2(0, 1.);


  // if(line_line_intersection(a,b,c,d,ip) ){
  //   i = 1.;
  // }

  if(line_line_intersection(left_0,left_1,c,d,ip) ){
    i = 1.;
  }

  gl_FragColor = vec4(vec3(i),1);
}









