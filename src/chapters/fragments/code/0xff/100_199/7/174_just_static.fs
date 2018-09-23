precision highp float;

uniform vec2 u_res;
uniform float u_time;

const float PI = 3.141592658;
const float TAU = PI*2.;
const float HALF_PI = PI/2.;

const float Rings = 8.;

// from iq
float sdRoundBox( in vec2 p, in vec2 b, in float r ) {
    vec2 q = abs(p) - b;
    vec2 m = vec2( min(q.x,q.y), max(q.x,q.y) );
    float d = (m.x > 0.0) ? length(q) : m.y;
    return d - r;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float t = u_time;
  float i;
  float cnt = 4.;

  p.y -= t*2.;
  vec2 c = vec2(0., 1.);
  p = mod(p, c)-c*0.5;

  i = step(sdRoundBox(p, vec2(0.45,0.25), 0.1), 0.);

  i = 1.-i;

  gl_FragColor = vec4(vec3(i),1);
}