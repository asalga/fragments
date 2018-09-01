// 153 - "Alt Tunnel"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float PI = 3.141592659;
const float TAU = PI*2.;

// from byteblacksmith.com
highp float rand(vec2 co){
  highp float a = 12.9898;
  highp float b = 78.233;
  highp float c = 43758.5453;
  highp float dt= dot(co.xy ,vec2(a,b));
  highp float sn= mod(dt,3.14);
  return fract(sin(sn) * c);
}
float sdCircle(vec2 p, float r){
  return length(p)-r;
}
float circle(vec2 p, float r){
  return smoothstep(0.01, 0.001, sdCircle(p, r));
}
float sdSquare(vec2 p, float w){
  vec2 _p = abs(p);
  return max(_p.x, _p.y)-w;
}
float square(vec2 p, float w){
  return step(sdSquare(p, w), 0.);
}

mat2 r2d(float a){
  return mat2(cos(a), -sin(a), sin(a), cos(a));
}

float tex(vec2 p, float ti){
// void main(){
  // vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float i;
  float r = 0.08;
  float time = ti*1.;
  float flipSpeed = ti*6.;
  vec2 origP = p;

  p.y = mod(p.y, 0.2);
  p.y -= r*1.25;

 float ss = (sin(flipSpeed)+1.)/2.;

  for(int it=0;it<10;++it){
    float _i = ((float(it))/10.)*2.-1.;
    // fix
    float Xidx = floor( ((origP.x+1.)/2.) *10.)/10.;
    float Yidx = floor( ((origP.y+1.)/2.) *10.)/10.;
    vec2 idx = vec2(Xidx + 0., Yidx);

    if(rand(idx) < ss){
      // i = 0.;
      i += circle(p + vec2(_i + 0.1, 0.), r) -
           square(p + vec2(_i + 0.1, 0.), r/2.);
    }else{
      // i += square(p + vec2(_i + 0.1, 0.), r) -
           // circle(p + vec2(_i + 0.1, 0.), r/2.);
    }
  }

  ss = (sin(flipSpeed+PI)+1.)/2.;

  if(ss > .5){
    i = 1.-i;
    i *= 0.5;
  }

   // gl_FragColor = vec4(vec3(i), 1);
  return i;
}


void main() {
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float t = u_time*0.25;

  p *= r2d(t);

  float len = length(p);

  float r = (.5/len + t)*2.;
  float th = atan(p.y / p.x)/TAU;

  // float depthIndex = floor(10./(len*10.) )/100.;
  float depthIndex = floor(r*1. )/10.;
  th*=1.6;

  vec2 uv = vec2(r,th);
  uv = fract(uv);

  vec3 col = vec3(tex(uv, (t + depthIndex*10.) ));


  float fog = pow(len, 2.);
  // col += depthIndex/2.;
  col *= fog;

  gl_FragColor = vec4(col, 1);
}
