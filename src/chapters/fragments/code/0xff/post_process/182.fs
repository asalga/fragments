precision mediump float;

uniform vec2 u_res;
uniform float u_time;
uniform sampler2D u_t0;

float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;
  return fract( sin(x+y) * 23454.);
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
  vec2 uv = gl_FragCoord.xy/u_res;
  uv.y = 1.-uv.y;

  float NumSlices = 4.;

  float localCell = (1.+floor(uv.y*NumSlices));

  float intensity = sin(u_time/2.);// + smoothstep(-1., 1., sin(u_time/1.))*0.1;
  uv.y = uv.y + fract( smoothValueNoise(vec2(0., (u_time + (localCell*423.123) )/10. ))) * intensity;
  uv.y = fract(uv.y);

  uv.x = uv.x + fract( smoothValueNoise(vec2(0., (u_time + (localCell*239.5323) )/10. ))) * intensity;
  uv.x = fract(uv.x);

  vec4 col = texture2D(u_t0, uv);

  gl_FragColor = col;
}
