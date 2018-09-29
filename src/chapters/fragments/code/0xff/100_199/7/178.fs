precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float CNT = 7.;
const int iCNT = 7;
uniform float contribs[iCNT];

float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;
  return fract( sin(x+y) * 23454.);
}

float grid(float x){
  vec2 p = gl_FragCoord.xy;
  p.x += x* 56.*7.;
  // p.x += u_time*0.25*56.5*7.;
  vec2 lineWidthInPx = vec2(4.);
  vec2 cellSize = vec2(56.5);
  vec2 i = step(mod(p, cellSize), lineWidthInPx);
  return i.x + i.y;
}


float smoothValueNoise(vec2 p){
  vec2 lv = fract(p);
  lv = smoothstep(0.,1.,lv);
  vec2 id = floor(p);
  float bl = valueNoise(vec2(id));
  float br = valueNoise(vec2(id)+vec2(1,0));
  float b = mix(bl,br,lv.x);

  float tl = valueNoise(vec2(id)+vec2(0,1));
  float tr = valueNoise(vec2(id)+vec2(1,1));
  float t = mix(tl,tr,lv.x);

  return mix(b,t,lv.y);
}



void main(){
  vec2 p = gl_FragCoord.xy/u_res;
  float i;

  float timeScale = 1.;
  float t = (u_time+25.) * timeScale;

  p.x += t*0.125;
  p.y = 1.-p.y;

  vec2 cell = floor(p*CNT)/CNT;
  vec2 lc =  p*CNT;

  i = floor(smoothValueNoise(cell*15.)*CNT)/CNT;


  // on weekends it should be slightly less
  if(lc.y <= 1. || lc.y > 6.){
    i = .92;
  }


  if( lc.x > floor(t+1.)){
    i = contribs[0];
  }



  if(lc.x > floor(t) && lc.x < floor(t+1.) &&
    // cell.x > t   && cell.x < t+1. ){
    cell.y > floor(fract(t)*7.)/7. )  {
    i = contribs[0];
  }




  if( fract(lc.y) < 0.05){
    i = 1.;
  }
  if( fract(lc.x) < 0.05){
    i = 1.;
  }

  // vec2 g = floor(p*CNT)/CNT;
  // float g = grid(offset);
  // i += g;
  gl_FragColor = vec4(vec3(i),1.);
  // gl_FragColor = vec4(vec3(i),1.);
}
