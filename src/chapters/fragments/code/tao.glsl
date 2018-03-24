precision mediump float;

uniform vec2 u_res;

float c(in vec2 c, in float r){
  vec2 aspect = vec2(u_res.x/u_res.y, 1.);
  vec2 uv = (gl_FragCoord.xy / u_res * 2.0 - 1.0) * aspect;
  return 1.0 - step(r, distance(c, uv));
}

void main(){
  float col = step(0., gl_FragCoord.x / u_res.x * 2. - 1.);
  col += c(vec2(.0, .5), .5) - c(vec2(.0, -.5), .5);
  col += 2. * c(vec2(.0,-.5), .1) - 2. * c(vec2(.0, .5), .1);
  col += (1. - c(vec2(0.), 1.)) * sign((1. - col) * 2. - 1.);
  gl_FragColor = vec4(vec3(col), 1.);
}