precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdSquare(vec2 p, float halfW){
  vec2 _p = abs(p);
  return max(_p.x,_p.y)-halfW;
}
float square(vec2 p, float halfW){
  return smoothstep(0.01, 0.001, sdSquare(p, halfW));
}
float sdCircle(vec2 p, float r){
  return length(p)-r;
}
float circle(vec2 p, float r){
  return smoothstep(0.01, 0.001, sdCircle(p,r));
}
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
void main(){
  const int cnt = 16;
  float time = u_time*2.;
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  mat2 rot = r2d(time* 2.0);
  float sz = 0.45;
  float rad = 0.06;
  float i;

  for(int it=0;it<cnt;++it){
    float _it = float(it);
    float offset = (mod(_it,4.)/3.) *2. -1.;
    vec2 v, shift, nudge;

    if(it < 4){
       v = vec2(offset*sz, -sz);
       shift = vec2(0., 1.);
       nudge = vec2(0., -.2);
    }
    else if(it > 7 && it < 12){
       v = vec2(offset*sz, sz);
       shift = vec2(0., -1.);
       nudge = vec2(0., .2);
    } 
    float dist = sdSquare(v*rot, sz);
    shift *= dist*1.4;// + pow(1.1, dist);
    i += circle(p + v + shift + nudge, rad);
  }

  i += square(p * rot, sz);
  gl_FragColor = vec4(vec3(i),1.);
}