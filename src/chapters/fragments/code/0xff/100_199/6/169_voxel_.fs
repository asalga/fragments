precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float STEP = 0.02;
const float MAX_DIST = 20.;
const float SCALE_STEP = 0.01;

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.0;
  float z = size.y / tan(radians(fieldOfView) / 2.0);
  return normalize(vec3(xy, z));
}

void main() {
  vec2 pc = gl_FragCoord.xy/u_res;
  float t = u_time;
  vec3 color;

  vec3 rayOrigin = vec3(0., 10., t*5.);
  vec3 rayDir = vec3(pc-.5, .5);

  float density = .25;

  vec3 flr;
  vec3 frct;
  float depth;

  for(float i = 0.; i < MAX_DIST; i += STEP) {

    vec3 p = rayOrigin + (depth * rayDir);

    flr = floor(p) * density;
    // flr.z *= abs(sin(t/10.));
    // flr.x *= abs(cos(t/20.));

    // float n = 0.;
    // float n2 = 0.;
    // if(flr.x>0.)flr.x = 0.;

    // flr.x = pow(flr.x, 2.);

    // float cs = n;
    // * abs(sin(t/50.));
    // float sn = n2;
    // = 0.;
    // = sin(flr.x);// * abs(sin(t/25. ));
    // sn = flr.x;

    float g = sin(flr.x) + cos(flr.z);

    if(g > flr.y ) {  // hit
      frct = fract(p);

      float zLine = step(frct.z, 0.92);
      float xLine = step(frct.x, 0.92);

      float xx = step(mod(flr.x, 1.), 0.25)*0.25;
      float zz = step(mod(flr.z, 1.), 0.25)*0.75;

      color = vec3( xx+zz );

      color *= 0.4+flr.y;

      if(xx == 1. && zz == 1.){
        color = vec3(0.);
      }

      // color = vec3(1.) * zLine * xLine;

      // color white on top, black on bottom
      // color = vec3(step(0.5, frct.y) * zLine * xLine);
      break;
    }

    depth += i * SCALE_STEP;
  }

  gl_FragColor = vec4(color, 1);
}