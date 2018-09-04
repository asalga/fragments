precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const int NUM = 14;

float valueNoise(float i, float scale){
  return fract(sin(i*scale));
}

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

float sdLine(vec2 p, vec2 a, vec2 b, float r) {
    vec2 pa = p - a, ba = b - a;
    float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    return length( pa - ba*h ) - r;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  vec2 pp = p;
  float inten;
  float t = u_time *0.25;

  vec2 pos[NUM];
  vec2 vel[NUM];
  vec2 fpos[NUM];

  for(int it=0; it < NUM; ++it){
    float fit = float(it);

    vec2 v = vec2(valueNoise((fit+1.) * (fit+1.), fit*198323.712983),
                  valueNoise((fit+1.) * (fit+1.), fit*128392.234923));
    vel[it] = v*2.-1.;

    vec2 p = vec2(valueNoise(fit*fit, fit*198323.523),
                  valueNoise(fit*fit, fit*9692033.623));
    pos[it] = p*2.-1.;

    vec2 _pos = pos[it] + vel[it] * t;

    vec2 screenIdx = floor(mod(_pos, 2.));// 0..1
    vec2 dir = screenIdx * 2. - 1.;// remap to  -1..+1
    // dir *= 1.5;
    vec2 finalPos = vec2((1.-screenIdx) + dir * mod(_pos, 1.));

    finalPos =  (finalPos-.5) * vec2(2);

    fpos[it] = finalPos;//(finalPos-.5)*1.5;

    // inten += step(sdCircle(pp + finalPos, 0.05),0.);
    inten += .0005/(sdCircle(pp + finalPos, 0.001)/20.);
  }

   for(int i = 0; i < NUM; ++i){
    for(int j = 0; j < NUM; ++j){
       if(i >= j) continue;
      vec2 p1 =  fpos[i];
      vec2 p2 =  fpos[j];

      if( length(p1 - p2) < 0.5 ) {
      // inten +=  step(sdLine(p, -_pos1, -_pos2, .005), 0.);
         inten += abs(2./(sdLine(pp, -p1, -p2, 0.00001) * 2000.));
      }
    }
  }

  gl_FragColor = vec4(vec3(inten),1.);
}