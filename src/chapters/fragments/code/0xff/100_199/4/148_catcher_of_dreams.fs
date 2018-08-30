// 147 "Dreamcatcher"
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

float ringSDF(vec2 p, float r, float w){
  return abs(length(p) - (r*.5)) - w;
}

float sdLine(vec2 p, vec2 a, vec2 b, float r) {
    vec2 pa = p - a, ba = b - a;
    float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    return length( pa - ba*h ) - r;
}

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

float rand(float i, float scale){
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
    p = a + t * r;
    return true;
  }

  return false;
}

float rand(in vec2 p){
  return fract( sin(p.x * p.y) * 9072.23432);
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float t = u_time*.5;
  float c;

  float circ;
  circ = ringSDF(p, 1.6, 0.000001);
  c += 1./(pow(circ, 1.5)*2000.);

  const int CNT = 8;
  vec4 lines[CNT];
  vec2 speed[CNT];

  for(int it = 0; it < CNT; it++){
    float fit = float(it) + 1.;

    float theta0 = rand(fit,2703.6) * TAU + t * ((rand(fit,2663.3823)*2.)-1.);
    float theta1 = rand(fit,7093.6) * TAU + t*1.9 * ((rand(fit,2663.3823)*2.)-1.);

    lines[it].xy = normalize(vec2(cos(theta0), sin(theta0))) * 0.8;
    lines[it].zw = normalize(vec2(cos(theta1), sin(theta1))) * 0.8;

    // c += abs(1./(sdLine(p, lines[it].xy, lines[it].zw, 0.001) * 400.));
  }

  // for(int i = 0; i < CNT; ++i){
  //   vec2 p1 = lines[i].xy;
  //   vec2 p3 = lines[i].xy;
  //   // c += step(sdCircle(p1-p, 0.025), 0.)*0.25;
  //   // c += step(sdCircle(p3-p, 0.025), 0.)*0.25;
  // }


  float intersectionCount[CNT];

  float cSize = 0.015;
  for(int i = 0; i < CNT; ++i){
    for(int j = 0; j < CNT; ++j){
      if(i >= j) continue;
      // if(i == j)continue;

      // line 1
      vec2 p1 = lines[j].xy;
      vec2 p2 = lines[j].zw;

      // line 2
      vec2 p3 = lines[i].xy;
      vec2 p4 = lines[i].zw;

      vec2 ip;

      // if(line_line_intersection(p1, p2, p3, p4, ip)){
      if(line_line_intersection(p1, p2, p3, p4, ip)){

        float test = sdCircle(ip-p, cSize);
        c += abs(3./(test*1000.));
        intersectionCount[i]++;
      }
    }
  }


 for(int it = 0; it < CNT; it++){

    vec2 p1 = lines[it].xy;
    vec2 p2 = lines[it].zw;

    float intCnt = intersectionCount[it];
    c += abs(2./(sdLine(p, p1, p2, 0.001) * (2000. - intCnt*450.) ));

    // float test = sdCircle(ip-p, cSize);
    // c += abs(1./(test*1000.));
  }


  // for(int i = 0; i < CNT; ++i){
  //     vec2 p1 = lines[j].xy;
  //     vec2 p2 = lines[j].zw;

  //     // line 2
  //     vec2 p3 = lines[i].xy;
  //     vec2 p4 = lines[i].zw;

  //   float test = sdCircle(ip-p, cSize);
  //   c += abs(1./(test*1000.));
  // }

  gl_FragColor = vec4(vec3(c),1);
}