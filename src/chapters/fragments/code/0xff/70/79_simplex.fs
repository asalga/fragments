precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define Y_SCALE 45343.
#define X_SCALE 37738.

/*
*/
float valueNoise(float seed, vec2 p){
  
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;
  
  return fract( sin(x+y) * (23454. + seed));
}

void main(){
  // vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = gl_FragCoord.xy/u_res;
  float c;
  float t = u_time * 1.;

  c = valueNoise(0., p);

  // p.x += t * valueNoise();

  p.x += t * (0.5 + valueNoise(3423., floor(vec2(p.y*10.)) ));

  vec2 lv = fract(p*10.);
  vec2 id = floor(p*10.);


  

  // we can't just scale the uv/p value
  // because that will reveal the pattern of the
  // sin wave. Instead, we interpolate between
  // the corners of the grid
  float bl = valueNoise(0., vec2(id));
  float br = valueNoise(0., vec2(id)+vec2(1.,0.));
  // float tr = valueNoise(vec2(id)+vec2(0.,1.));
  float b = mix(bl,br,lv.x);



  // c.rg += lv;
  // i += lv;

  vec3 col = vec3(b);
  // col.rg = id/10.;
  
  gl_FragColor = vec4(col,1);
}