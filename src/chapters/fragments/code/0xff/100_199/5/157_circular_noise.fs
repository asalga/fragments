precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float PI = 3.141592658;
const float TAU = PI*2.;

mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
float sdLine(vec2 p, vec2 a, vec2 b, float r) {
    vec2 pa = p - a, ba = b - a;
    float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    return length( pa - ba*h ) - r;
}
float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;

  return fract( sin(x+y) * 23454.);
}

float smoothValueNoise(vec2 p){
  vec2 lv = fract(p);
  lv = lv*lv*(3.-2.*lv);
  vec2 id = floor(p);
  float bl = valueNoise(vec2(id));
  float br = valueNoise(vec2(id)+vec2(1,0));
  float b = mix(bl,br,lv.x);

  float tl = valueNoise(vec2(id)+vec2(0,1));
  float tr = valueNoise(vec2(id)+vec2(1,1));
  float t = mix(tl,tr,lv.x);

  return mix(b,t,lv.y);
}

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

float sdRing(vec2 p, float r, float w){
  return abs(length(p)- (r*.5)) - w;
}


// float sdRing(vec2 p, float r1, float r2){
//   float u = 0.0;
//   float l = 0.01;
//   const float D = 0.005;

//   float c = smoothstep(l, u, sdCircle(p, r1)) -
//             smoothstep(l-D, u-D, sdCircle(p, r2));
//   return c * smoothstep(1., 0., r1*2.);
// }

const float BANDS = 5.;

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float c;
  float ti = u_time * 1.0;

  float density = 2.5;

  float a = ((atan(p.y,-p.x)/PI)+1.)/2.;
  // float r = length(p)*density + a;
  // float i = step(mod(r, 1.), 0.5);
  // gl_FragColor = vec4(vec3(i), 1.);


  vec2 op = p*r2d( PI/3.);

  p *= length(p);

  float theta = atan(p.y, p.x);
  float r = length(p) * density - ti;

  vec2 pc = vec2(r,theta);
  float n;
  ti = 0.;
  n += smoothValueNoise(pc*4.          );
  n += smoothValueNoise(pc*8.  + ti*2. ) * .5;
  n += smoothValueNoise(pc*16. + ti*4. ) * .25;
  n += smoothValueNoise(pc*32. + ti*6. ) * .125;
  n += smoothValueNoise(pc*64. + ti*8. ) * .0625;
  n /= 1.5;



  vec2 pc2 = vec2(r, atan(op.y, op.x));

  //   vec2 pc2 = vec2(r,theta2);
  float n2;
  // // ti = 0.;
  n2 += smoothValueNoise(pc*4.          );
  n2 += smoothValueNoise(pc*8.  + ti*2. ) * .5;
  n2 += smoothValueNoise(pc*16. + ti*4. ) * .25;
  n2 += smoothValueNoise(pc*32. + ti*6. ) * .125;
  n2 += smoothValueNoise(pc*64. + ti*8. ) * .0625;
  n2 /= 1.5;







  float test = floor(n*BANDS)/BANDS;
  // c = sdCircle(p, 0.5);

  float b = floor(((length(p))* BANDS))/BANDS;

  c =  length(p) * step(sdRing(p, 2.5, 2.), 0.);// * b;

  // c = step(sdRing(p, .5, 1.1), 0.);

  float i = sin((atan(p.y,p.x) + r)*3.);

  // float i2 = sin(atan(p.y, p.x)+PI) * 3.;

  float sdf = abs(sin((atan(p.y,p.x)+PI)*1.));



  // c *= sdf;// * i2;
  c *= i;

  float sdf2 = abs(sin((atan(op.y,op.x)+PI)*1.));
   c *= n2;

  c += 50./(sdLine(p, vec2(-11., 0.), vec2(11., 0.), 0.00001)* 20000.);

  gl_FragColor = vec4(vec3(c),1.);

  if(c > 0.5){
    // gl_FragColor = vec4(vec3(0.0), 1.0);
  }
}