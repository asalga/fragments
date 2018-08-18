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
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float t = u_time;
  vec2 ip;
  vec2 m = vec2(u_mouse.x, 1.-u_mouse.y);

  // line 1
  // vec2 _l1p0 = vec2(0,.5);
  // float len = 2.;
  // vec2 _l1p1 = vec2(cos(t)*len, sin(t)*len);

  // vec2 _l2p0 = _l1p0;
  // vec2 _l2p1 = vec2(cos(t)*len, sin(t)*len);



  vec2 _p1 = vec2(0., .5);
  vec2 _p2 = vec2(0, -.5);
  vec2 _p3 = vec2(-0.707, 0.707);

  vec2 cell = floor(p*50.)/10.;

  float sz = .08;
  vec2 left_0 = cell;
  vec2 left_1 = cell + vec2(0, sz);

  vec2 top_0 = cell;
  vec2 top_1 = cell + vec2(sz, 0.);

  vec2 right_0 = cell + vec2(sz, 0.);
  vec2 right_1 = right_0 + vec2(0, sz);

  vec2 bot_0 = cell + vec2(0, sz);
  vec2 bot_1 = bot_0 + vec2(sz, 0);

  float i;
  // if(line_line_intersection(left_0,left_1, _p1, _p2, ip) ){
  //   i = 1.;
  // }
  // else if(line_line_intersection(top_0,top_1, _p1, _p2, ip) ){
  //   i = 1.;
  // }
  // else if(line_line_intersection(right_0,right_1, _p1, _p2, ip) ){
  //   i = 1.;
  // }
  // else if(line_line_intersection(bot_0,bot_1, _p1, _p2, ip) ){
  //   i = 1.;
  // }

  if(line_line_intersection(left_0,left_1, _p1, _p2, ip) ){
    i = 1.;
  }
  else if(line_line_intersection(top_0,top_1, _p1, _p2, ip) ){
    i = 1.;
  }
  else if(line_line_intersection(right_0,right_1, _p1, _p2, ip) ){
    i = 1.;
  }
  else if(line_line_intersection(bot_0,bot_1, _p1, _p2, ip) ){
    i = 1.;
  }
  gl_FragColor = vec4(vec3(i),1);
}









