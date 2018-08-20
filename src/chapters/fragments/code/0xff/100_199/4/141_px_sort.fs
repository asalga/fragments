#define XAXIS luma
#define YAXIS brightness

float luma( vec3 c ) {
  return 0.2126*c.r + 0.7152*c.g + 0.0722*c.b;
}

float brightness( vec3 c ) {
  return c.r+c.g+c.b;
}

void mainImage( out vec4 fragColor, in vec2 fragCoord ) {
  vec3 col;

  if (iFrame<10) {
    col = texture(iChannel1, fragCoord/iResolution.xy).rgb;           //initialize with image
  }
  else {
  col = texelFetch(iChannel0, ivec2(fragCoord), 0).rgb;

    float ny = min(fragCoord.y+1.0, iResolution.y);
    vec3 c = texelFetch(iChannel0, ivec2(fragCoord.x, ny), 0).rgb;
    if ( YAXIS(col) <= YAXIS(c) ) col = c;
  }

  fragColor = vec4(col,1.0);
}