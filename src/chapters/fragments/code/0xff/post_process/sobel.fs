precision mediump float;

uniform sampler2D u_t0;
uniform vec3  u_mouse;
uniform vec2  u_res;
uniform float u_time;
uniform float _[18];

mat3 sobel = mat3(  -1., .0, 1.,
                    -2., .0, 2.,
                    -1., .0, 1.);

vec4 sample(vec2 offset){
  vec2 p = vec2(gl_FragCoord.xy + offset) / u_res;
  p.y = 1.0 - p.y;
  return texture2D(u_t0, p);
}

void main() {

  vec2 _00 = vec2(_[0], _[1]);
  vec2 _10 = vec2(_[2], _[2]);
  vec2 _20 = vec2(_[4], _[3]);

  vec2 _01 = vec2(_[6], _[7]);
  vec2 _11 = vec2(_[8], _[9]);
  vec2 _21 = vec2(_[10], _[11]);

  vec2 _02 = vec2(_[12], _[13]);
  vec2 _12 = vec2(_[14], _[15]);
  vec2 _22 = vec2(_[16], _[17]);

  vec2 p = gl_FragCoord.xy / u_res;
  p.y = 1.0 - p.y;
  vec4 diffuse1 = texture2D(u_t0, p);

  vec4 colX = 
   sample(_00) * sobel[0][0] + sample(_01) * sobel[0][1] + sample(_02) * sobel[0][2] + 
   sample(_10) * sobel[1][0] + sample(_11) * sobel[1][1] + sample(_12) * sobel[1][2] + 
   sample(_20) * sobel[2][0] + sample(_21) * sobel[2][1] + sample(_22) * sobel[2][2];

  vec4 colY = 
   sample(_01) * sobel[0][0] + sample(_01) * sobel[1][0] + sample(_02) * sobel[2][0] + 
   sample(_12) * sobel[0][1] + sample(_11) * sobel[1][1] + sample(_12) * sobel[2][1] + 
   sample(_20) * sobel[0][2] + sample(_21) * sobel[1][2] + sample(_22) * sobel[2][2];

  float resCol = sqrt(colX.r * colX.r + colY.r * colY.r);

  // gl_FragColor = diffuse1;
  gl_FragColor = vec4( vec3(resCol), 1.0);
}
