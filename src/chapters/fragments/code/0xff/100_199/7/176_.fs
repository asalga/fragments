// 166 - "Truchet Tiles"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
const float PI = 3.141592658;

float sdRing(vec2 p, float r, float w){
  return abs(length(p)- (r*.5)) - w;
}

float sdCircle(vec2 p, float r){
  return length(p)-r;
}
float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

mat2 r2d(float a){
  return mat2(cos(a),sin(a), -sin(a), cos(a));
}

float truchet(in vec2 p, float s){
  // if(s > 0.75){
    // p = vec2(-p.x, p.y);
  // }
  if(s > .5){
    p = vec2(p.x, -p.y);
  }
  //else if( s > .25){
    //p = -p;
  //}

  // float t = sin(u_time*0.5);
  // float a = smoothstep(2., 5., u_time) * PI/2.;
  float a = sin(u_time*2.)*PI*1.;
  // a = max(PI,a);

  a = max(a, 0.);
  a = min(a, PI/2.);

  p *= r2d(a);

  // a /= PI;
  float i;

  // i += step(sdRect(p + vec2(1.), vec2(1)), 0.);
  // i += step(sdRect(p - vec2(1.), vec2(1)), 0.);
  // i += step(sdRect(p - vec2(.5), vec2(0.5)), 0.);

  i += smoothstep(0.1, 0.01, sdRing(p + vec2(1), 2., .02));
  i += smoothstep(0.1, 0.01, sdRing(p - vec2(1), 2., .02));

  // i = i;
  // p *= 0.25;
  // p.x += 10.;
  // p *= -1.;
  // i += step(sdCircle(p + vec2(1.), 1.), 0.);
  // i += step(sdCircle(p - vec2(1.), .2), 0.);

  return i;
}


float truchet2(in vec2 p, float s){
  // if(s > 0.75){
    // p = vec2(-p.x, p.y);
  // }
  if(s > .5){
    p = vec2(p.x, -p.y);
  }
  //else if( s > .25){
    //p = -p;
  //}

  // float t = sin(u_time*0.5);
  // float a = smoothstep(2., 5., t) * PI/2.;
  // float a = sin(t)*PI;
  // p *= r2d(a);

  float i;


  // i += step(sdRect(p + vec2(1.), vec2(1)), 0.);


  // i += step(sdRect(p - vec2(.5), vec2(1)), 0.);

  // i += smoothstep(0.1, 0.01, sdRing(p + vec2(1), 2., .02));
  // i += smoothstep(0.1, 0.01, sdRing(p - vec2(1), 2., .02));


  p *= 1.25;
  i += smoothstep(0.1, 0.01, sdRing(p - vec2(.0), .707, .02));
  i += smoothstep(0.1, 0.01, sdRing(p + vec2(.125), .125, .02));

  // i += step(sdRect(p - vec2(.5), vec2(.5)), 0.);

  // p *= 1.25;
  // i += smoothstep(0.1, 0.01, sdRing(p - vec2(.25), .707, .02));
  // i += smoothstep(0.1, 0.01, sdRing(p + vec2(.25), .25, .02));

  // // p *= 0.25;
  // p.x += 10.;
  // p *= -1.;
  // i += step(sdCircle(p + vec2(1.), 1.), 0.);
  // i += step(sdCircle(p - vec2(1.), .2), 0.);

  return i;
}


float rand(vec2 p){
  vec2 s = vec2(5234.8093, 230.984);
  vec2 r = sin(p*s)*2034.234;
  return fract(r.x*r.y);
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float CNT = 3.;
  float i;
  float t = u_time;

  vec2 np = p;
  p += t/2.;
  // p.x += t*1.;

  vec2 id = floor(p*CNT);
  vec2 id2 = floor(p*10.);
  // id = vec2(0.);

  vec2 c = vec2(1./CNT);

  vec2 c2 = vec2(1./10.);
  vec2 p2 = mod(p, c)-0.5*c2;

  p = mod(p, c)-0.5*c;

  // float i = step(sdCircle(p, 1./CNT), 0.);
  i = truchet(p*CNT*2., rand(id));

  float a = sin(u_time*4.)*PI;
  a = max(a, 0.);
  a = min(a, PI/2.);
  a /= PI/2.;

  i = abs(a-i);

  // vec2 grid = fract(np*CNT);
  gl_FragColor = vec4(vec3(i),1.);
}