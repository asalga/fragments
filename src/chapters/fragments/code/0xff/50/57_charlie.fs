// 57 - "Charlie"
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define Z 0.0
#define O 1.0
#define V2UP vec2(0., 1.)
// #define LINE(p0, p1) step(line(p,p0,p1,.03),0.);
#define LINE(p0,p1,offset)step(line(p,v[p0]+offset,v[p1]+offset,.03),0.);
#define LINE(p0,p1)step(line(p,p0,p1,.03),0.);
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
// book of shaders
float line(vec2 p, vec2 a, vec2 b, float r) {
  vec2 pa = p - a, ba = b - a;
  float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
  return length( pa - ba*h ) - r;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time;
  p *= 1.5;
  vec2 v[4];
  v[0] = vec2(-O, Z); v[1] = vec2( Z, O);
  v[2] = vec2( O, Z); v[3] = vec2( Z,-O);
  for(int it=0;it<4;it++){// ROT
    v[it] *= r2d(t*PI);
    v[it].y = v[it].y * 0.25 + 0.5;
  }
  // TOP  += and the macro don't work ???
  vec2 offset = vec2(0.,.5);
  i += LINE(0,1,offset);  i += LINE(1,2,offset);
  i += LINE(2,3,offset);  i += LINE(3,0,offset);
  // BOTTOM
  offset = vec2(0.,-.5);
  i += LINE(0,1,offset);  i += LINE(1,2,offset);
  i += LINE(2,3,offset);  i += LINE(3,0,offset);
  
  i += LINE(0,0,offset);  i += LINE(1,1,offset);
  i += LINE(2,2,offset);  i += LINE(3,3,offset);

  gl_FragColor = vec4(vec3(i),1.);
}

  // SIDES
  // for(int it=0;it<4;it++){
  //   i += LINE(v[it], v[it]-V2UP);
  // }
