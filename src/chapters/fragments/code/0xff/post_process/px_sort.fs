#version 300 es

precision mediump float;

uniform vec2 u_res;
uniform float u_time;
uniform sampler2D u_t0;

#define XAXIS luma
#define YAXIS brightness

float luma( vec3 c ) {
  vec3 final = c * vec3(0.2126, 0.7152, 0.0722);
  return final.x + final.y + final.z;
  // return 0.2126*c.r + 0.7152*c.g + 0.0722*c.b;
}

float brightness( vec3 c ) {
  return c.r + c.g + c.b;
}

void main() {
  vec3 col;
  // if (iFrame<10) {
    // col = texture(iChannel1, gl_FragCoord/u_res.xy).rgb;           //initialize with image
  // }else {
  // col = texelFetch(u_t0, vec2(gl_FragCoord.xy), 0).rgb;
  texelFetch(u_t0, ivec2(0), 0);

  // float ny = min(gl_FragCoord.y+1.0, u_res.y);
  // vec3 c = texelFetch(u_t0, ivec2(gl_FragCoord.x, ny), 0).rgb;

  // if ( YAXIS(col) <= YAXIS(c) ){
  //   col = c;
  // }

  gl_FragColor = vec4(col,1.0);
}