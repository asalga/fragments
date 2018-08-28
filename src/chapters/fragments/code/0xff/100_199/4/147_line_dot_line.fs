// 139
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
uniform vec2 u_mouse;

const float PI = 3.141592658;
const float HALF_PI = PI/8.;
const float TAU = PI*2.;

float cross2d(vec2 a, vec2 b){
  return a.x*b.y-a.y*b.x;
}

float sdLine(vec2 p, vec2 a, vec2 b, float r) {
    vec2 pa = p - a, ba = b - a;
    float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    return length( pa - ba*h ) - r;
}

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

float valueNoise(float i, float scale){
  return fract(sin(i*scale));
}

float random(vec2 p){
  const float X_SCALE = 456834.2258429;
  const float Y_SCALE = 114362.3802242;
  float r = sin(p.x*X_SCALE + p.y*Y_SCALE) * 7191424.;
  return fract(r);
}

bool line_line_intersection(vec2 a, vec2 b, vec2 c, vec2 d, out vec2 p){
  vec2 r = b-a;
  vec2 s = d-c;

  float d_ = cross2d(r,s);
  float u = ((c.x - a.x) * r.y - (c.y-a.y)*r.x)/d_;
  float t = ((c.x - a.x) * s.y - (c.y-a.y)*s.x)/d_;

  if( u >= 0. && u <= 1. && t >= 0. && t <= 1.){
    p = a + t*r;
    return true;
  }

  return false;
}

float rand(in vec2 p){
  return fract( sin(p.x * p.y) * 9072.23432);
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float t = u_time*0.25;
  float c;

  c +=  step(sdCircle(p, 1.015), 0.);
  c -=  step(sdCircle(p, 0.96), 0.);

  const int CNT = 10;
  vec4 lines[CNT];

  for(int it = 0; it < CNT; it++){
    float fit = float(it);

    float r = (fit/9.) * TAU;
    lines[it].xy = vec2(cos(r), sin(r));
    lines[it].zw = -lines[it].xy;

    lines[it].x += sin(t+ fit);
    lines[it].y += sin(t - fit*2.);

    lines[it].w -= sin(t- fit*3.);
    lines[it].z += cos(fit*fit* 14. + fit*12.);

    lines[it].xy = normalize(lines[it].xy)*0.99;
    lines[it].zw = normalize(lines[it].zw)*0.99;

    c += step(sdLine(p, lines[it].xy, lines[it].zw, 0.004), 0.);
  }


  // for(int i = 0; i < CNT; ++i){
  //   vec2 p1 = lines[i].xy;
  //   vec2 p3 = lines[i].xy;
  //   // c += step(sdCircle(p1-p, 0.025), 0.)*0.25;
  //   // c += step(sdCircle(p3-p, 0.025), 0.)*0.25;
  // }


  for(int i = 0; i < CNT; ++i){
    for(int j = 0; j < CNT; ++j){
      // if(i > j) continue;
      if(i == j)continue;

      // line 1
      vec2 p1 = lines[j].xy;
      vec2 p2 = lines[j].zw;

      // line 2
      vec2 p3 = lines[i].xy;
      vec2 p4 = lines[i].zw;

      vec2 ip;
      // if(line_line_intersection(p1, p2, p3, p4, ip)){
      if(line_line_intersection(p1, p2, p3, p4, ip)){
        float test = sdCircle(ip-p, 0.02);
        c += smoothstep(0.01, .000, test);
      }
    }
  }


  gl_FragColor = vec4(vec3(c),1);
}