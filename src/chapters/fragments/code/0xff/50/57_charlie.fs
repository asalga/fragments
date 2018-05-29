// 57 - "Charlie"
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define V2UP vec2(0.,1.)
#define V2_HALF vec2(0.,.5)
#define LINE(p0,p1,offset)smoothstep(.01,.001,line(p,v[p0]+offset,v[p1]+offset,.03));
#define LINE2(p0,p1)step(line(p,p0,p1,.03),0.);
mat2 r2d(float a){return mat2(cos(a),-sin(a),sin(a),cos(a));}
// book of shaders
float line(vec2 p, vec2 a, vec2 b, float r) {
  vec2 pa = p - a, ba = b - a;
  float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
  return length( pa - ba*h ) - r;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = 1.5* (a * (gl_FragCoord.xy/u_res*2.-1.));
  vec2 v[4];
  v[0] = vec2(-1., 0.); v[1] = vec2(0., 1.);
  v[2] = vec2( 1., 0.); v[3] = vec2(0.,-1.);
  for(int it=0;it<4;it++){// ROT
    v[it] *= r2d(u_time*PI);
    v[it].y = v[it].y * 0.25;
  }
  vec2 offset = vec2(0.,.5);// TOP  += and macro = :(
  float i = LINE(0,1,offset);  i += LINE(1,2,offset);
  i += LINE(2,3,offset);  i += LINE(3,0,offset);
  offset = vec2(0.,-.5);// BOTTOM
  i += LINE(0,1,offset);  i += LINE(1,2,offset);
  i += LINE(2,3,offset);  i += LINE(3,0,offset);
  for(int it=0;it<4;it++){// SIDES
    i += LINE2(v[it]+V2_HALF,v[it]-V2_HALF);
  }
  gl_FragColor = vec4(vec3(i),1.);
}