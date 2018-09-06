precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const int NUM = 10;

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
  float connections[NUM];

  for(int it=0; it < NUM; ++it){
    float fit = float(it);

    vec2 v = vec2(valueNoise((fit+1.) * (fit+1.), fit*89323.712983),
                  valueNoise((fit+1.) * (fit+1.), fit*12392.234923));
    vel[it] = v*2.-1.;

    vec2 p = vec2(valueNoise(fit*fit, fit*19323.523),
                  valueNoise(fit*fit, fit*46239.627));
    pos[it] = p*2.-1.;

    vec2 _pos = pos[it] + vel[it] * t;

    vec2 screenIdx = floor(mod(_pos, 2.));// 0..1
    vec2 dir = screenIdx * 2. - 1.;// remap to  -1..+1
    // dir *= 1.5;
    vec2 finalPos = vec2((1.-screenIdx) + dir * mod(_pos, 1.));

    finalPos =  (finalPos-.5) * vec2(2);

    fpos[it] = finalPos;//(finalPos-.5)*1.5;


  }

   for(int i = 0; i < NUM; ++i){
    for(int j = 0; j < NUM; ++j){
       //if(i >= j) continue;
      vec2 p1 =  fpos[i];
      vec2 p2 =  fpos[j];

      float _dist = (sin(u_time*5.)+1.)/4.;

      vec2 v = p1 - p2;
      float square = dot(v,v);
      // float square = distance(p1, p2);
      if( square < _dist ) {

        connections[i]++;
        // connections[j]++;

        square = _dist - square;
        square = square*square * 40.;
        // square = pow(1.1, square);
        //1./pow(square,2.);

        // square = max(.5, square);

        // inten += step(sdLine(p, -p1, -p2, .005), 0.);

         inten +=  step(sdLine(pp, -p1, -p2,  0.001 * square), 0.);

         inten += abs(2./(sdLine(pp, -p1, -p2,  0.001 * square ) * 3000.));
      }
    }
  }

   for(int it=0; it < NUM; ++it){
      vec2 finalPos = fpos[it];
      float sz = connections[it]*3.;


      inten += (.002)/(sdCircle(pp + finalPos, 0.0014 * sz));
      inten = clamp(inten, 0., 1.);

      inten += step(sdCircle(pp + finalPos, 0.0014 * sz ), 0.);
   }

  gl_FragColor = vec4(vec3(inten),1.);
}