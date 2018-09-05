// void mainImage(o,p)
//   for (float s,i; i<6.; s = mod(i++ - iTime,6.)) {
//     o += texture(iChannel0, (p/ iResolution.xy-.5) * exp(s-2.)) * s * (6. - s) * .05
//   }


// untitled
precision highp float;

uniform vec2 u_res;
uniform float u_time;

float checker(vec2 c) {
  float col;
  float sz = 1.;
  float x = step(mod(c.x,sz), sz/2.);
  float y = step(mod(c.y,sz), sz/2.);
  if(x == y){return 0.8;}
  return x*y;
}

void main(){
  float col = 0.;
  vec2 p = gl_FragCoord.xy/u_res;

  for (float i=0.; i < 6.; i++) {
    float s = mod(i - u_time, 6.);
    col += checker((p/u_res - .5)) * exp(s-2.) * s * (6. - s) * 1.05;
  }

  gl_FragColor = vec4(vec3(col),1);
}
