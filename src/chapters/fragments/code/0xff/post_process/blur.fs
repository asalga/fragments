precision mediump float;

const float kernelSize = 4.;
uniform sampler2D u_t0;
uniform vec2 u_res;

void main() {
  vec2 p = (gl_FragCoord.xy / u_res.xy);
  p.y = 1.0 - p.y;

  vec4 c = vec4(0.0);
  const float sz = floor(float(kernelSize)/2.0);

  for(float x = 0.; x < sz; ++x){
    for(float y = 0.; y < sz; ++y){
      vec2 uv = (gl_FragCoord.xy + vec2(x, y)) / u_res.xy;
      uv.y = 1.0 - uv.y;
      // uv.x += 12.;
      c += texture2D(u_t0, uv);
    }
  }
  c /= kernelSize;
  gl_FragColor = c;
}
