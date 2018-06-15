precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658

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
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float r = 0.08;
  float time = u_time*.0000001;
  float flipSpeed = u_time*4.;
  vec2 origP = p;

  p.y = mod(p.y, 0.2);
  p.y -= r*1.25;

  for(int it=0;it<10;++it){
    float _i = ((float(it))/10.)*2.-1.;
    // fix
    float Xidx = floor( ((origP.x+1.)/2.) *10.)/10.;
    float Yidx = floor( ((origP.y+1.)/2.) *10.)/10.;
    vec2 idx = vec2(Xidx + time, Yidx);
    
    if(rand(idx) < (sin(flipSpeed)+1.)/2.){
      i += circle(p + vec2(_i + 0.1, 0.), r) - 
           square(p + vec2(_i + 0.1, 0.), r/2.);
    }else{
      i += square(p + vec2(_i + 0.1, 0.), r) -
           circle(p + vec2(_i + 0.1, 0.), r/2.);
    }
  }
  gl_FragColor = vec4(vec3(i),1.);
}