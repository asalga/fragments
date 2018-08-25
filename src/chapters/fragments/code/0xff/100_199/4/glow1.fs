precision mediump float;
uniform vec2 u_res;
uniform float u_time;
const float PI = 3.141592658;

void main( void ) {
  vec2 p = ( gl_FragCoord.xy / u_res ) * 2.0 - 1.0;
  float c;
  float t = u_time * 2.;

  for(int it = 0; it < 18; ++it){
    float fit = float(it);
    float x = (sin(fit/15. + t))/1.5;

    c += abs(1. / ((p.y + x) * (900. - fit*44.) ));
    c += abs(1. / (p.x * 900.));
  }

  // c += pow(abs( (p.x+0.) / 3.0), 1.5);
  c += pow(length(p)+0.3, 30.);
  //10. + (sin(u_time)+1.)/2. *31.);

  // hard black border
  c *= step(-.9, p.x) * step(p.y, 0.9);
  c *= step(p.x, 0.9) * step( -0.9, p.y);

  gl_FragColor = vec4( vec3(c), 1 );
}