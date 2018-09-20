precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 v, float r){
  return length(v)-r;
}

// float valueNoise(vec2 v){
//   vec2 r = sin(v * vec2(2983.02389, 243.230));
//   return fract(r.x*r.y);
// }

float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;
  return fract( sin(x+y) * 23454.);
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float i;
  float t = u_time;

 // for(float j = -1.0;j < 1.0; j += 0.5){
float j = 1.;

    for(float it = -1.0; it < 1.0; it += 0.5){
      vec2 pos = p;
      // pos.x += it;
      pos.x = fract(pos.x+it/1. + t) * 2.-1.;
      float r = valueNoise(vec2(it + pow(t , 0.0000001) , j));
      pos.y += (r*2.-1.)*0.575;

      // pos.y += j;

      float d = sdCircle(pos, 0.125);
      d = .01/dot(d,d);

      i += d;
    }

 // }

  // for(float it=-1.;it < 1.;it += 0.2){
  //   // float r = valueNoise(vec2(it,it));

  //   i = step(sdCircle(p + vec2(test,0.), 0.125), 0.);

  //   test += 0.01;
  // }


  gl_FragColor = vec4(vec3(i),1.);
}