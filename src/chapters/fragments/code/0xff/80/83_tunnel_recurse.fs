precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sampleChecker(vec2 p, float flip, float sz){
  vec2 uv = vec2(step(fract(p), vec2(sz)));
  
  float i = (uv.x == 0. && uv.y == 0.) ? 1. : uv.x * uv.y;

  if(flip == 1.){
    i = 1. - i;
  }
  return i;
}

float sampleTunnel(vec2 st, float time, float flip, float sz){
  vec2 p = st*2.-1.;
  float t = time * 0.4;
  vec2 uv = vec2( fract(abs(1./p.y)+0.) + t*2.,
                  fract(p.x / abs(p.y*2.)));
  
  float i = sampleChecker(uv, flip, .5);

  float fog = pow(abs(p.y), 2.4)*2.;
  return i * fog;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time*0.2;

  // t = -(sin(t)+1.)/2. * 4.; a bit too much


  vec2 np = p;
  vec2 uv = vec2( fract(np.x / abs(np.y) + t),
                  fract(abs(1./np.y)+t*1.) );
  
  float fog = pow(abs(p.y), 2.)*.8;

  float flip = sampleChecker(np, 0., .5);
  float tu = sampleTunnel(uv, u_time, 0., .5);

  float i =  tu;// + flip;
  
  i *= fog;
  gl_FragColor = vec4(vec3(i),1);
}