// www.shadertoy.com/view/ldKGDd
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define SPEED     -1.
#define STAR_NUMBER 20

vec3 col1 = vec3(155., 176., 255.) / 256.; // Coolest star color
vec3 col2 = vec3(255., 204., 111.) / 256.; // Hottest star color

float rand(float i){
    return fract(sin(dot(vec2(i, i) ,vec2(32.9898,78.233))) * 43758.5453);
}

void main(){

  vec2 uv = gl_FragCoord.xy / u_res.y;
  float res = u_res.x / u_res.y;

  // gl_FragColor = vec4(0.);

  for (int i = 0; i < STAR_NUMBER; ++i){
    float n = float(i);

    //position of the star
    vec3 pos = vec3(rand(n) * res + u_time * SPEED, rand(n + 1.) , rand(n + 2.) * 5.);

    // parralax effect
    pos.x = mod(pos.x * pos.z, res);

    pos.y = (pos.y + rand(n + 10.)) * 0.5;

    //drawing the star
    vec4 col = vec4(pow(length(pos.xy - uv), -1.25) * 0.0002 * pos.z * rand(n + 3.));

    //star flickering
    //col.xyz *= mix(rand(n + 5.), 1.0, abs(cos(u_time * rand(n + 76.) * 10.)));

    // gl_FragColor += vec4(col);
  }

  vec4 col = vec4(rand(uv.x * uv.y));

  // col.xyz *= 0.5 - abs((uv.y - 0.5));

  if (rand(uv.x * uv.y) > 0.99){
    gl_FragColor += col * 0.25;
  }

  col = 0.5 - vec4(length(vec2(uv.x, 0.5) - uv));
  col.xyz *= col2;

  // float f = .92;
  // vec4 shifted = vec4(1.- f*uv.x,0.,1. - f*(1. -uv.x),1.);
  // col += mix(vec4(0.),shifted,.075);

  gl_FragColor += col;
}