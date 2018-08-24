precision mediump float;

uniform vec2 u_res;
uniform float u_time;


float capsule(vec2 p, vec2 a, vec2 b, float r) {
    vec2 pa = p - a, ba = b - a;
    float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    return length( pa - ba*h ) - r;
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
  return mat2(cos(a),sin(a),-sin(a),cos(a));
}

float f(float x){
  return fract(x);
}

void main(){
  float i = 0.15;
  float t = u_time*2.;
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float sz = 30.;

  vec2 p1 = floor(p*110.)/110.;

  float d=0.;

  for(int it = 0; it < 210; it++){
    float fit = float(it)/10.;
    float f = fract(t + 0.410);
     d += step(capsule(p - vec2(f,0.720) * fit,
                          vec2(0.120,0.070),
                            vec2(-0.160,-0.190), 0.01), 0.0);
  }

//   // p *= -t/2.;

//
//   // p1 = p;
//   vec2 p2 = floor(p*10.)/10.;
//   vec2 p3 = floor(p*20.)/20.;
//   vec2 p4 = floor(p*10.)/10.;

//   float s = u_time/2.;

//   float test = floor(0.707 +s);
//   float neg = 1.;
//   if(mod(test, 1.) < .5){
//     neg = -1.;
//   }  mat2 r = r2d(t);
// //*r2d(t*2.)
//   i += neg * step(sdRect(p1*r, vec2(f(0.707 + s))), 0.);


//   float test2 = floor(0.4 +s);
//   float neg2 = -1.;
//   if(mod(test2, 1.) < .5){
//     neg2 = 1.;
//   }
//    r = r2d(-t);
//   i += neg2 * step(sdRect(p2*r , vec2(f(0.1+s) )), 0.);


  // i += step(sdRect(p3*r2d(t*.5), vec2(0.3)), 0.);
  // i -= step(sdRect(p3*r2d(-t*.25), vec2(0.1)), 0.);

  // float circ = sdCircle(p, 0.95);
  // i = step(mix(r2, r, (sin(t)+1.)/2.), 0.);

  gl_FragColor = vec4(vec3(d),1);
}